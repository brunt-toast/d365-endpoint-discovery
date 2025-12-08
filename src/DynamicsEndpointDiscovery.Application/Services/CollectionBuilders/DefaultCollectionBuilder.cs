using DynamicsEndpointDiscovery.Application.Types.Dynamics;

namespace DynamicsEndpointDiscovery.Application.Services.CollectionBuilders;

internal class DefaultCollectionBuilder : CollectionBuilderBase<IEnumerable<DynSvcGroup>>
{
    protected override IEnumerable<DynSvcGroup> BuildTypedCollection(IEnumerable<DynSvcGroup> groups, string resource, string collectionName = "Collection")
    {
        return groups;
    }
}
