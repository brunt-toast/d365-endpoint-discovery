using Dev.JoshBrunton.DynamicsEndpointDiscovery.Cli.Ioc;
using Microsoft.Extensions.DependencyInjection;
using System.CommandLine;
using Dev.JoshBrunton.DynamicsEndpointDiscovery.Cli.Commands;

namespace Dev.JoshBrunton.DynamicsEndpointDiscovery.Tests.Cli.Tests.Generators.Commands;

internal static class CommandGenerator
{
    public static IEnumerable<object[]> GetCommands()
    {
        ServiceCollection sc = new ServiceCollection();
        CliServiceRegistrar.RegisterServices(sc);
        var provider = sc.BuildServiceProvider();

        var rootCommand = provider.GetRequiredService<ServiceDiscoveryCommand>();
        var stack = new Stack<Command>();
        stack.Push(rootCommand);
        while (stack.Count != 0)
        {
            var next = stack.Pop();
            yield return [next];
            foreach (var child in next.Subcommands)
            {
                stack.Push(child);
            }
        }
    }
}
