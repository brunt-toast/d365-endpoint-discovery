using System.CommandLine;

namespace Dev.JoshBrunton.DynamicsEndpointDiscovery.Cli.Options;

internal class LogToStdErrOption : Option<bool>
{
    public LogToStdErrOption() : base("--log-to-stderr", "-2")
    {
        Description = "Send all logs to the error stream, regardless of their log level.";
    }
}
