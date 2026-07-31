using Dev.JoshBrunton.DynamicsEndpointDiscovery.Tests.Cli.Tests.DataSources.Tests.Options;
using System.CommandLine;

namespace Dev.JoshBrunton.DynamicsEndpointDiscovery.Tests.Cli.Tests.Tests.Options;

[TestClass]
public class OptionTests
{
    [TestMethod]
    [OptionDataSource]
    public void Option_ShouldHaveDescription(Option option)
    {
        if (string.IsNullOrWhiteSpace(option.Description))
        {
            Assert.Fail($"{option.GetType().FullName} must have a description.");
        }
    }

}
