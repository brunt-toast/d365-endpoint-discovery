using System;
using System.Collections.Generic;
using System.CommandLine;
using System.Text;
using Dev.JoshBrunton.DynamicsEndpointDiscovery.Tests.Cli.Tests.Generators.Commands;

namespace Dev.JoshBrunton.DynamicsEndpointDiscovery.Tests.Cli.Tests.Tests.Commands;

[TestClass]
public class CommandTests
{
    [TestMethod]
    [DynamicData(nameof(CommandGenerator.GetCommands), typeof(CommandGenerator))]
    public void Command_ShouldHaveDescription(Command command)
    {
        if (string.IsNullOrWhiteSpace(command.Description))
        {
            Assert.Fail($"{command.GetType().FullName} must have a description.");
        }
    }


}
