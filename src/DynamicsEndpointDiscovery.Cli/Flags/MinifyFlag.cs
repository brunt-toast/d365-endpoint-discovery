using System.CommandLine;

namespace DynamicsEndpointDiscovery.Cli.Flags;

internal class MinifyFlag : Option<bool>
{
    public MinifyFlag() : base("--minify")
    {
        Description = "Remove redundant whitespace in output";
    }
}
