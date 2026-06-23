using Dev.JoshBrunton.DynamicsEndpointDiscovery.Lib.Ax.Types;
using Dev.JoshBrunton.DynamicsEndpointDiscovery.Lib.Ax.Types.Soap;

namespace Dev.JoshBrunton.DynamicsEndpointDiscovery.Application.Services.CollectionBuilders;

public interface ICollectionBuilder
{
    object BuildCollection(IEnumerable<DynSvcGroup> groups, SoapTypeCollection types, string resource, string collectionName = "Collection");
}
