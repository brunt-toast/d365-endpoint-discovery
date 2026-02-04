using Newtonsoft.Json;

namespace Dev.JoshBrunton.DynamicsEndpointDiscovery.Lib.Ax.Types;

public class DynSvcOp
{
    [JsonIgnore] public string ServiceGroupName { get; set; } = string.Empty;
    [JsonIgnore] public string ServiceName { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public DynSvcMethodSymbol[] Parameters { get; set; } = [];
    public required DynSvcMethodSymbol? Return { get; set; } 
}