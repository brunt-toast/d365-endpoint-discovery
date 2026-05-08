using System.CommandLine;
using Serilog.Events;

namespace Dev.JoshBrunton.DynamicsEndpointDiscovery.Cli.Options;

internal class LogLevelOption : Option<LogEventLevel>
{
    public LogLevelOption() : base("--log-level", "-l")
    {
        DefaultValueFactory = _ => LogEventLevel.Warning;
        Description = LogLevelOptionResources.Description;
    }
}
