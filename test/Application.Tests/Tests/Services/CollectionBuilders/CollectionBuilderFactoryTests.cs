using Dev.JoshBrunton.DynamicsEndpointDiscovery.Application.Enums;
using Dev.JoshBrunton.DynamicsEndpointDiscovery.Application.Ioc;
using Dev.JoshBrunton.DynamicsEndpointDiscovery.Application.Services.CollectionBuilders;
using Dev.JoshBrunton.DynamicsEndpointDiscovery.Tests.Application.Tests.DataSources.Tests.Services.CollectionBuilders;
using Microsoft.Extensions.DependencyInjection;

namespace Dev.JoshBrunton.DynamicsEndpointDiscovery.Tests.Application.Tests.Tests.Services.CollectionBuilders;

[TestClass]
public class CollectionBuilderFactoryTests
{
    [TestMethod]
    [OutputSchemaDataSource]
    public void ShouldResolve(OutputSchemas schema)
    {
        var sc = new ServiceCollection();
        ApplicationServiceRegistrar.RegisterServices(sc, _ => false);
        var sp = sc.BuildServiceProvider();

        var sut = sp.GetRequiredService<CollectionBuilderFactory>();
        sut.GetCollectionBuilder(schema);
    }

}
