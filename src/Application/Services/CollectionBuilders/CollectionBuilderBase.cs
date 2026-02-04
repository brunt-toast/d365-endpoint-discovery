using Dev.JoshBrunton.DynamicsEndpointDiscovery.Lib.Ax.Types;

namespace Dev.JoshBrunton.DynamicsEndpointDiscovery.Application.Services.CollectionBuilders;

public abstract class CollectionBuilderBase<T> : ICollectionBuilder
{
    public object BuildCollection(IEnumerable<DynSvcGroup> groups, Dictionary<string,string> typeDefs, string resource, string collectionName = "Collection") => 
        BuildTypedCollection(groups, typeDefs, resource, collectionName)!;

    protected abstract T BuildTypedCollection(IEnumerable<DynSvcGroup> groups, Dictionary<string,string> typeDefs, string resource, string collectionName = "Collection");
}