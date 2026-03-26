using Dev.JoshBrunton.DynamicsEndpointDiscovery.Cli.Commands;
using Dev.JoshBrunton.DynamicsEndpointDiscovery.Cli.Ioc;
using Microsoft.Extensions.DependencyInjection;

ServiceCollection sc = new ServiceCollection();
CliServiceRegistrar.RegisterServices(sc);
var services = sc.BuildServiceProvider();

return await services.GetRequiredService<ServiceDiscoveryCommand>().Parse(args).InvokeAsync();