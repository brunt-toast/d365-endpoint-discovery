using Dev.JoshBrunton.DynamicsEndpointDiscovery.Lib.Ax.Types;

namespace Dev.JoshBrunton.DynamicsEndpointDiscovery.Lib.Ax.Responses;

internal class GetSvcResponse
{
    public DynSvcOp[] Operations { get; set; } = [];
}
