using Dev.JoshBrunton.DynamicsEndpointDiscovery.Tests.Cli.Tests.DataSources.Tests.Commands;
using System.CommandLine;

namespace Dev.JoshBrunton.DynamicsEndpointDiscovery.Tests.Cli.Tests.Tests.Commands;

[TestClass]
public class CommandTests
{
    [TestMethod]
    [CommandDataSource]
    public void Command_ShouldHaveDescription(Command command)
    {
        if (string.IsNullOrWhiteSpace(command.Description))
        {
            Assert.Fail($"{command.GetType().FullName} must have a description.");
        }
    }

}
