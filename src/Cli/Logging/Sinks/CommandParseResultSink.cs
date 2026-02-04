using System.CommandLine;
using Serilog.Core;
using Serilog.Events;

namespace Dev.JoshBrunton.DynamicsEndpointDiscovery.Cli.Logging.Sinks;

internal class CommandParseResultSink : ILogEventSink, ICommandParseResultSink
{
    private TextWriter? _outputWriter;
    private TextWriter? _errorWriter;
    private LogEventLevel _logLevel;

    public IDisposable Configure(ParseResult parseResult, 
        LogEventLevel logLevel = LogEventLevel.Warning, 
        bool sendAllToOut = false, 
        bool sendAllToError = false)
    {
        if (sendAllToOut && sendAllToError)
        {
            throw new ArgumentException($"{nameof(sendAllToOut)} and {nameof(sendAllToError)} cannot both be true", 
                nameof(sendAllToError));
        }

        _logLevel = logLevel;
        _outputWriter = sendAllToError 
            ? parseResult.InvocationConfiguration.Error
            : parseResult.InvocationConfiguration.Output;
        _errorWriter = sendAllToOut
            ? parseResult.InvocationConfiguration.Output
            : parseResult.InvocationConfiguration.Error;
        return new CommandParseResultSinkConfigurationDisposable(this);
    }

    public void Emit(LogEvent logEvent)
    {
        if (logEvent.Level < _logLevel)
        {
            return;
        }

        int ansiCode = logEvent.Level switch
        {
            LogEventLevel.Verbose => 0,
            LogEventLevel.Debug => 0,
            LogEventLevel.Information => 34,
            LogEventLevel.Warning => 33,
            LogEventLevel.Error => 31,
            LogEventLevel.Fatal => 35,
            _ => throw new ArgumentOutOfRangeException()
        };

        string levelAbbr = logEvent.Level switch
        {
            LogEventLevel.Verbose => "TRC",
            LogEventLevel.Debug => "DBG",
            LogEventLevel.Information => "INF",
            LogEventLevel.Warning => "WRN",
            LogEventLevel.Error => "ERR",
            LogEventLevel.Fatal => "FTL",
            _ => throw new ArgumentOutOfRangeException()
        };

        var writer = logEvent.Level >= LogEventLevel.Warning ? _errorWriter : _outputWriter;
        writer?.WriteLine($"\e[{ansiCode}m[{levelAbbr}]\e[0m {logEvent.RenderMessage()}");
    }

    private class CommandParseResultSinkConfigurationDisposable : IDisposable
    {
        private readonly CommandParseResultSink _instance;

        public CommandParseResultSinkConfigurationDisposable(CommandParseResultSink instance)
        {
            _instance = instance;
        }
        
        public void Dispose()
        {
            _instance._logLevel = default;
            _instance._outputWriter = null;
            _instance._errorWriter = null;
        }
    }
}

public interface ICommandParseResultSink
{
    IDisposable Configure(ParseResult parseResult, 
        LogEventLevel logLevel = LogEventLevel.Warning,
        bool sendAllToOut = false, 
        bool sendAllToError = false);
}