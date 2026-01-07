using Dev.JoshBrunton.DynamicsEndpointDiscovery.Lib.Ax.Types;

namespace Dev.JoshBrunton.DynamicsEndpointDiscovery.Application.Services.CollectionBuilders;

public interface ICollectionBuilder
{
    object BuildCollection(IEnumerable<DynSvcGroup> groups, string resource, string collectionName = "Collection");
}