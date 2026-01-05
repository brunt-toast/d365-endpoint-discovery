using Dev.JoshBrunton.DynamicsEndpointDiscovery.Application.Services;
using Dev.JoshBrunton.DynamicsEndpointDiscovery.Application.Services.CollectionBuilders;
using Dev.JoshBrunton.DynamicsEndpointDiscovery.Application.Services.Serialisers;
using Microsoft.Extensions.DependencyInjection;
using Dev.JoshBrunton.DynamicsEndpointDiscovery.Lib.Ax.Ioc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;

namespace Dev.JoshBrunton.DynamicsEndpointDiscovery.Application.Ioc;

public static class ApplicationServiceRegistrar
{
    public static void RegisterServices(IServiceCollection sc)
    {
        AxServiceRegistrar.RegisterServices(sc);

        sc.AddSingleton<ILogger>(_ => new NullLoggerFactory().CreateLogger(string.Empty));
        sc.AddSingleton<IMainService, MainService>();
        sc.AddSingleton<SerialiserFactory>();
        sc.AddSingleton<CollectionBuilderFactory>();
    }
}
