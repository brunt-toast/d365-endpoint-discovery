using Dev.JoshBrunton.DynamicsEndpointDiscovery.Rcl.Ioc;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Dev.JoshBrunton.DynamicsEndpointDiscovery.Tests.Rcl.Tests.DataSources.Tests.Ioc;

internal sealed class ServiceDataSourceAttribute : Attribute, ITestDataSource
{
    public IEnumerable<object[]> GetData(MethodInfo methodInfo)
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

    public string? GetDisplayName(MethodInfo methodInfo, object?[]? data)
    {
        var sd = (ServiceDescriptor)data![0]!;
        return $"{methodInfo.Name} ({sd.ServiceType.Name})";
    }
}
