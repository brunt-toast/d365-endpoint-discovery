using Serilog;
using Serilog.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dev.JoshBrunton.DynamicsEndpointDiscovery.Core.Extensions.Serilog;

public static class LoggerExtensions
{
    [MessageTemplateFormatMethod(nameof(messageTemplate))]
    public static void LogInformation(this ILogger logger, string messageTemplate, params object?[]? propertyValues)
    {
        logger.Information(messageTemplate, propertyValues);
    }

    [MessageTemplateFormatMethod(nameof(messageTemplate))]
    public static void LogError(this ILogger logger, string messageTemplate, params object?[]? propertyValues)
    {
        logger.Error(messageTemplate, propertyValues);
    }
}
