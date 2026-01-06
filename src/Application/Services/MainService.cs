using Dev.JoshBrunton.DynamicsEndpointDiscovery.Application.Requests;
using Dev.JoshBrunton.DynamicsEndpointDiscovery.Application.Services.CollectionBuilders;
using Dev.JoshBrunton.DynamicsEndpointDiscovery.Application.Services.Serialisers;
using Dev.JoshBrunton.DynamicsEndpointDiscovery.Lib.Ax.Config;
using Dev.JoshBrunton.DynamicsEndpointDiscovery.Lib.Ax.Services;

namespace Dev.JoshBrunton.DynamicsEndpointDiscovery.Application.Services;

internal class MainService : IMainService
{
    private readonly IAxConfig _config;
    private readonly IAxScvDiscoveryService _discoveryService;
    private readonly CollectionBuilderFactory _collectionBuilderFactory;
    private readonly SerialiserFactory _serialiserFactory;

    public MainService(IAxConfig config, IAxScvDiscoveryService discoveryService, CollectionBuilderFactory collectionBuilderFactory, SerialiserFactory serialiserFactory)
    {
        _config = config;
        _discoveryService = discoveryService;
        _collectionBuilderFactory = collectionBuilderFactory;
        _serialiserFactory = serialiserFactory;
    }

    public async Task<string> GetServiceCollectionAsync(GetServiceCollectionRequest request)
    {
        _config.ClientId = request.ClientId;
        _config.ClientSecret = request.ClientSecret;
        _config.Resource = request.Resource;
        _config.TokenRequestEndpoint = request.TokenRequestEndpoint;

        var collectionBuilder = _collectionBuilderFactory.GetCollectionBuilder(request.OutputSchema);
        var serialiser = _serialiserFactory.GetSerialiser(request.OutputFormat);

        var services = await _discoveryService.MapServicesAsync(request.GrepGroupsRegex, request.GrepServicesRegex, request.GrepOperationsRegex);
        var collection = collectionBuilder.BuildCollection(services, request.Resource, request.CollectionName);
        var serialisation = serialiser.Serialise(collection, request.Minify);

        return serialisation;
    }
}

public interface IMainService
{
    Task<string> GetServiceCollectionAsync(GetServiceCollectionRequest request);
}