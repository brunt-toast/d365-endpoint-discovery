using Dev.JoshBrunton.DynamicsEndpointDiscovery.Lib.Ax.Types;
using Dev.JoshBrunton.DynamicsEndpointDiscovery.Lib.Ax.Types.Soap;

namespace Dev.JoshBrunton.DynamicsEndpointDiscovery.Application.Services.CollectionBuilders;

public abstract class CollectionBuilderBase<T> : ICollectionBuilder
{
    public object BuildCollection(IEnumerable<DynSvcGroup> groups, SoapTypeCollection types, string resource, string collectionName = "Collection") =>
        BuildTypedCollection(groups, types, resource, collectionName)!;

    protected abstract T BuildTypedCollection(IEnumerable<DynSvcGroup> groups, SoapTypeCollection types, string resource, string collectionName = "Collection");
}
