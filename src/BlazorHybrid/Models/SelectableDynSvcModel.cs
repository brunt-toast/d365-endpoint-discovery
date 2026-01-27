using Dev.JoshBrunton.DynamicsEndpointDiscovery.Lib.Ax.Types;

namespace Dev.JoshBrunton.DynamicsEndpointDiscovery.BlazorHybrid.Models;

public class SelectableDynSvcModel
{
    public DynSvc Item { get; }
    public SelectableDynSvcOpModel[] Children { get; }
    public string FullName => $"{Item.ServiceGroupName}/{Item.Name}";

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

    public SelectableDynSvcModel(DynSvc item, SelectableDynSvcOpModel[]? children = null)
    {
        Item = item;
        Children = children ?? [];
    }
}