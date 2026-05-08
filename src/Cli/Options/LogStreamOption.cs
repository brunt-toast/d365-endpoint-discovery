using System;
using System.Collections.Generic;
using System.CommandLine;
using System.Text;
using Dev.JoshBrunton.DynamicsEndpointDiscovery.Cli.Enums;
using Microsoft.Extensions.Localization;

namespace Dev.JoshBrunton.DynamicsEndpointDiscovery.Cli.Options;

internal class LogStreamOption : Option<LogDestination>
{
    public LogStreamOption(IStringLocalizer<LogStreamOptionResources> localizer) : base("--log-stream")
    {
        Description = localizer[nameof(LogStreamOptionResources.Description), nameof(LogDestination.Default)];
    }
}
