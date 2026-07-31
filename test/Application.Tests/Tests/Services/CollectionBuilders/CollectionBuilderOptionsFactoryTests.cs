using Dev.JoshBrunton.DynamicsEndpointDiscovery.Application.Enums;
using Dev.JoshBrunton.DynamicsEndpointDiscovery.Application.Services.CollectionBuilders.Options;
using Dev.JoshBrunton.DynamicsEndpointDiscovery.Tests.Application.Tests.DataSources.Tests.Services.CollectionBuilders;

namespace Dev.JoshBrunton.DynamicsEndpointDiscovery.Tests.Application.Tests.Tests.Services.CollectionBuilders;

[TestClass]
public class CollectionBuilderOptionsFactoryTests
{
    [TestMethod]
    [OutputSchemaDataSource]
    public void ShouldCreateOptionsForSchema(OutputSchemas schema)
    {
        var result = CollectionBuilderOptionsFactory.Create(schema);

        Assert.IsInstanceOfType<ICollectionBuilderOptions>(result);
    }

    [TestMethod]
    public void ShouldDisableMinifyForYamlSerialisedOptions()
    {
        var sut = new OpenApiCollectionBuilderOptions
        {
            OutputFormat = OutputFormats.Yaml,
            Minify = true
        };

        sut.Validate();

        Assert.IsFalse(sut.Minify);
        Assert.IsTrue(sut.IsOptionDisabled(nameof(OpenApiCollectionBuilderOptions.Minify)));
    }

    [TestMethod]
    public void ShouldExposeNewtonsoftJsonOptionForCSharp()
    {
        var sut = new CSharpCollectionBuilderOptions();
        var editableProperties = sut
            .GetType()
            .GetProperties()
            .Where(x => x.CanRead && x.CanWrite)
            .ToArray();

        Assert.HasCount(2, editableProperties);
        Assert.AreEqual(nameof(CSharpCollectionBuilderOptions.IncludeNewtonsoftJsonAttributes), editableProperties[0].Name);
        Assert.AreEqual(nameof(CSharpCollectionBuilderOptions.IncludeSystemTextJsonAttributes), editableProperties[1].Name);
        Assert.IsTrue(sut.IncludeNewtonsoftJsonAttributes);
        Assert.IsTrue(sut.IncludeSystemTextJsonAttributes);
    }
}
