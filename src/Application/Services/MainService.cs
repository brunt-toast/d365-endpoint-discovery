using Dev.JoshBrunton.DynamicsEndpointDiscovery.Application.Requests;
using Dev.JoshBrunton.DynamicsEndpointDiscovery.Application.Services.CollectionBuilders;
using Dev.JoshBrunton.DynamicsEndpointDiscovery.Application.Services.CollectionBuilders.Options;
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
    private readonly IAxSoapService _soapService;

    public MainService(
        IAxConfig config,
        IAxSvcDiscoveryService discoveryService,
        CollectionBuilderFactory collectionBuilderFactory,
        IAxSoapService soapService)
    {
        _config = config;
        _discoveryService = discoveryService;
        _collectionBuilderFactory = collectionBuilderFactory;
        _soapService = soapService;
    }

    public async Task<string> GetServiceCollectionAsync(GetServiceCollectionRequest request)
    {
        var options = CollectionBuilderOptionsFactory.Create(request.OutputSchema);
        if (options is SerialisedCollectionBuilderOptions serialisedOptions)
        {
            serialisedOptions.OutputFormat = request.OutputFormat;
            serialisedOptions.Minify = request.Minify;
        }

        options.Validate();

        var services = (await _discoveryService.MapServicesAsync(
                request.GrepGroupsRegex,
                request.GrepServicesRegex,
                request.GrepOperationsRegex))
            .ToList();
        var types = await _soapService.GetDataContractsForServices(services.Select(x => x.Name));
        var serialisation = _collectionBuilderFactory.BuildCollection(
            request.OutputSchema,
            services,
            types,
            _config.Resource,
            request.CollectionName,
            options);

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
        request.Options.Validate();

        var types = await _soapService.GetDataContractsForServices(request.Services.Select(x => x.Name));
        var serialisation = _collectionBuilderFactory.BuildCollection(
            request.OutputSchema,
            request.Services,
            types,
            request.Resource,
            request.CollectionName,
            request.Options);
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
