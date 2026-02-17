using System.CommandLine;

namespace Dev.JoshBrunton.DynamicsEndpointDiscovery.Cli.Options;

internal class MaxConnectionsOption : Option<int>
{
    public MaxConnectionsOption() : base("--max-connections", "-m")
    {
        Description = "Set the maximum number of connections. This helps to avoid socket exhaustion for large jobs, " +
                      "but massively degrades performance. '0' represents no limit, but the application may encounter " +
                      "additional errors due to insufficient system resources not checked or managed by the program.";
        DefaultValueFactory = _ => 0;
    }
}
