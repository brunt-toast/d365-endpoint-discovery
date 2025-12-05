namespace DynamicsEndpointDiscovery.Application.Types.Dynamics;

public class DynSvcGroup 
{
    public string Name { get; set; } = string.Empty;
    public DynSvc[] Services { get; set; } = [];
}
