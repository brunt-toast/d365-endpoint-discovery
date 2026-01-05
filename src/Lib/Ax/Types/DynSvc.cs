using Newtonsoft.Json;

namespace Dev.JoshBrunton.DynamicsEndpointDiscovery.Lib.Ax.Types;

public class DynSvc
{
    [JsonIgnore] public string ServiceGroupName { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public DynSvcOp[] Operations { get; set; } = [];
}
