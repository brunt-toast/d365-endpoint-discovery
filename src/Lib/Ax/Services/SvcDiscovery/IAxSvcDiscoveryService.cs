using Dev.JoshBrunton.DynamicsEndpointDiscovery.Lib.Ax.Types;

namespace Dev.JoshBrunton.DynamicsEndpointDiscovery.Lib.Ax.Services.SvcDiscovery;

public interface IAxSvcDiscoveryService
{
    Task<IEnumerable<DynSvcGroup>> MapServicesAsync(string grepGroupsRegexString = ".*",
        string grepServicesRegexString = ".*",
        string grepOperationsRegexString = ".*");

    Task<IEnumerable<DynSvcGroup>> GetAllGroups();
    Task<IEnumerable<DynSvc>> GetServicesForGroups(IEnumerable<DynSvcGroup> groups);
    Task<IEnumerable<DynSvcOp>> GetOperationsForServices(IEnumerable<DynSvc> services);
}