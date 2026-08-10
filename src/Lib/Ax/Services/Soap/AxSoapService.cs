using System.Xml.Linq;
using Dev.JoshBrunton.DynamicsEndpointDiscovery.Core.Extensions.Serilog;
using Dev.JoshBrunton.DynamicsEndpointDiscovery.Lib.Ax.Services.Metadata;
using Dev.JoshBrunton.DynamicsEndpointDiscovery.Lib.Ax.Types.Soap;
using Newtonsoft.Json;
using Serilog;

namespace Dev.JoshBrunton.DynamicsEndpointDiscovery.Lib.Ax.Services.Soap;

internal class AxSoapService : IAxSoapService
{
    private const int MaxConcurrentMetadataRequests = 8;

    private readonly AxCallingService _axCalling;
    private readonly ILogger _logger;
    private readonly AxMetadataLabelService _labelService;

    public AxSoapService(AxCallingService axCalling, ILogger logger, AxMetadataLabelService labelService)
    {
        _axCalling = axCalling;
        _logger = logger;
        _labelService = labelService;
    }

    public async Task<SoapTypeCollection> GetDataContractsForServices(IEnumerable<string> serviceNames)
    {
        var getWsdlLocationsResults = await RunMetadataRequests(serviceNames, GetWsdlLocationsForService);
        var wsdlUris = getWsdlLocationsResults.SelectMany(x => x);

        var getXsdLocationsResult = await RunMetadataRequests(wsdlUris, GetXsdSchemaLocationsForWsdlUri);
        var xsdLocations = getXsdLocationsResult.SelectMany(x => x);

        var getXsdResult = await RunMetadataRequests(xsdLocations, GetXsd);

        XNamespace xs = "http://www.w3.org/2001/XMLSchema";

        var parsed = getXsdResult.Where(x => !string.IsNullOrWhiteSpace(x))
            .Select(XDocument.Parse)
            .SelectMany(x => x.Descendants(xs + "complexType"))
            .Select(AxDataContractDefn.Parse)
            .DistinctBy(x => x.Name)
            .ToList();

        var inheritance = new SoapDataContractInheritanceResolver(_logger);
        var treeBuilder = new TypeTreeBuilder(inheritance);

        NormalizeArrayUsages(parsed, DetectArrayWrappers(parsed));

        var samples = parsed.Select(x =>
        {
            var tree = treeBuilder.Build(parsed, x.Name);
            var defaultObject = DefaultValueGenerator.Generate(tree);
            var json = JsonConvert.SerializeObject(defaultObject, Formatting.Indented);
            return new KeyValuePair<string, string>(x.Name, json);
        }).ToDictionary();

        var labelIds = parsed
            .Select(x => x.LabelId)
            .Concat(parsed.SelectMany(x => x.Properties.Select(y => y.LabelId)));

        return new SoapTypeCollection
        {
            Samples = samples,
            Definitions = parsed,
            Localisations = await _labelService.GetLabels(labelIds)
        };
    }

    private static async Task<TResult[]> RunMetadataRequests<TInput, TResult>(IEnumerable<TInput> inputs, Func<TInput, Task<TResult>> action)
    {
        var throttler = new SemaphoreSlim(MaxConcurrentMetadataRequests, MaxConcurrentMetadataRequests);
        var tasks = inputs.Select(async input =>
        {
            await throttler.WaitAsync();
            try
            {
                return await action(input);
            }
            finally
            {
                throttler.Release();
            }
        });

        return await Task.WhenAll(tasks);
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
        try
        {
            string httpResponse = await _axCalling.GetHttp(ToEndpoint(wsdlUri));
            var doc = XDocument.Parse(httpResponse);
            XNamespace xsd = "http://www.w3.org/2001/XMLSchema";

            return doc.Descendants(xsd + "import")
                .Select(e => (string?)e.Attribute("schemaLocation"))
                .Where(s => !string.IsNullOrWhiteSpace(s))
                .Cast<string>();
        }
        catch (Exception ex)
        {
            _logger.LogError("Error while getting XSD locations for WSDL {wsdlUri}: {ex}", wsdlUri, ex.Message);
            return [];
        }
    }

    private async Task<string> GetXsd(string xsdUri)
    {
        try
        {
            string httpResponse = await _axCalling.GetHttp(ToEndpoint(xsdUri));
            return httpResponse;
        }
        catch (Exception ex)
        {
            _logger.LogError("Error while getting XSD {xsdUri}: {ex}", xsdUri, ex.Message);
            return string.Empty;
        }
    }

    private static string ToEndpoint(string uriOrEndpoint)
    {
        return Uri.TryCreate(uriOrEndpoint, UriKind.Absolute, out var absolute)
            ? absolute.PathAndQuery
            : uriOrEndpoint;
    }
}
