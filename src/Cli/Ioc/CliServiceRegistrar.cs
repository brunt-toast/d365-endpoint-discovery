using Dev.JoshBrunton.DynamicsEndpointDiscovery.Application.Ioc;
using Dev.JoshBrunton.DynamicsEndpointDiscovery.Cli.Commands;
using Microsoft.Extensions.DependencyInjection;
using Dev.JoshBrunton.DynamicsEndpointDiscovery.Cli.Logging.Sinks;
using Serilog.Core;

namespace Dev.JoshBrunton.DynamicsEndpointDiscovery.Cli.Ioc;

public static class CliServiceRegistrar
{
    public static void RegisterServices(IServiceCollection sc)
    {
        ApplicationServiceRegistrar.RegisterServices(sc);
        sc.AddTransient<ServiceDiscoveryCommand>();
        sc.AddTransient<DynSvcDiscoveryRootCommand>();

        sc.AddSingleton<CommandParseResultSink>();
        sc.AddSingleton<ILogEventSink>(x => x.GetRequiredService<CommandParseResultSink>());
        sc.AddSingleton<ICommandParseResultSink>(x => x.GetRequiredService<CommandParseResultSink>());
    }
}
