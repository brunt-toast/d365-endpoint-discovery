using System.Text.RegularExpressions;
using Dev.JoshBrunton.DynamicsEndpointDiscovery.Lib.Ax.Config;
using Dev.JoshBrunton.DynamicsEndpointDiscovery.Lib.Ax.Responses;
using Dev.JoshBrunton.DynamicsEndpointDiscovery.Lib.Ax.Types;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace Dev.JoshBrunton.DynamicsEndpointDiscovery.Lib.Ax.Services;

internal class AxSvcDiscoveryService : IAxScvDiscoveryService
{
    private readonly AxAuthService _authSvc;
    private readonly IAxConfig _config;
    private readonly ILogger _logger;

    public AxSvcDiscoveryService(AxAuthService authSvc, IAxConfig config, ILogger logger, Regex? grepGroupsRegex = null, Regex? grepServicesRegex = null, Regex? grepOperationsRegex = null)
    {
        _authSvc = authSvc;
        _config = config;
        _logger = logger;
    }

    public async Task<IEnumerable<DynSvcGroup>> MapServicesAsync(string grepGroupsRegexString = ".*", 
        string grepServicesRegexString = ".*", 
        string grepOperationsRegexString = ".*")
    {
        Regex grepGroupsRegex = new Regex(grepGroupsRegexString);
        Regex grepServicesRegex = new Regex(grepServicesRegexString);
        Regex grepOperationsRegex = new Regex(grepOperationsRegexString);

        _logger.LogInformation("Mapping services");
        var res = JsonConvert.DeserializeObject<GetSvcGroupsResponse>(await GetHttp($"{_config.Resource}/api/services")) ?? throw new ArgumentNullException();

        foreach (var group in res.Groups.Where(x => grepGroupsRegex?.IsMatch(x.Name) ?? true))
        {
            group.Services = (await GetServices(group.Name, grepServicesRegex, grepOperationsRegex)).ToArray();
        }

        return res.Groups.Where(x => grepGroupsRegex?.IsMatch(x.Name) ?? true);
    }

    private async Task<IEnumerable<DynSvc>> GetServices(string group, Regex grepServicesRegex, Regex grepOperationsRegex)
    {
        _logger.LogInformation("Getting services for group {group}", group);
        var res = JsonConvert.DeserializeObject<GetSvcGroupResponse>(await GetHttp($"{_config.Resource}/api/services/{group}")) ?? throw new ArgumentNullException();

        foreach (var service in res.Services.Where(x => grepServicesRegex?.IsMatch(x.Name) ?? true))
        {
            service.ServiceGroupName = group;
            service.Operations = (await GetOperations(group, service.Name, grepOperationsRegex)).ToArray();
        }

        return res.Services.Where(x => grepServicesRegex?.IsMatch(x.Name) ?? true);
    }

    private async Task<IEnumerable<DynSvcOp>> GetOperations(string group, string service, Regex grepOperationsRegex)
    {
        _logger.LogInformation("Getting services for group {group}'s service {service}", group, service);
        var res = JsonConvert.DeserializeObject<GetSvcResponse>(await GetHttp($"{_config.Resource}/api/services/{group}/{service}")) ?? throw new ArgumentNullException();

        foreach (var operation in res.Operations.Where(x => grepOperationsRegex?.IsMatch(x.Name) ?? true))
        {
            var opRes = JsonConvert.DeserializeObject<GetOperationResponse>(await GetHttp($"{_config.Resource}/api/services/{group}/{service}/{operation.Name}")) ?? throw new ArgumentNullException();

            operation.ServiceGroupName = group;
            operation.ServiceName = service;
            operation.Parameters = opRes.Parameters;
            operation.Return = opRes.Return;
        }

        return res.Operations.Where(x => grepOperationsRegex?.IsMatch(x.Name) ?? true);
    }

    private async Task<string> GetHttp(string endpoint)
    {
        string bearer = await _authSvc.GetBearerToken();
        using HttpClient client = new();
        HttpRequestMessage request = new(HttpMethod.Get, endpoint);
        request.Headers.Clear();
        request.Headers.Add("Authorization", $"Bearer {bearer}");
        var response = await client.SendAsync(request);

        string content = await response.Content.ReadAsStringAsync();
        if (!response.IsSuccessStatusCode)
        {
            _logger.LogError("A request to {endpoint} returned HTTP status {statusInt} ({status}). Content was: {newLine}{content}", endpoint, (int)response.StatusCode, response.StatusCode, Environment.NewLine, content);
        }

        return content;
    }
}

public interface IAxScvDiscoveryService
{
    Task<IEnumerable<DynSvcGroup>> MapServicesAsync(string grepGroupsRegexString = ".*",
        string grepServicesRegexString = ".*",
        string grepOperationsRegexString = ".*");
}