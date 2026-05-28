using System;
using System.Collections.Generic;
using System.Text;
using Dev.JoshBrunton.DynamicsEndpointDiscovery.Application.Ioc;
using Microsoft.Extensions.DependencyInjection;

namespace Dev.JoshBrunton.DynamicsEndpointDiscovery.Tests.Application.Tests.Tests.Ioc;

[TestClass]
public class ApplicationServiceRegistrarTests
{
    public static IEnumerable<object[]> GetServices()
    {
        var sc = new ServiceCollection();
        ApplicationServiceRegistrar.RegisterServices(sc, _ => false);
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
        ApplicationServiceRegistrar.RegisterServices(sc, _ => false);
        var sp = sc.BuildServiceProvider();
        ResolveService(sp, sd);
    }

    private static void ResolveService(IServiceProvider sp, ServiceDescriptor sd) =>
        _ = sd.IsKeyedService
            ? sp.GetRequiredKeyedService(sd.ServiceType, sd.ServiceKey)
            : sp.GetRequiredService(sd.ServiceType);
}
