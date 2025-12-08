using System;
using System.Collections.Generic;
using System.Text;
using DynamicsEndpointDiscovery.Application.Enums;

namespace DynamicsEndpointDiscovery.Application.Services.CollectionBuilders;

public static class CollectionBuilderFactory
{
    public static ICollectionBuilder GetCollectionBuilder(OutputSchemas schema)
    {
        return schema switch
        {
            OutputSchemas.Default => new DefaultCollectionBuilder(),
            OutputSchemas.Postman => new PostmanCollectionBuilder(),
            OutputSchemas.OpenApi => new OpenApiCollectionBuilder(),
            _ => throw new ArgumentOutOfRangeException(nameof(schema), schema, null)
        };
    }
}
