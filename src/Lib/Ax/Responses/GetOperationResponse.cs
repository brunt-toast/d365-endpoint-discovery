using Dev.JoshBrunton.DynamicsEndpointDiscovery.Lib.Ax.Types;

namespace Dev.JoshBrunton.DynamicsEndpointDiscovery.Lib.Ax.Responses;

internal class GetOperationResponse
{
    public DynSvcMethodSymbol[] Parameters { get; set; } = [];
    public required DynSvcMethodSymbol Return { get; set; }
}
