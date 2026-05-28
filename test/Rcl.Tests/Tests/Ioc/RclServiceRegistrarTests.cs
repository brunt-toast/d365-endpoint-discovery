using Dev.JoshBrunton.DynamicsEndpointDiscovery.Rcl.Ioc;
using Microsoft.AspNetCore.Components;

namespace Dev.JoshBrunton.DynamicsEndpointDiscovery.Tests.Rcl.Tests.Tests.Ioc;

[TestClass]
public class RclServiceRegistrarTests
{
    public static IEnumerable<object[]> GetServices()
    {
        var sc = new ServiceCollection();
        RclServiceRegistrar.RegisterServices(sc);
        foreach (var defn in sc)
        {
            if (defn.ServiceType.FullName?.StartsWith("Dev.JoshBrunton.DynamicsEndpointDiscovery") == true)
            {
                if (!defn.IsKeyedService)
                {
                    yield return [defn];
                }
            }
        }
    }

    [TestMethod]
    [DynamicData(nameof(GetServices))]
    public void GetRequiredService_ShouldNotThrow(ServiceDescriptor sd)
    {
        var sc = new ServiceCollection();
        RclServiceRegistrar.RegisterServices(sc);
        var sp = sc.BuildServiceProvider();
        sp.GetRequiredService(sd.ServiceType);
    }

    public static IEnumerable<object[]> GetAllComponentTypes()
    {
        return typeof(RclServiceRegistrar)
            .Assembly
            .GetTypes()
            .Where(t =>
                t is { IsAbstract: false, IsInterface: false, ContainsGenericParameters: false } &&
                typeof(ComponentBase).IsAssignableFrom(t))
            .Select(x => new object[] { x });
    }

    [TestMethod]
    [DynamicData(nameof(GetAllComponentTypes))]
    public void ComponentGeneration_ShouldNotThrow(Type componentType)
    {
        IServiceCollection sc = new ServiceCollection();
        RclServiceRegistrar.RegisterServices(sc);
        foreach (var t in GetAllComponentTypes())
        {
            sc.AddTransient((Type)t[0]);
        }
        IServiceProvider sp = sc.BuildServiceProvider();

        sp.GetRequiredService(componentType);
    }
}
