using System.Text.RegularExpressions;
using DynamicsEndpointDiscovery.Application.Config;
using DynamicsEndpointDiscovery.Application.Responses;
using DynamicsEndpointDiscovery.Application.Types;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace DynamicsEndpointDiscovery.Application.Services.Dynamics;

public class DynSvcDiscoveryService
{
    private readonly DynAuthService _authSvc;
    private readonly string _resource;
    private readonly ILogger _logger;
    private readonly Regex? _grepGroupsRegex;
    private readonly Regex? _grepServicesRegex;
    private readonly Regex? _grepOperationsRegex;

    public DynSvcDiscoveryService(DynAuthService authSvc, AppConfig config, ILogger logger, Regex? grepGroupsRegex = null, Regex? grepServicesRegex = null, Regex? grepOperationsRegex = null)
    {
        _authSvc = authSvc;
        _logger = logger;
        _resource = config.Resource;

        _grepGroupsRegex = grepGroupsRegex;
        _grepServicesRegex = grepServicesRegex;
        _grepOperationsRegex = grepOperationsRegex;
    }

    public IEnumerable<DynSvcGroup> MapServices()
    {
        _logger.LogInformation("Mapping services");
        var res = JsonConvert.DeserializeObject<DynGetSvcGroupsResponse>(GetHttp($"{_resource}/api/services")) ?? throw new ArgumentNullException();

        foreach (var group in res.Groups.Where(x => _grepGroupsRegex?.IsMatch(x.Name) ?? true))
        {
            group.Services = GetServices(group.Name).ToArray();
            yield return group;
        }
    }

    private IEnumerable<DynSvc> GetServices(string group)
    {
        _logger.LogInformation("Getting services for group {group}", group);
        var res = JsonConvert.DeserializeObject<DynGetSvcGroupResponse>(GetHttp($"{_resource}/api/services/{group}")) ?? throw new ArgumentNullException();

        foreach (var service in res.Services.Where(x => _grepServicesRegex?.IsMatch(x.Name) ?? true))
        {
            service.ServiceGroupName = group;
            service.Operations = GetOperations(group, service.Name).ToArray();
            yield return service;
        }
    }

    private IEnumerable<DynSvcOp> GetOperations(string group, string service)
    {
        _logger.LogInformation("Getting services for group {group}'s service {service}", group, service);
        var res = JsonConvert.DeserializeObject<DynGetSvcResponse>(GetHttp($"{_resource}/api/services/{group}/{service}")) ?? throw new ArgumentNullException();

        foreach (var operation in res.Operations.Where(x => _grepOperationsRegex?.IsMatch(x.Name) ?? true))
        {
            var opRes = JsonConvert.DeserializeObject<DynGetOperationResponse>(GetHttp($"{_resource}/api/services/{group}/{service}/{operation.Name}")) ?? throw new ArgumentNullException();

            operation.ServiceGroupName = group;
            operation.ServiceName = service;
            operation.Parameters = opRes.Parameters;
            operation.Return = opRes.Return;
            yield return operation;
        }
    }

    private string GetHttp(string endpoint)
    {
        string bearer = _authSvc.GetBearerToken();
        using HttpClient client = new();
        HttpRequestMessage request = new(HttpMethod.Get, endpoint);
        request.Headers.Clear();
        request.Headers.Add("Authorization", $"Bearer {bearer}");
        var response = client.Send(request);

        string content = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();
        if (!response.IsSuccessStatusCode)
        {
            _logger.LogError("A request to {endpoint} returned HTTP status {statusInt} ({status}). Content was: {newLine}{content}", endpoint, (int)response.StatusCode, response.StatusCode, Environment.NewLine, content);
        }

        return content;
    }
}
