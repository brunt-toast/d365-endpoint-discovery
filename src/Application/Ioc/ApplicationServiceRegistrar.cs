using Dev.JoshBrunton.DynamicsEndpointDiscovery.Application.Config;
using Dev.JoshBrunton.DynamicsEndpointDiscovery.Application.Logging.Sinks;
using Dev.JoshBrunton.DynamicsEndpointDiscovery.Application.Services;
using Dev.JoshBrunton.DynamicsEndpointDiscovery.Application.Services.CollectionBuilders;
using Dev.JoshBrunton.DynamicsEndpointDiscovery.Application.Services.Serialisers;
using Dev.JoshBrunton.DynamicsEndpointDiscovery.Core.Consts;
using Dev.JoshBrunton.DynamicsEndpointDiscovery.Lib.Ax.Ioc;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using Serilog.Core;

namespace Dev.JoshBrunton.DynamicsEndpointDiscovery.Application.Ioc;

public static class ApplicationServiceRegistrar
{
    public static void RegisterServices(IServiceCollection sc, Func<IServiceProvider, bool> mockPredicate)
    {
        AxServiceRegistrar.RegisterServices(sc, mockPredicate);

        sc.AddSingleton<HttpClientOptions>();
        sc.AddSingleton<IHttpClientFactory, ConfigurableHttpClientFactory>();

        sc.AddTransient<DefaultCollectionBuilder>();
        sc.AddTransient<PostmanCollectionBuilder>();
        sc.AddTransient<OpenApiCollectionBuilder>();
        sc.AddTransient<CSharpCollectionBuilder>();

        sc.AddSingleton<IMainService, MainService>();
        sc.AddSingleton<SerialiserFactory>();
        sc.AddSingleton<CollectionBuilderFactory>();

        sc.AddSingleton<ILogEventSink, DebuggerSink>();

        AppDomain.CurrentDomain.ProcessExit += (_, _) => Log.CloseAndFlush();
    }
}
