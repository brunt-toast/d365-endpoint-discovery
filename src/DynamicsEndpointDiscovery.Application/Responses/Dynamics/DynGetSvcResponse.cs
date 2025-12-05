using DynamicsEndpointDiscovery.Application.Types.Dynamics;

namespace DynamicsEndpointDiscovery.Application.Responses.Dynamics;

internal class DynGetSvcResponse
{
    public DynSvcOp[] Operations { get; set; } = [];
}
