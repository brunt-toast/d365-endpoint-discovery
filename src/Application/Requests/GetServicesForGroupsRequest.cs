using Dev.JoshBrunton.DynamicsEndpointDiscovery.Lib.Ax.Types;

namespace Dev.JoshBrunton.DynamicsEndpointDiscovery.Application.Requests;

public class GetServicesForGroupsRequest 
{
    public required DynSvcGroup[] Groups { get; init; }
}