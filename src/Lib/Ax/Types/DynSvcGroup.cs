namespace Dev.JoshBrunton.DynamicsEndpointDiscovery.Lib.Ax.Types;

public class DynSvcGroup 
{
    public string Name { get; set; } = string.Empty;
    public DynSvc[] Services { get; set; } = [];
}
