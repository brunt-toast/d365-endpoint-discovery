using Dev.JoshBrunton.DynamicsEndpointDiscovery.Lib.Ax.Types;

namespace Dev.JoshBrunton.DynamicsEndpointDiscovery.Rcl.Models;

public class SelectableDynSvcGroupModel
{
    public DynSvcGroup Item { get; }
    public SelectableDynSvcModel[] Children { get; }
    public bool IsSelected
    {
        get => Children.Length > 0 ? Children.All(x => x.IsSelected) : field;
        set
        {
            if (Children.Length > 0)
            {
                foreach (var child in Children)
                {
                    child.IsSelected = value;
                }
            }
            else
            {
                field = value;
            }
        }
    }

    public SelectableDynSvcGroupModel(DynSvcGroup item, SelectableDynSvcModel[]? children = null)
    {
        Item = item;
        Children = children ?? [];
    }
}