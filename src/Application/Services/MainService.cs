using Dev.JoshBrunton.DynamicsEndpointDiscovery.Application.Requests;
using Dev.JoshBrunton.DynamicsEndpointDiscovery.Application.Services.CollectionBuilders;
using Dev.JoshBrunton.DynamicsEndpointDiscovery.Application.Services.Serialisers;
using Dev.JoshBrunton.DynamicsEndpointDiscovery.Lib.Ax.Config;
using Dev.JoshBrunton.DynamicsEndpointDiscovery.Lib.Ax.Services;
using Dev.JoshBrunton.DynamicsEndpointDiscovery.Lib.Ax.Types;

namespace Dev.JoshBrunton.DynamicsEndpointDiscovery.Application.Services;

internal class MainService : IMainService
{
    private readonly IAxConfig _config;
    private readonly IAxSvcDiscoveryService _discoveryService;
    private readonly CollectionBuilderFactory _collectionBuilderFactory;
    private readonly SerialiserFactory _serialiserFactory;

    public MainService(IAxConfig config, IAxSvcDiscoveryService discoveryService, CollectionBuilderFactory collectionBuilderFactory, SerialiserFactory serialiserFactory)
    {
        _config = config;
        _discoveryService = discoveryService;
        _collectionBuilderFactory = collectionBuilderFactory;
        _serialiserFactory = serialiserFactory;
    }

    public async Task<string> GetServiceCollectionAsync(GetServiceCollectionRequest request)
    {
        UpdateConfig(request);

        var collectionBuilder = _collectionBuilderFactory.GetCollectionBuilder(request.OutputSchema);
        var serialiser = _serialiserFactory.GetSerialiser(request.OutputFormat);

        var services = await _discoveryService.MapServicesAsync(request.GrepGroupsRegex, request.GrepServicesRegex, request.GrepOperationsRegex);
        var collection = collectionBuilder.BuildCollection(services, request.Resource, request.CollectionName);
        var serialisation = serialiser.Serialise(collection, request.Minify);

        return serialisation;
    }

    public async Task<IEnumerable<DynSvcGroup>> GetAllGroups(GetAllGroupsRequest request)
    {
        UpdateConfig(request);
        var ret = await _discoveryService.GetAllGroups();
        return ret;
    }

    public async Task<IEnumerable<DynSvc>> GetServicesForGroups(GetServicesForGroupsRequest request)
    {
        UpdateConfig(request);
        var ret = await _discoveryService.GetServicesForGroups(request.Groups);
        return ret;
    }

    public async Task<IEnumerable<DynSvcOp>> GetOperationsForServices(GetOperationsForServicesRequest request)
    {
        UpdateConfig(request);
        var ret = await _discoveryService.GetOperationsForServices(request.Services);
        return ret;
    }

    private void UpdateConfig(IHasAxCredentials credentials)
    {
        _config.ClientId = credentials.ClientId;
        _config.ClientSecret = credentials.ClientSecret;
        _config.Resource = credentials.Resource;
        _config.TokenRequestEndpoint = credentials.TokenRequestEndpoint;
    }

    public string BuildCustomCollection(BuildCustomCollectionRequest request)
    {
        var collectionBuilder = _collectionBuilderFactory.GetCollectionBuilder(request.OutputSchema);
        var serialiser = _serialiserFactory.GetSerialiser(request.OutputFormat);
        var collection = collectionBuilder.BuildCollection(request.Services, request.Resource, request.CollectionName);
        var serialisation = serialiser.Serialise(collection, request.Minify);
        return serialisation;
    }
}

public interface IMainService
{
    Task<string> GetServiceCollectionAsync(GetServiceCollectionRequest request);
    Task<IEnumerable<DynSvcGroup>> GetAllGroups(GetAllGroupsRequest request);
    Task<IEnumerable<DynSvc>> GetServicesForGroups(GetServicesForGroupsRequest request);
    Task<IEnumerable<DynSvcOp>> GetOperationsForServices(GetOperationsForServicesRequest request);
    string BuildCustomCollection(BuildCustomCollectionRequest request);
}