using System.CommandLine;
using System.CommandLine.Parsing;

namespace Dev.JoshBrunton.DynamicsEndpointDiscovery.Cli.Options;

internal class LogToStdOutOption : Option<bool>
{
    public LogToStdOutOption() : base("--log-to-stdout", "-1")
    {
        Description = "Send all logs to the output stream, regardless of their log level.";

        Validators.Add(arg =>
        {
            if (!arg.GetValue(this) || arg.Parent is not CommandResult cr)
            {
                return;
            }

            if (cr.Children.OfType<OptionResult>()
                    .FirstOrDefault(o => o.Option is LogToStdErrOption)
                    ?.GetValueOrDefault<bool>() == true)
            {
                arg.AddError($"{Name} can't be used when also using {new LogToStdErrOption().Name}"); 
            }
        });
    }
}
