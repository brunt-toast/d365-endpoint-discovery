using Dev.JoshBrunton.DynamicsEndpointDiscovery.Application.Enums;
using Dev.JoshBrunton.DynamicsEndpointDiscovery.Application.Ioc;
using Dev.JoshBrunton.DynamicsEndpointDiscovery.Application.Requests;
using Dev.JoshBrunton.DynamicsEndpointDiscovery.Application.Services;
using Dev.JoshBrunton.DynamicsEndpointDiscovery.Application.Services.CollectionBuilders.Options;
using Dev.JoshBrunton.DynamicsEndpointDiscovery.Lib.Ax.Types;
using Microsoft.Extensions.DependencyInjection;

namespace Dev.JoshBrunton.DynamicsEndpointDiscovery.Tests.Application.Tests.Tests.Services;

[TestClass]
public class MockCollectionBuilderFlowTests
{
    [TestMethod]
    public async Task ShouldGenerateCSharpLocalisationResourcesFromMockData()
    {
        var sc = new ServiceCollection();
        ApplicationServiceRegistrar.RegisterServices(sc, _ => true);
        var sp = sc.BuildServiceProvider();
        var sut = sp.GetRequiredService<IMainService>();

        var result = await sut.BuildCustomCollection(new BuildCustomCollectionRequest
        {
            OutputSchema = OutputSchemas.CSharp,
            CollectionName = "Collection",
            Resource = "https://example.test",
            Services =
            [
                new DynSvcGroup
                {
                    Name = "Sample",
                    Services = []
                }
            ],
            Options = new CSharpCollectionBuilderOptions()
        });

        StringAssert.Contains(result, "#region Resources");
        StringAssert.Contains(result, "public class SampleContractResources_en");
        StringAssert.Contains(result, "public class SampleContractResources_es");
        StringAssert.Contains(result, "public class SampleContractResources_fr");
        StringAssert.Contains(result, "public static string SampleContract => \"Sample Contract\";");
        StringAssert.Contains(result, "public static string Description => \"Description\";");
        StringAssert.Contains(result, "public static string Description => \"Descripción\";");
        StringAssert.Contains(result, "public static string EffectiveDate => \"Date d'effet\";");
        StringAssert.Contains(result, "[Display(");
    }
}
