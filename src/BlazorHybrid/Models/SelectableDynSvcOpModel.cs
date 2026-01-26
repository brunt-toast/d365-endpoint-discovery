using Dev.JoshBrunton.DynamicsEndpointDiscovery.Lib.Ax.Types;

namespace Dev.JoshBrunton.DynamicsEndpointDiscovery.BlazorHybrid.Models;

public class SelectableDynSvcOpModel
{
    public DynSvcOp Item { get; }
    public bool IsSelected { get; set; }

    public SelectableDynSvcOpModel(DynSvcOp item)
    {
        Item = item;
    }
}