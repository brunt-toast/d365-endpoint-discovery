using Dev.JoshBrunton.DynamicsEndpointDiscovery.Application.Enums;
using System.Reflection;

namespace Dev.JoshBrunton.DynamicsEndpointDiscovery.Tests.Application.Tests.DataSources.Tests.Services.CollectionBuilders;

internal sealed class OutputSchemaDataSourceAttribute : Attribute, ITestDataSource
{
    public IEnumerable<object[]> GetData(MethodInfo methodInfo)
    {
        return Enum.GetValues<OutputSchemas>().Select(x => new object[] { x });
    }

    public string? GetDisplayName(MethodInfo methodInfo, object?[]? data)
    {
        return $"{methodInfo.Name} ({data![0]})";
    }
}
