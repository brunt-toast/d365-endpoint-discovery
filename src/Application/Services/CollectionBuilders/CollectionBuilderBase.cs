using Dev.JoshBrunton.DynamicsEndpointDiscovery.Application.Services.CollectionBuilders.Options;
using Dev.JoshBrunton.DynamicsEndpointDiscovery.Lib.Ax.Types;
using Dev.JoshBrunton.DynamicsEndpointDiscovery.Lib.Ax.Types.Soap;

namespace Dev.JoshBrunton.DynamicsEndpointDiscovery.Application.Services.CollectionBuilders;

public abstract class CollectionBuilderBase<TOptions> : ICollectionBuilder<TOptions>
    where TOptions : ICollectionBuilderOptions
{
    public abstract string BuildCollection(
        IEnumerable<DynSvcGroup> groups,
        SoapTypeCollection types,
        string resource,
        string collectionName,
        TOptions options);
}
