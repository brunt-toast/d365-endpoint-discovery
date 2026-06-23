using Dev.JoshBrunton.DynamicsEndpointDiscovery.Application.Requests;
using Dev.JoshBrunton.DynamicsEndpointDiscovery.Application.Services.CollectionBuilders;
using Dev.JoshBrunton.DynamicsEndpointDiscovery.Application.Services.Serialisers;
using Dev.JoshBrunton.DynamicsEndpointDiscovery.Lib.Ax.Config;
using Dev.JoshBrunton.DynamicsEndpointDiscovery.Lib.Ax.Services.Soap;
using Dev.JoshBrunton.DynamicsEndpointDiscovery.Lib.Ax.Services.SvcDiscovery;
using Dev.JoshBrunton.DynamicsEndpointDiscovery.Lib.Ax.Types;

namespace Dev.JoshBrunton.DynamicsEndpointDiscovery.Application.Services;

internal class MainService : IMainService
{
    private readonly IAxConfig _config;
    private readonly IAxSvcDiscoveryService _discoveryService;
    private readonly CollectionBuilderFactory _collectionBuilderFactory;
    private readonly SerialiserFactory _serialiserFactory;
    private readonly IAxSoapService _soapService;

    public MainService(
        IAxConfig config,
        IAxSvcDiscoveryService discoveryService,
        CollectionBuilderFactory collectionBuilderFactory,
        SerialiserFactory serialiserFactory,
        IAxSoapService soapService)
    {
        _config = config;
        _discoveryService = discoveryService;
        _collectionBuilderFactory = collectionBuilderFactory;
        _serialiserFactory = serialiserFactory;
        _soapService = soapService;
    }

    public async Task<string> GetServiceCollectionAsync(GetServiceCollectionRequest request)
    {
        var collectionBuilder = _collectionBuilderFactory.GetCollectionBuilder(request.OutputSchema);
        var serialiser = _serialiserFactory.GetSerialiser(request.OutputFormat);

        var services = (await _discoveryService.MapServicesAsync(
                request.GrepGroupsRegex,
                request.GrepServicesRegex,
                request.GrepOperationsRegex))
            .ToList();
        var types = await _soapService.GetDataContractsForServices(services.Select(x => x.Name));
        var collection = collectionBuilder.BuildCollection(services, types, _config.Resource, request.CollectionName);
        var serialisation = collection is string generatedCode
            ? generatedCode
            : serialiser.Serialise(collection, request.Minify);

        return serialisation;
    }

    public async Task<IEnumerable<DynSvcGroup>> GetAllGroups()
    {
        var ret = await _discoveryService.GetAllGroups();
        return ret;
    }

    public async Task<IEnumerable<DynSvc>> GetServicesForGroups(GetServicesForGroupsRequest request)
    {
        var ret = await _discoveryService.GetServicesForGroups(request.Groups);
        return ret;
    }

    public async Task<IEnumerable<DynSvcOp>> GetOperationsForServices(GetOperationsForServicesRequest request)
    {
        var ret = await _discoveryService.GetOperationsForServices(request.Services);
        return ret;
    }

    public async Task<string> BuildCustomCollection(BuildCustomCollectionRequest request)
    {
        var collectionBuilder = _collectionBuilderFactory.GetCollectionBuilder(request.OutputSchema);
        var serialiser = _serialiserFactory.GetSerialiser(request.OutputFormat);
        var types = await _soapService.GetDataContractsForServices(request.Services.Select(x => x.Name));
        var collection = collectionBuilder.BuildCollection(request.Services, types, request.Resource, request.CollectionName);
        var serialisation = collection is string generatedCode
            ? generatedCode
            : serialiser.Serialise(collection, request.Minify);
        return serialisation;
    }
}

public interface IMainService
{
    Task<string> GetServiceCollectionAsync(GetServiceCollectionRequest request);
    Task<IEnumerable<DynSvcGroup>> GetAllGroups();
    Task<IEnumerable<DynSvc>> GetServicesForGroups(GetServicesForGroupsRequest request);
    Task<IEnumerable<DynSvcOp>> GetOperationsForServices(GetOperationsForServicesRequest request);
    Task<string> BuildCustomCollection(BuildCustomCollectionRequest request);
}
