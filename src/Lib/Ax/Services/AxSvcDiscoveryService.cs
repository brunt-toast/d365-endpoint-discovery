using System.Text.RegularExpressions;
using Dev.JoshBrunton.DynamicsEndpointDiscovery.Lib.Ax.Responses;
using Dev.JoshBrunton.DynamicsEndpointDiscovery.Lib.Ax.Types;
using Serilog;
using Dev.JoshBrunton.DynamicsEndpointDiscovery.Core.Extensions.Serilog;

namespace Dev.JoshBrunton.DynamicsEndpointDiscovery.Lib.Ax.Services;

internal class AxSvcDiscoveryService : IAxSvcDiscoveryService
{
    private readonly AxCallingService _axCalling;
    private readonly ILogger _logger;
    private readonly IJsonConverterService _jsonConverter;

    public AxSvcDiscoveryService(AxCallingService axCalling, ILogger logger, IJsonConverterService jsonConverter)
    {
        _axCalling = axCalling;
        _logger = logger;
        _jsonConverter = jsonConverter;
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
        if (!_jsonConverter.TryDeserialise(await _axCalling.GetHttp("/api/services"),
                out GetSvcGroupsResponse? res))
        {
            _logger.LogError("Deserialisation error while getting all groups.");
            return [];
        }

        _logger.LogInformation("Discovered {n} groups", res.Groups.Length);
        return res.Groups;
    }

    public async Task<IEnumerable<DynSvc>> GetServicesForGroups(IEnumerable<DynSvcGroup> groups)
    {
        var ret = await Task.WhenAll(groups.Select(GetServicesForGroup));
        return ret.SelectMany(x => x);
    }

    private async Task<IEnumerable<DynSvc>> GetServicesForGroup(DynSvcGroup group)
    {
        if (!_jsonConverter.TryDeserialise(await _axCalling.GetHttp($"/api/services/{group.Name}"), out GetSvcGroupResponse? res))
        {
            _logger.LogError("Deserialisation error while getting services for group {group}", group.Name);
            return [];
        }

        foreach (var service in res.Services)
        {
            service.ServiceGroupName = group.Name;
        }

        _logger.LogInformation("Discovered {n} services for group {group}", res.Services.Length, group.Name);
        return res.Services;

    }

    public async Task<IEnumerable<DynSvcOp>> GetOperationsForServices(IEnumerable<DynSvc> services)
    {
        var ret = await Task.WhenAll(services.Select(GetOperationsForService));
        return ret.SelectMany(x => x);
    }

    private async Task<IEnumerable<DynSvcOp>> GetOperationsForService(DynSvc service)
    {
        if (!_jsonConverter.TryDeserialise(
                await _axCalling.GetHttp($"/api/services/{service.ServiceGroupName}/{service.Name}"),
                out GetSvcResponse? res))
        {
            _logger.LogError("Deserialisation error while getting operations for service {group}/{service}", service.ServiceGroupName, service.Name);
            return [];
        }

        await Task.WhenAll(res.Operations.Select(x => MutateOperationWithParamsAndReturnType(service, x)));
        _logger.LogInformation("Discovered {n} operations for service {group}/{service}", res.Operations.Length, service.ServiceGroupName, service.Name);
        return res.Operations;
    }

    private async Task MutateOperationWithParamsAndReturnType(DynSvc service, DynSvcOp operation)
    {
        if (!_jsonConverter.TryDeserialise(
                await _axCalling.GetHttp($"/api/services/{service.ServiceGroupName}/{service.Name}/{operation.Name}"),
                out GetOperationResponse? opRes))
        {
            _logger.LogError("Deserialisation error while getting parameter and return types for operation {group}/{service}/{operation}",
                service.ServiceGroupName, service.Name, operation.Name);
            return;
        }

        _logger.LogInformation("Discovered {n} parameters and return type for operation {group}/{service}/{operation}",
            opRes.Parameters.Length, service.ServiceGroupName, service.Name, operation.Name);
        operation.ServiceGroupName = service.ServiceGroupName;
        operation.ServiceName = service.Name;
        operation.Parameters = opRes.Parameters;
        operation.Return = opRes.Return;
    }
}

public interface IAxSvcDiscoveryService
{
    Task<IEnumerable<DynSvcGroup>> MapServicesAsync(string grepGroupsRegexString = ".*",
        string grepServicesRegexString = ".*",
        string grepOperationsRegexString = ".*");

    Task<IEnumerable<DynSvcGroup>> GetAllGroups();
    Task<IEnumerable<DynSvc>> GetServicesForGroups(IEnumerable<DynSvcGroup> groups);
    Task<IEnumerable<DynSvcOp>> GetOperationsForServices(IEnumerable<DynSvc> services);
}