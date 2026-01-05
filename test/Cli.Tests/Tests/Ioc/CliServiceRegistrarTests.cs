using Dev.JoshBrunton.DynamicsEndpointDiscovery.Cli.Ioc;
using Microsoft.Extensions.DependencyInjection;

namespace Dev.JoshBrunton.DynamicsEndpointDiscovery.Tests.Cli.Tests.Tests.Ioc;

[TestClass]
public class CliServiceRegistrarTests
{
    [TestMethod]
    public void CanResolveAllServices()
    {
        var sc = new ServiceCollection();
        CliServiceRegistrar.RegisterServices(sc);
        var provider = sc.BuildServiceProvider();

        foreach (var defn in sc)
        {
            provider.GetRequiredService(defn.ServiceType);
        }
    }
}
