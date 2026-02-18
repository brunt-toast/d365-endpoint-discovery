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
        ApplicationServiceRegistrar.RegisterServices(sc);
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
        ApplicationServiceRegistrar.RegisterServices(sc);
        var sp = sc.BuildServiceProvider();
        sp.GetRequiredService(sd.ServiceType);
    }
}
