using Dev.JoshBrunton.DynamicsEndpointDiscovery.Lib.Ax.Types;

namespace Dev.JoshBrunton.DynamicsEndpointDiscovery.Application.Services.CollectionBuilders;

public interface ICollectionBuilder
{
    object BuildCollection(IEnumerable<DynSvcGroup> groups, Dictionary<string,string> typeDefs, string resource, string collectionName = "Collection");
}