namespace DynamicsEndpointDiscovery.Application.Types;

public class DynSvcGroup 
{
    public string Name { get; set; } = string.Empty;
    public DynSvc[] Services { get; set; } = [];
}
