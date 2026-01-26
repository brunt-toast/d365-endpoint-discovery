using Dev.JoshBrunton.DynamicsEndpointDiscovery.Lib.Ax.Types;

namespace Dev.JoshBrunton.DynamicsEndpointDiscovery.Application.Requests;

public class GetOperationsForServicesRequest
{
    public required DynSvc[] Services { get; init; }
}