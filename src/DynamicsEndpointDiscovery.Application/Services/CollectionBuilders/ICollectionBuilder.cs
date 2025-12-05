using DynamicsEndpointDiscovery.Application.Types.Dynamics;

namespace DynamicsEndpointDiscovery.Application.Services.CollectionBuilders;

public interface ICollectionBuilder
{
    object BuildCollection(IEnumerable<DynSvcGroup> groups, string resource, string collectionName = "Collection");
}