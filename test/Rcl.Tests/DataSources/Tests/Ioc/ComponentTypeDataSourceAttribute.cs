using Dev.JoshBrunton.DynamicsEndpointDiscovery.Rcl.Ioc;
using Microsoft.AspNetCore.Components;
using System.Reflection;

namespace Dev.JoshBrunton.DynamicsEndpointDiscovery.Tests.Rcl.Tests.DataSources.Tests.Ioc;

internal sealed class ComponentTypeDataSourceAttribute : Attribute, ITestDataSource
{
    public IEnumerable<object[]> GetData(MethodInfo methodInfo)
    {
        return GetComponentTypes().Select(x => new object[] { x });
    }

    public string? GetDisplayName(MethodInfo methodInfo, object?[]? data)
    {
        var componentType = (Type)data![0]!;
        return $"{methodInfo.Name} ({componentType.Name})";
    }

    public static IEnumerable<Type> GetComponentTypes()
    {
        return typeof(RclServiceRegistrar)
            .Assembly
            .GetTypes()
            .Where(t =>
                t is { IsAbstract: false, IsInterface: false, ContainsGenericParameters: false } &&
                typeof(ComponentBase).IsAssignableFrom(t));
    }
}
