using Dev.JoshBrunton.DynamicsEndpointDiscovery.Application.Enums;

namespace Dev.JoshBrunton.DynamicsEndpointDiscovery.Application.Services.CollectionBuilders;

public class CollectionBuilderFactory
{
    public ICollectionBuilder GetCollectionBuilder(OutputSchemas schema)
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
