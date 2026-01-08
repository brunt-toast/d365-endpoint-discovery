using System.CommandLine;
using Dev.JoshBrunton.DynamicsEndpointDiscovery.Tests.Cli.Tests.Generators.Options;

namespace Dev.JoshBrunton.DynamicsEndpointDiscovery.Tests.Cli.Tests.Tests.Options;

[TestClass]
public class OptionTests
{
    [TestMethod]
    [DynamicData(nameof(OptionsGenerator.GetOptions), typeof(OptionsGenerator))]
    public void Option_ShouldHaveDescription(Option option)
    {
        if (string.IsNullOrWhiteSpace(option.Description))
        {
            Assert.Fail($"{option.GetType().FullName} must have a description.");
        }
    }
}
