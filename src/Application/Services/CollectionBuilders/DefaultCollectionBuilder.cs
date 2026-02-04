using Dev.JoshBrunton.DynamicsEndpointDiscovery.Lib.Ax.Types;

namespace Dev.JoshBrunton.DynamicsEndpointDiscovery.Application.Services.CollectionBuilders;

internal class DefaultCollectionBuilder : CollectionBuilderBase<IEnumerable<DynSvcGroup>>
{
    protected override IEnumerable<DynSvcGroup> BuildTypedCollection(IEnumerable<DynSvcGroup> groups, 
        Dictionary<string, string> typeDefs, 
        string resource,
        string collectionName = "Collection")
    {
        return groups;
    }
}
