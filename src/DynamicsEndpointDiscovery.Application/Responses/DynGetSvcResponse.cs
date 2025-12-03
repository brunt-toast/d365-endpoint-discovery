using DynamicsEndpointDiscovery.Application.Types;

namespace DynamicsEndpointDiscovery.Application.Responses;

internal class DynGetSvcResponse
{
    public DynSvcOp[] Operations { get; set; } = [];
}
