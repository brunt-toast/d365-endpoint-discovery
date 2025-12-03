namespace DynamicsEndpointDiscovery.Application.Types;

public class DynSvcOp 
{
    public string Name { get; set; } = string.Empty;
    public DynSvcMethodSymbol[] Parameters { get; set; } = [];
    public required DynSvcMethodSymbol Return { get; set; } 
}