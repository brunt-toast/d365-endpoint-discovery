using Dev.JoshBrunton.DynamicsEndpointDiscovery.Application.Logging.Sinks;
using Dev.JoshBrunton.DynamicsEndpointDiscovery.Application.Services;
using Dev.JoshBrunton.DynamicsEndpointDiscovery.Application.Services.CollectionBuilders;
using Dev.JoshBrunton.DynamicsEndpointDiscovery.Application.Services.Serialisers;
using Dev.JoshBrunton.DynamicsEndpointDiscovery.Core.Consts;
using Microsoft.Extensions.DependencyInjection;
using Dev.JoshBrunton.DynamicsEndpointDiscovery.Lib.Ax.Ioc;
using Serilog;
using Serilog.Core;

namespace Dev.JoshBrunton.DynamicsEndpointDiscovery.Application.Ioc;

public static class ApplicationServiceRegistrar
{
    public static void RegisterServices(IServiceCollection sc)
    {
        AxServiceRegistrar.RegisterServices(sc);

        sc.AddTransient<IHttpClientFactory, LegacyHttpClientFactory>();

        sc.AddTransient<DefaultCollectionBuilder>();
        sc.AddTransient<PostmanCollectionBuilder>();
        sc.AddTransient<OpenApiCollectionBuilder>();

        sc.AddSingleton<IMainService, MainService>();
        sc.AddSingleton<SerialiserFactory>();
        sc.AddSingleton<CollectionBuilderFactory>();

        sc.AddSingleton<ILogEventSink, DebuggerSink>();

        AppDomain.CurrentDomain.ProcessExit += (_, _) => Log.CloseAndFlush();
    }
}
