using DynamicsEndpointDiscovery.Application.Types.Dynamics;

namespace DynamicsEndpointDiscovery.Application.Responses.Dynamics;

internal class DynGetOperationResponse
{
    public DynSvcMethodSymbol[] Parameters { get; set; } = [];
    public required DynSvcMethodSymbol Return { get; set; }
}
