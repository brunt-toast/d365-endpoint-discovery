using Microsoft.Extensions.DependencyInjection;
using Serilog;
using Serilog.Core;

namespace Dev.JoshBrunton.DynamicsEndpointDiscovery.Core.Ioc;

public static class CoreServiceRegistrar
{
    public static void RegisterServices(IServiceCollection sc)
    {
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
    }
}
