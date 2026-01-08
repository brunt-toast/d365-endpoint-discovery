using System.CommandLine;
using Serilog.Core;
using Serilog.Events;

namespace Dev.JoshBrunton.DynamicsEndpointDiscovery.Cli.Logging.Sinks;

internal class CommandParseResultSink : ILogEventSink, ICommandParseResultSink
{
    private TextWriter? _outputWriter;
    private TextWriter? _errorWriter;
    private LogEventLevel _logLevel;

    public IDisposable Configure(ParseResult parseResult, LogEventLevel logLevel = LogEventLevel.Warning)
    {
        _logLevel = logLevel;
        _outputWriter = parseResult.InvocationConfiguration.Output;
        _errorWriter = parseResult.InvocationConfiguration.Error;
        return new CommandParseResultSinkConfigurationDisposable(this);
    }

    public void Emit(LogEvent logEvent)
    {
        if (logEvent.Level < _logLevel)
        {
            return;
        }

        var writer = logEvent.Level >= LogEventLevel.Warning ? _errorWriter : _outputWriter;
        writer?.WriteLine(logEvent.RenderMessage());
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
    IDisposable Configure(ParseResult parseResult, LogEventLevel logLevel = LogEventLevel.Warning);
}