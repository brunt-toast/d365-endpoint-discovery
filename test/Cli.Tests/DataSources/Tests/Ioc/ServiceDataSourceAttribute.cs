using Dev.JoshBrunton.DynamicsEndpointDiscovery.Cli.Ioc;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Dev.JoshBrunton.DynamicsEndpointDiscovery.Tests.Cli.Tests.DataSources.Tests.Ioc;

internal sealed class ServiceDataSourceAttribute : Attribute, ITestDataSource
{
    public IEnumerable<object[]> GetData(MethodInfo methodInfo)
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

    public string? GetDisplayName(MethodInfo methodInfo, object?[]? data)
    {
        var sd = (ServiceDescriptor)data![0]!;
        return $"{methodInfo.Name} ({sd.ServiceType.Name})";
    }
}
