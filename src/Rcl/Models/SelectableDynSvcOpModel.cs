using Dev.JoshBrunton.DynamicsEndpointDiscovery.Lib.Ax.Types;

namespace Dev.JoshBrunton.DynamicsEndpointDiscovery.Rcl.Models;

public class SelectableDynSvcOpModel
{
    public DynSvcOp Item { get; }
    public bool IsSelected { get; set; }
    public string FullName => $"{Item.ServiceGroupName}/{Item.ServiceName}/{Item.Name}";

    public SelectableDynSvcOpModel(DynSvcOp item)
    {
        Item = item;
    }
}