using Dev.JoshBrunton.DynamicsEndpointDiscovery.Application.Enums;

namespace Dev.JoshBrunton.DynamicsEndpointDiscovery.Application.Services.CollectionBuilders.Options;

public static class CollectionBuilderOptionsFactory
{
    public static ICollectionBuilderOptions Create(OutputSchemas schema)
    {
        return schema switch
        {
            OutputSchemas.Default => new DefaultCollectionBuilderOptions(),
            OutputSchemas.Postman => new PostmanCollectionBuilderOptions(),
            OutputSchemas.OpenApi => new OpenApiCollectionBuilderOptions(),
            OutputSchemas.CSharp => new CSharpCollectionBuilderOptions(),
            _ => throw new ArgumentOutOfRangeException(nameof(schema), schema, null)
        };
    }
}
