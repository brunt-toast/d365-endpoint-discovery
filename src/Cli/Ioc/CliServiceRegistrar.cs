using Dev.JoshBrunton.DynamicsEndpointDiscovery.Application.Ioc;
using Dev.JoshBrunton.DynamicsEndpointDiscovery.Cli.Commands;
using Microsoft.Extensions.DependencyInjection;
using Dev.JoshBrunton.DynamicsEndpointDiscovery.Cli.Logging.Sinks;
using Dev.JoshBrunton.DynamicsEndpointDiscovery.Cli.Options;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Serilog.Core;

namespace Dev.JoshBrunton.DynamicsEndpointDiscovery.Cli.Ioc;

public static class CliServiceRegistrar
{
    public static void RegisterServices(IServiceCollection sc, Func<IServiceProvider, bool> mockPredicate)
    {
        ApplicationServiceRegistrar.RegisterServices(sc, mockPredicate);

        sc.AddSingleton<ILoggerFactory, NullLoggerFactory>();
        sc.AddLocalization();

        sc.AddTransient<ClientIdOption>();
        sc.AddTransient<ClientSecretOption>();
        sc.AddTransient<ResourceOption>();
        sc.AddTransient<TokenRequestEndpointOption>();
        sc.AddTransient<GrepGroupsOption>();
        sc.AddTransient<GrepServicesOption>();
        sc.AddTransient<GrepOperationsOption>();
        sc.AddTransient<CollectionNameOption>();
        sc.AddTransient<SchemaOption>();
        sc.AddTransient<FormatOption>();
        sc.AddTransient<LogLevelOption>();
        sc.AddTransient<LogStreamOption>();
        sc.AddTransient<MaxConnectionsOption>();
        sc.AddTransient<IgnoreSslOption>();
        sc.AddTransient<MinifyOption>();
        sc.AddTransient<TenantIdOption>();
        sc.AddTransient<AuthKindOption>();
        sc.AddTransient<AcceptThumbprintOption>();

        sc.AddTransient<ServiceDiscoveryCommand>();

        sc.AddSingleton<CommandParseResultSink>();
        sc.AddSingleton<ILogEventSink>(x => x.GetRequiredService<CommandParseResultSink>());
        sc.AddSingleton<ICommandParseResultSink>(x => x.GetRequiredService<CommandParseResultSink>());
    }
}
