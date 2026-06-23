using Dev.JoshBrunton.DynamicsEndpointDiscovery.Lib.Ax.Types;
using Dev.JoshBrunton.DynamicsEndpointDiscovery.Lib.Ax.Types.Soap;

namespace Dev.JoshBrunton.DynamicsEndpointDiscovery.Application.Services.CollectionBuilders;

internal class DefaultCollectionBuilder : CollectionBuilderBase<IEnumerable<DynSvcGroup>>
{
    protected override IEnumerable<DynSvcGroup> BuildTypedCollection(
        IEnumerable<DynSvcGroup> groups,
        SoapTypeCollection types,
        string resource,
        string collectionName = "Collection")
    {
        return groups;
    }
}
