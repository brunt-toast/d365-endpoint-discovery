using System;
using System.Collections.Generic;
using System.CommandLine;
using System.Text;
using Dev.JoshBrunton.DynamicsEndpointDiscovery.Cli.Enums;

namespace Dev.JoshBrunton.DynamicsEndpointDiscovery.Cli.Options;

internal class LogStreamOption : Option<LogDestination>
{
    public LogStreamOption() : base("--log-stream")
    {
        Description = $"Send logs to a specific stream. {nameof(LogDestination.Default)} means that means that " +
                      $"information and below are sent to output, while warning and above are sent to error.";
    }
}
