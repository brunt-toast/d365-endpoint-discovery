using Dev.JoshBrunton.DynamicsEndpointDiscovery.Core.Extensions.Serilog;
using Dev.JoshBrunton.DynamicsEndpointDiscovery.Lib.Ax.Config;
using Dev.JoshBrunton.DynamicsEndpointDiscovery.Lib.Ax.Types.Soap;
using Serilog;
using System.Net;
using System.Text.Json;
using System.Xml.Linq;
using Newtonsoft.Json;

namespace Dev.JoshBrunton.DynamicsEndpointDiscovery.Lib.Ax.Services.Soap;

internal class AxSoapService : IAxSoapService
{
    private readonly AxAuthService _authSvc;
    private readonly IAxConfig _config;
    private readonly ILogger _logger;
    private readonly IAxSvcDiscoveryService _svcDiscoveryService;

    public AxSoapService(AxAuthService authSvc, IAxConfig config, ILogger logger, IAxSvcDiscoveryService svcDiscoveryService)
    {
        _authSvc = authSvc;
        _config = config;
        _logger = logger;
        _svcDiscoveryService = svcDiscoveryService;
    }

    public async Task<Dictionary<string, string>> GetDataContractsForServices()
    {
        return await GetDataContractsForServices((await _svcDiscoveryService.GetAllGroups()).Select(x => x.Name));
    }

    public async Task<Dictionary<string,string>> GetDataContractsForServices(IEnumerable<string> serviceNames)
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
        
        return parsed.Select(x =>
        {
            var tree = treeBuilder.Build(parsed, x.Name);
            var defaultObject = DefaultValueGenerator.Generate(tree);
            var json = JsonConvert.SerializeObject(defaultObject, Formatting.Indented);
            return new KeyValuePair<string, string>(x.Name, json);
        }).ToDictionary(); 
    }

    private async Task<IEnumerable<string>> GetWsdlLocationsForService(string serviceName)
    {
        try
        {
            string httpResponse = await GetHttp($"{_config.Resource}/soap/services/{serviceName}?wsdl");
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
        string httpResponse = await GetHttp(wsdlUri);
        var doc = XDocument.Parse(httpResponse);
        XNamespace xsd = "http://www.w3.org/2001/XMLSchema";

        return doc.Descendants(xsd + "import")
            .Select(e => (string?)e.Attribute("schemaLocation"))
            .Where(s => !string.IsNullOrWhiteSpace(s))
            .Cast<string>();
    }

    private async Task<string> GetXsd(string xsdUri)
    {
        string httpResponse = await GetHttp(xsdUri);
        return httpResponse;
    }

    private async Task<string> GetHttp(string endpoint)
    {
        string bearer = await _authSvc.GetBearerToken();
        using var client = new HttpClient(new HttpClientHandler
        {
            ServerCertificateCustomValidationCallback =
                HttpClientHandler.DangerousAcceptAnyServerCertificateValidator
        });

        HttpRequestMessage request = new(HttpMethod.Get, endpoint);
        request.Headers.Clear();
        request.Headers.Add("Authorization", $"Bearer {bearer}");
        var response = await client.SendAsync(request);

        string content = await response.Content.ReadAsStringAsync();

        if (response.StatusCode == HttpStatusCode.TooManyRequests)
        {
            _logger.LogError("Too many requests! ({endpoint})", endpoint);
        }
        else if (!response.IsSuccessStatusCode)
        {
            _logger.LogError("A request to {endpoint} returned HTTP status {statusInt} ({status}). Content was: {newLine}{content}", endpoint, (int)response.StatusCode, response.StatusCode, Environment.NewLine, content);
        }

        return content;
    }
}

public interface IAxSoapService
{
    Task<Dictionary<string,string>> GetDataContractsForServices();
    Task<Dictionary<string, string>> GetDataContractsForServices(IEnumerable<string> serviceNames);
}
