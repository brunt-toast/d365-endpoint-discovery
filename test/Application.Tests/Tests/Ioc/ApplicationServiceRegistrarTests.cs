using System;
using System.Collections.Generic;
using System.Text;
using Dev.JoshBrunton.DynamicsEndpointDiscovery.Application.Ioc;
using Microsoft.Extensions.DependencyInjection;

namespace Dev.JoshBrunton.DynamicsEndpointDiscovery.Tests.Application.Tests.Tests.Ioc;

[TestClass]
public class ApplicationServiceRegistrarTests
{
    [TestMethod]
    public void CanResolveAllServices()
    {
        var sc = new ServiceCollection();
        ApplicationServiceRegistrar.RegisterServices(sc);
        var provider = sc.BuildServiceProvider();

        foreach (var defn in sc)
        {
            provider.GetRequiredService(defn.ServiceType);
        }
    }
}
