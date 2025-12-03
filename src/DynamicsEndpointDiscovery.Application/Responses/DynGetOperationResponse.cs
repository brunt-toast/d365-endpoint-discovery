using DynamicsEndpointDiscovery.Application.Types;

namespace DynamicsEndpointDiscovery.Application.Responses;

internal class DynGetOperationResponse
{
    public DynSvcMethodSymbol[] Parameters { get; set; } = [];
    public required DynSvcMethodSymbol Return { get; set; }
}
