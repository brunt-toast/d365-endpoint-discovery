using Dev.JoshBrunton.DynamicsEndpointDiscovery.Cli.Commands;
using Dev.JoshBrunton.DynamicsEndpointDiscovery.Cli.Ioc;
using Microsoft.Extensions.DependencyInjection;
using System.CommandLine;
using System.Reflection;

namespace Dev.JoshBrunton.DynamicsEndpointDiscovery.Tests.Cli.Tests.DataSources.Tests.Options;

internal sealed class OptionDataSourceAttribute : Attribute, ITestDataSource
{
    public IEnumerable<object[]> GetData(MethodInfo methodInfo)
    {
        foreach (var command in GetCommands())
        {
            foreach (var option in command.Options)
            {
                yield return [option];
            }
        }
    }

    public string? GetDisplayName(MethodInfo methodInfo, object?[]? data)
    {
        var option = (Option)data![0]!;
        return $"{methodInfo.Name} ({option.Name})";
    }

    private static IEnumerable<Command> GetCommands()
    {
        ServiceCollection sc = new ServiceCollection();
        CliServiceRegistrar.RegisterServices(sc, _ => false);
        var provider = sc.BuildServiceProvider();

        var rootCommand = provider.GetRequiredService<ServiceDiscoveryCommand>();
        var stack = new Stack<Command>();
        stack.Push(rootCommand);
        while (stack.Count != 0)
        {
            var next = stack.Pop();
            yield return next;
            foreach (var child in next.Subcommands)
            {
                stack.Push(child);
            }
        }
    }
}
