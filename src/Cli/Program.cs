using System.CommandLine.Parsing;
using Dev.JoshBrunton.DynamicsEndpointDiscovery.Cli.Commands;
using Dev.JoshBrunton.DynamicsEndpointDiscovery.Cli.Ioc;
using Microsoft.Extensions.DependencyInjection;

namespace Dev.JoshBrunton.DynamicsEndpointDiscovery.Cli;

internal class Program
{
    public static async Task<int> Main(string[] args)
    {
#if DEBUG
        if (args.Length == 0)
        {
            while (true)
            {
                Console.Write(@"dynsvcdiscovery ");
                string commandLine = Console.ReadLine() ?? string.Empty;
                string[] newArgs = CommandLineParser.SplitCommandLine(commandLine).ToArray();

                DateTimeOffset start = DateTimeOffset.Now;
                int exitCode = await Main(newArgs);
                DateTimeOffset end = DateTimeOffset.Now;

                Console.WriteLine();
                Console.WriteLine(@"====================");
                Console.WriteLine(@"Run report");
                Console.WriteLine(@$"Args: [{Environment.NewLine}" +
                                  @$"{string.Join($",{Environment.NewLine}", newArgs.Select(x => $"    \"{x}\""))}" +
                                  @$"{Environment.NewLine}]");
                Console.WriteLine(@$"Duration: {(end - start).TotalSeconds}s");
                Console.WriteLine(@$"Exit code: {exitCode}");
                Console.WriteLine(@"====================");
                Console.WriteLine();
            }
        }
#endif

        ServiceCollection sc = new ServiceCollection();
        CliServiceRegistrar.RegisterServices(sc, _ => args.Contains("--mock"));
        var services = sc.BuildServiceProvider();

        return await services.GetRequiredService<ServiceDiscoveryCommand>().Parse(args).InvokeAsync();
    }
}
