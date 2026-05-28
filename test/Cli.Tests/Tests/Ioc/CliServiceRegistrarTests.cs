using Dev.JoshBrunton.DynamicsEndpointDiscovery.Application.Ioc;
using Dev.JoshBrunton.DynamicsEndpointDiscovery.Cli.Ioc;
using Microsoft.Extensions.DependencyInjection;

namespace Dev.JoshBrunton.DynamicsEndpointDiscovery.Tests.Cli.Tests.Tests.Ioc;

[TestClass]
public class CliServiceRegistrarTests
{
    public static IEnumerable<object[]> GetServices()
    {
        var sc = new ServiceCollection();
        CliServiceRegistrar.RegisterServices(sc, _ => false);
        foreach (var defn in sc)
        {
            if (defn.ServiceType.FullName?.StartsWith("Dev.JoshBrunton.DynamicsEndpointDiscovery") == true)
            {
                yield return [defn];
            }
        }
    }

    [TestMethod]
    [DynamicData(nameof(GetServices))]
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
