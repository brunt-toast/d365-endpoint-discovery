using Dev.JoshBrunton.DynamicsEndpointDiscovery.Cli.Ioc;
using Dev.JoshBrunton.DynamicsEndpointDiscovery.Tests.Cli.Tests.DataSources.Tests.Ioc;
using Microsoft.Extensions.DependencyInjection;

namespace Dev.JoshBrunton.DynamicsEndpointDiscovery.Tests.Cli.Tests.Tests.Ioc;

[TestClass]
public class CliServiceRegistrarTests
{
    [TestMethod]
    [ServiceDataSource]
    public void CanResolveAllServices(ServiceDescriptor sd)
    {
        var sc = new ServiceCollection();
        CliServiceRegistrar.RegisterServices(sc, _ => false);
        var sp = sc.BuildServiceProvider();
        ResolveService(sp, sd);
    }

    private static void ResolveService(IServiceProvider sp, ServiceDescriptor sd) =>
        _ = sd.IsKeyedService
            ? sp.GetRequiredKeyedService(sd.ServiceType, sd.ServiceKey)
            : sp.GetRequiredService(sd.ServiceType);

}
