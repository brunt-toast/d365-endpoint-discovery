using System.CommandLine;
using Dev.JoshBrunton.DynamicsEndpointDiscovery.Tests.Cli.Tests.Generators.Commands;

namespace Dev.JoshBrunton.DynamicsEndpointDiscovery.Tests.Cli.Tests.Generators.Options;

internal static class OptionsGenerator
{
    public static IEnumerable<object[]> GetOptions()
    {
        var commands = CommandGenerator
            .GetCommands()
            .SelectMany(x => x)
            .Cast<Command>();

        foreach (var command in commands)
        {
            foreach (var arg in command.Options)
            {
                yield return [arg];
            }
        }
    }
}
