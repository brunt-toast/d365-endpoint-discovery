using Dev.JoshBrunton.DynamicsEndpointDiscovery.Rcl.Enums;
using System.Reflection;

namespace Dev.JoshBrunton.DynamicsEndpointDiscovery.Tests.Rcl.Tests.DataSources.Tests.Services;

internal sealed class KnownCultureDataSourceAttribute : Attribute, ITestDataSource
{
    public IEnumerable<object[]> GetData(MethodInfo methodInfo)
    {
        return Enum.GetValues<KnownCultures>().Select(value => (object[])[value]);
    }

    public string? GetDisplayName(MethodInfo methodInfo, object?[]? data)
    {
        return $"{methodInfo.Name} ({data![0]})";
    }
}
