using System.CommandLine;

namespace Dev.JoshBrunton.DynamicsEndpointDiscovery.Cli.Options;

internal class LogToStdOutOption : Option<bool>
{
    public LogToStdOutOption() : base("--log-to-stdout", "-1")
    {
        Description = "Send all logs to the output stream, regardless of their log level.";
    }
}
