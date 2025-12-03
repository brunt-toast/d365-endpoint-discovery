using System.CommandLine;
using Microsoft.Extensions.Logging;

namespace DynamicsEndpointDiscovery.Cli.Options;

internal class LogLevelOption : Option<LogLevel>
{
    public LogLevelOption() : base("--log-level", "-l")
    {
        DefaultValueFactory = _ => LogLevel.None;
        Description = "Level of logging";
    }
}
