using Dev.JoshBrunton.DynamicsEndpointDiscovery.Application.Services.CollectionBuilders.Options;
using Dev.JoshBrunton.DynamicsEndpointDiscovery.Application.Services.Serialisers;
using Dev.JoshBrunton.DynamicsEndpointDiscovery.Lib.Ax.Types;
using Dev.JoshBrunton.DynamicsEndpointDiscovery.Lib.Ax.Types.Soap;

namespace Dev.JoshBrunton.DynamicsEndpointDiscovery.Application.Services.CollectionBuilders;

internal class DefaultCollectionBuilder : SerialisedCollectionBuilderBase<DefaultCollectionBuilderOptions>
{
    public DefaultCollectionBuilder(SerialiserFactory serialiserFactory) : base(serialiserFactory)
    {
    }

    protected override object BuildSerializableCollection(
        IEnumerable<DynSvcGroup> groups,
        SoapTypeCollection types,
        string resource,
        string collectionName,
        DefaultCollectionBuilderOptions options)
    {
        return groups;
    }
}
