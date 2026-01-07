using System.Text.RegularExpressions;
using Dev.JoshBrunton.DynamicsEndpointDiscovery.Lib.Ax.Config;
using Dev.JoshBrunton.DynamicsEndpointDiscovery.Lib.Ax.Responses;
using Dev.JoshBrunton.DynamicsEndpointDiscovery.Lib.Ax.Types;
using Newtonsoft.Json;
using Serilog;
using Dev.JoshBrunton.DynamicsEndpointDiscovery.Core.Extensions.Serilog;

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

        var groups = (await GetAllGroups()).Where(x => grepGroupsRegex.IsMatch(x.Name)).ToList();
        var services = (await GetServicesForGroups(groups)).Where(x => grepServicesRegex.IsMatch(x.Name)).ToList();
        var operations = (await GetOperationsForServices(services)).Where(x => grepOperationsRegex.IsMatch(x.Name)).ToList();

        foreach (var group in groups)
        {
            group.Services = services.Where(x => x.ServiceGroupName == group.Name).ToArray();
            foreach (var service in group.Services)
            {
                service.Operations = operations.Where(x => x.ServiceGroupName == group.Name && x.ServiceName == service.Name).ToArray();
            }
        }
        
        return groups;
    }

    public async Task<IEnumerable<DynSvcGroup>> GetAllGroups()
    {
        var res = JsonConvert.DeserializeObject<GetSvcGroupsResponse>(await GetHttp($"{_config.Resource}/api/services")) ?? throw new ArgumentNullException();
        return res.Groups;
    }

    public async Task<IEnumerable<DynSvc>> GetServicesForGroups(IEnumerable<DynSvcGroup> groups)
    {
        var ret = await Task.WhenAll(groups.Select(GetServicesForGroup));
        return ret.SelectMany(x => x);
    }

    private async Task<IEnumerable<DynSvc>> GetServicesForGroup(DynSvcGroup group)
    {
        var res = JsonConvert.DeserializeObject<GetSvcGroupResponse>(await GetHttp($"{_config.Resource}/api/services/{group.Name}")) ?? throw new ArgumentNullException();
        foreach (var service in res.Services)
        {
            service.ServiceGroupName = group.Name;
        }
        return res.Services;
    }

    public async Task<IEnumerable<DynSvcOp>> GetOperationsForServices(IEnumerable<DynSvc> services)
    {
        var ret = await Task.WhenAll(services.Select(GetOperationsForService));
        return ret.SelectMany(x => x);
    }

    private async Task<IEnumerable<DynSvcOp>> GetOperationsForService(DynSvc service)
    {
        var res = JsonConvert.DeserializeObject<GetSvcResponse>(await GetHttp($"{_config.Resource}/api/services/{service.ServiceGroupName}/{service.Name}")) ?? throw new ArgumentNullException();
        await Task.WhenAll(res.Operations.Select(x => MutateOperationWithParamsAndReturnType(service, x)));
        return res.Operations;
    }

    private async Task MutateOperationWithParamsAndReturnType(DynSvc service, DynSvcOp operation)
    {
        var opRes = JsonConvert.DeserializeObject<GetOperationResponse>(await GetHttp($"{_config.Resource}/api/services/{service.ServiceGroupName}/{service.Name}/{operation.Name}")) ?? throw new ArgumentNullException();
        operation.ServiceGroupName = service.ServiceGroupName;
        operation.ServiceName = service.Name;
        operation.Parameters = opRes.Parameters;
        operation.Return = opRes.Return;
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

    Task<IEnumerable<DynSvcGroup>> GetAllGroups();
    Task<IEnumerable<DynSvc>> GetServicesForGroups(IEnumerable<DynSvcGroup> groups);
    Task<IEnumerable<DynSvcOp>> GetOperationsForServices(IEnumerable<DynSvc> services);
}