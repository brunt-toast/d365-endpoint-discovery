using Serilog.Core;
using Serilog.Events;

namespace Dev.JoshBrunton.DynamicsEndpointDiscovery.Application.Logging.Sinks;

internal class DebuggerSink : ILogEventSink
{
    public void Emit(LogEvent logEvent)
    {
        if (!System.Diagnostics.Debugger.IsLogging())
        {
            return;
        }

        System.Diagnostics.Debugger.Log((int)logEvent.Level, nameof(DebuggerSink), logEvent.RenderMessage() + "\n");
    }
}
