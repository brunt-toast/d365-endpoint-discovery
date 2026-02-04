using System.CommandLine;
using System.CommandLine.Parsing;

namespace Dev.JoshBrunton.DynamicsEndpointDiscovery.Cli.Options;

internal class LogToStdErrOption : Option<bool>
{
    public LogToStdErrOption() : base("--log-to-stderr", "-2")
    {
        Description = "Send all logs to the error stream, regardless of their log level.";
        Validators.Add(arg =>
        {
            if (!arg.GetValue(this) || arg.Parent is not CommandResult cr)
            {
                return;
            }

            if (cr.Children.OfType<OptionResult>()
                    .FirstOrDefault(o => o.Option is LogToStdOutOption)
                    ?.GetValueOrDefault<bool>() == true)
            {
                arg.AddError($"{Name} can't be used when also using {new LogToStdOutOption().Name}");
            }
        });
    }
}
