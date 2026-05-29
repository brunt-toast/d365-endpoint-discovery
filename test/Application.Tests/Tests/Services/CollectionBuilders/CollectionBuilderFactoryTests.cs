using Dev.JoshBrunton.DynamicsEndpointDiscovery.Application.Ioc;
using Dev.JoshBrunton.DynamicsEndpointDiscovery.Application.Services.CollectionBuilders;
using Microsoft.Extensions.DependencyInjection;
using Dev.JoshBrunton.DynamicsEndpointDiscovery.Application.Enums;

namespace Dev.JoshBrunton.DynamicsEndpointDiscovery.Tests.Application.Tests.Tests.Services.CollectionBuilders;

[TestClass]
public class CollectionBuilderFactoryTests
{
    public static IEnumerable<object[]> GetOutputSchemas()
    {
        return Enum.GetValues<OutputSchemas>().Select(x => new object[] { x });
    }

    [TestMethod]
    [DynamicData(nameof(GetOutputSchemas))]
    public void ShouldResolve(OutputSchemas schema)
    {
        var sc = new ServiceCollection();
        ApplicationServiceRegistrar.RegisterServices(sc, _ => false);
        var sp = sc.BuildServiceProvider();

        var sut = sp.GetRequiredService<CollectionBuilderFactory>();
        sut.GetCollectionBuilder(schema);
    }
}
