using Dev.JoshBrunton.DynamicsEndpointDiscovery.Application.Ioc;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Dev.JoshBrunton.DynamicsEndpointDiscovery.Tests.Application.Tests.DataSources.Tests.Ioc;

internal sealed class ServiceDataSourceAttribute : Attribute, ITestDataSource
{
    public IEnumerable<object[]> GetData(MethodInfo methodInfo)
    {
        var sc = new ServiceCollection();
        ApplicationServiceRegistrar.RegisterServices(sc, _ => false);
        foreach (var defn in sc)
        {
            yield return [defn];
        }
    }

    public string? GetDisplayName(MethodInfo methodInfo, object?[]? data)
    {
        var sd = (ServiceDescriptor)data![0]!;
        return $"{methodInfo.Name} ({sd.ServiceType.Name})";
    }
}
