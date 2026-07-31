using Dev.JoshBrunton.DynamicsEndpointDiscovery.Application.Enums;
using Dev.JoshBrunton.DynamicsEndpointDiscovery.Application.Services.CollectionBuilders.Options;
using Dev.JoshBrunton.DynamicsEndpointDiscovery.Lib.Ax.Types;
using Dev.JoshBrunton.DynamicsEndpointDiscovery.Lib.Ax.Types.Soap;
using Microsoft.Extensions.DependencyInjection;

namespace Dev.JoshBrunton.DynamicsEndpointDiscovery.Application.Services.CollectionBuilders;

public class CollectionBuilderFactory
{
    private readonly IServiceProvider _services;

    public CollectionBuilderFactory(IServiceProvider services)
    {
        _services = services;
    }

    public string BuildCollection(
        OutputSchemas schema,
        IEnumerable<DynSvcGroup> groups,
        SoapTypeCollection types,
        string resource,
        string collectionName,
        ICollectionBuilderOptions options)
    {
        return schema switch
        {
            OutputSchemas.Default => BuildCollection<DefaultCollectionBuilderOptions, DefaultCollectionBuilder>(
                groups,
                types,
                resource,
                collectionName,
                options),
            OutputSchemas.Postman => BuildCollection<PostmanCollectionBuilderOptions, PostmanCollectionBuilder>(
                groups,
                types,
                resource,
                collectionName,
                options),
            OutputSchemas.OpenApi => BuildCollection<OpenApiCollectionBuilderOptions, OpenApiCollectionBuilder>(
                groups,
                types,
                resource,
                collectionName,
                options),
            OutputSchemas.CSharp => BuildCollection<CSharpCollectionBuilderOptions, CSharpCollectionBuilder>(
                groups,
                types,
                resource,
                collectionName,
                options),
            _ => throw new ArgumentOutOfRangeException(nameof(schema), schema, null)
        };
    }

    private string BuildCollection<TOptions, TBuilder>(
        IEnumerable<DynSvcGroup> groups,
        SoapTypeCollection types,
        string resource,
        string collectionName,
        ICollectionBuilderOptions options)
        where TOptions : ICollectionBuilderOptions
        where TBuilder : ICollectionBuilder<TOptions>
    {
        if (options is not TOptions typedOptions)
        {
            throw new ArgumentException($"Expected options of type {typeof(TOptions).Name}.", nameof(options));
        }

        var builder = _services.GetRequiredService<TBuilder>();
        return builder.BuildCollection(groups, types, resource, collectionName, typedOptions);
    }
}
