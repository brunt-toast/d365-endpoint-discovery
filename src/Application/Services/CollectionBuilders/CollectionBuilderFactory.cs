using Dev.JoshBrunton.DynamicsEndpointDiscovery.Application.Enums;
using Microsoft.Extensions.DependencyInjection;

namespace Dev.JoshBrunton.DynamicsEndpointDiscovery.Application.Services.CollectionBuilders;

public class CollectionBuilderFactory
{
    private readonly IServiceProvider _services;

    public CollectionBuilderFactory(IServiceProvider services)
    {
        _services = services;
    }

    public ICollectionBuilder GetCollectionBuilder(OutputSchemas schema)
    {
        return schema switch
        {
            OutputSchemas.Default => _services.GetRequiredService<DefaultCollectionBuilder>(),
            OutputSchemas.Postman => _services.GetRequiredService<PostmanCollectionBuilder>(),
            OutputSchemas.OpenApi => _services.GetRequiredService<OpenApiCollectionBuilder>(),
            OutputSchemas.CSharp => _services.GetRequiredService<CSharpCollectionBuilder>(),
            _ => throw new ArgumentOutOfRangeException(nameof(schema), schema, null)
        };
    }
}
