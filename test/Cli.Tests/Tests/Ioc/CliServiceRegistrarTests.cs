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
        CliServiceRegistrar.RegisterServices(sc);
        foreach (var defn in sc)
        {
            yield return [defn];
        }
    }

    [TestMethod]
    [DynamicData(nameof(GetServices))]
    public void CanResolveAllServices(ServiceDescriptor sd)
    {
        var sc = new ServiceCollection();
        CliServiceRegistrar.RegisterServices(sc);
        var sp = sc.BuildServiceProvider();
        sp.GetRequiredService(sd.ServiceType);
    }
}
