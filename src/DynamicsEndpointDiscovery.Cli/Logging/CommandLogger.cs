using System.CommandLine;
using Microsoft.Extensions.Logging;

namespace DynamicsEndpointDiscovery.Cli.Logging;

internal class CommandLogger : ILogger
{
    private readonly LogLevel _logLevel;
    private readonly TextWriter _outStream;
    private readonly TextWriter _errStream;

    public CommandLogger(ParseResult parseResult, LogLevel logLevel)
    {
        _logLevel = logLevel;
        _outStream = parseResult.InvocationConfiguration.Output;
        _errStream = parseResult.InvocationConfiguration.Error;
    }

    public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception? exception, Func<TState, Exception?, string> formatter)
    {
        if (!IsEnabled(logLevel))
        {
            return;
        }

        if (logLevel >= LogLevel.Warning)
        {
            _errStream.WriteLine(formatter.Invoke(state, exception));
        }
        else
        {
            _outStream.WriteLine(formatter.Invoke(state, exception));
        }
    }

    public bool IsEnabled(LogLevel logLevel)
    {
        return logLevel >= _logLevel;
    }

    public IDisposable BeginScope<TState>(TState state) where TState : notnull
    {
        return new NullDisposable();
    }

    private class NullDisposable : IDisposable
    {
        public void Dispose()
        {
            // noop 
        }
    }
}
