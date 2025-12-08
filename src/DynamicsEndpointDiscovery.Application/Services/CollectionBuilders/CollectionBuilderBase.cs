using DynamicsEndpointDiscovery.Application.Types.Dynamics;
using DynamicsEndpointDiscovery.Application.Types.OpenApi;
using System;
using System.Collections.Generic;
using System.Text;

namespace DynamicsEndpointDiscovery.Application.Services.CollectionBuilders;

public abstract class CollectionBuilderBase<T> : ICollectionBuilder
{
    public object BuildCollection(IEnumerable<DynSvcGroup> groups, string resource, string collectionName = "Collection") => 
        BuildTypedCollection(groups, resource, collectionName)!;

    protected abstract T BuildTypedCollection(IEnumerable<DynSvcGroup> groups, string resource, string collectionName = "Collection");
}