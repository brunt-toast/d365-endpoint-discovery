namespace DynamicsEndpointDiscovery.Application.Types;

public class DynSvc
{
    public string Name { get; set; } = string.Empty;
    public DynSvcOp[] Operations { get; set; } = [];
}
