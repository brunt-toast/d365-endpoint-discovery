using Dev.JoshBrunton.DynamicsEndpointDiscovery.Lib.Ax.Types;

namespace Dev.JoshBrunton.DynamicsEndpointDiscovery.Application.Services.CollectionBuilders;

public abstract class CollectionBuilderBase<T> : ICollectionBuilder
{
    public object BuildCollection(IEnumerable<DynSvcGroup> groups, string resource, string collectionName = "Collection") => 
        BuildTypedCollection(groups, resource, collectionName)!;

    protected abstract T BuildTypedCollection(IEnumerable<DynSvcGroup> groups, string resource, string collectionName = "Collection");
}