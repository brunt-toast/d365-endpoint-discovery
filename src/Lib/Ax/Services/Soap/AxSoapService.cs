using Dev.JoshBrunton.DynamicsEndpointDiscovery.Core.Extensions.Serilog;
using Dev.JoshBrunton.DynamicsEndpointDiscovery.Lib.Ax.Types.Soap;
using Serilog;
using System.Xml.Linq;
using Newtonsoft.Json;

namespace Dev.JoshBrunton.DynamicsEndpointDiscovery.Lib.Ax.Services.Soap;

internal class AxSoapService : IAxSoapService
{
    private readonly AxCallingService _axCalling;
    private readonly ILogger _logger;

    public AxSoapService(AxCallingService axCalling, ILogger logger)
    {
        _axCalling = axCalling;
        _logger = logger;
    }

    public async Task<Dictionary<string, string>> GetDataContractsForServices(IEnumerable<string> serviceNames)
    {
        var getWsdlLocationsTasks = serviceNames.Select(GetWsdlLocationsForService);
        var getWsdlLocationsResults = await Task.WhenAll(getWsdlLocationsTasks);
        var wsdlUris = getWsdlLocationsResults.SelectMany(x => x);

        var getXsdLocationsTasks = wsdlUris.Select(GetXsdSchemaLocationsForWsdlUri);
        var getXsdLocationsResult = await Task.WhenAll(getXsdLocationsTasks);
        var xsdLocations = getXsdLocationsResult.SelectMany(x => x);

        var getXsdTasks = xsdLocations.Select(GetXsd);
        var getXsdResult = await Task.WhenAll(getXsdTasks);

        XNamespace xs = "http://www.w3.org/2001/XMLSchema";

        var parsed = getXsdResult.Select(XDocument.Parse)
            .SelectMany(x => x.Descendants(xs + "complexType"))
            .Select(AxDataContractDefn.Parse)
            .DistinctBy(x => x.Name)
            .ToList();

        var inheritance = new SoapDataContractInheritanceResolver(_logger);
        var treeBuilder = new TypeTreeBuilder(inheritance);

        NormalizeArrayUsages(parsed, DetectArrayWrappers(parsed));

        return parsed.Select(x =>
        {
            var tree = treeBuilder.Build(parsed, x.Name);
            var defaultObject = DefaultValueGenerator.Generate(tree);
            var json = JsonConvert.SerializeObject(defaultObject, Formatting.Indented);
            return new KeyValuePair<string, string>(x.Name, json);
        }).ToDictionary();
    }

    private static Dictionary<string, string> DetectArrayWrappers(IEnumerable<AxDataContractDefn> types)
    {
        var map = new Dictionary<string, string>();

        foreach (var t in types)
        {
            if (t.Properties.Length != 1)
            {
                continue;
            }

            var p = t.Properties[0];

            if (!p.IsCollection)
            {
                continue;
            }

            map[t.Name] = p.Type;
        }

        return map;
    }

    private static void NormalizeArrayUsages(IEnumerable<AxDataContractDefn> types, Dictionary<string, string> wrappers)
    {
        foreach (var t in types)
        {
            for (int i = 0; i < t.Properties.Length; i++)
            {
                if (wrappers.TryGetValue(t.Properties[i].Type, out var itemType))
                {
                    t.Properties[i] = t.Properties[i] with
                    {
                        Type = itemType,
                        MaximumOccurances = null
                    };
                }
            }
        }
    }
    
    private async Task<IEnumerable<string>> GetWsdlLocationsForService(string serviceName)
    {
        try
        {
            string httpResponse = await _axCalling.GetHttp($"/soap/services/{serviceName}?wsdl");
            var doc = XDocument.Parse(httpResponse);
            XNamespace wsdl = "http://schemas.xmlsoap.org/wsdl/";

            return doc.Descendants(wsdl + "import")
                .Select(e => (string?)e.Attribute("location"))
                .Where(s => !string.IsNullOrWhiteSpace(s))
                .Cast<string>();
        }
        catch (Exception ex)
        {
            _logger.LogError("Error while getting WSDL locations for service {sName}: {ex}", serviceName, ex.Message);
            return [];
        }
    }

    private async Task<IEnumerable<string>> GetXsdSchemaLocationsForWsdlUri(string wsdlUri)
    {
        string httpResponse = await _axCalling.GetHttp(ToEndpoint(wsdlUri));
        var doc = XDocument.Parse(httpResponse);
        XNamespace xsd = "http://www.w3.org/2001/XMLSchema";

        return doc.Descendants(xsd + "import")
            .Select(e => (string?)e.Attribute("schemaLocation"))
            .Where(s => !string.IsNullOrWhiteSpace(s))
            .Cast<string>();
    }

    private async Task<string> GetXsd(string xsdUri)
    {
        string httpResponse = await _axCalling.GetHttp(ToEndpoint(xsdUri));
        return httpResponse;
    }

    private static string ToEndpoint(string uriOrEndpoint)
    {
        return Uri.TryCreate(uriOrEndpoint, UriKind.Absolute, out var absolute)
            ? absolute.PathAndQuery
            : uriOrEndpoint;
    }
}

public interface IAxSoapService
{
    Task<Dictionary<string, string>> GetDataContractsForServices(IEnumerable<string> serviceNames);
}
