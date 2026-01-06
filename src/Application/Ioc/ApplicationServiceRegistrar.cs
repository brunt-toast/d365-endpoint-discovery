using System.Diagnostics;
using Dev.JoshBrunton.DynamicsEndpointDiscovery.Application.Logging.Sinks;
using Dev.JoshBrunton.DynamicsEndpointDiscovery.Application.Services;
using Dev.JoshBrunton.DynamicsEndpointDiscovery.Application.Services.CollectionBuilders;
using Dev.JoshBrunton.DynamicsEndpointDiscovery.Application.Services.Serialisers;
using Microsoft.Extensions.DependencyInjection;
using Dev.JoshBrunton.DynamicsEndpointDiscovery.Lib.Ax.Ioc;
using Serilog;
using Serilog.Core;
using YamlDotNet.Serialization.TypeInspectors;

namespace Dev.JoshBrunton.DynamicsEndpointDiscovery.Application.Ioc;

public static class ApplicationServiceRegistrar
{
    public static void RegisterServices(IServiceCollection sc)
    {
        AxServiceRegistrar.RegisterServices(sc);

        sc.AddSingleton<IMainService, MainService>();
        sc.AddSingleton<SerialiserFactory>();
        sc.AddSingleton<CollectionBuilderFactory>();

        sc.AddSingleton<ILogEventSink, DebuggerSink>();
        sc.AddSingleton<LoggerConfiguration>(sp =>
        {
            var ret = new LoggerConfiguration();
            foreach (var sink in sp.GetServices<ILogEventSink>())
            {
                ret.WriteTo.Sink(sink);
            }

            return ret;
        });

        sc.AddSingleton<ILogger>(x => x.GetRequiredService<LoggerConfiguration>().CreateLogger());
        AppDomain.CurrentDomain.ProcessExit += (_, _) => Log.CloseAndFlush();
    }
}
