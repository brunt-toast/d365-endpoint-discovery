using BlazorHybrid.Messages;
using CommunityToolkit.Mvvm.Messaging;
using Serilog.Core;
using Microsoft.FluentUI.AspNetCore.Components;
using Serilog.Events;

namespace BlazorHybrid.Logging.Sinks;

internal class ToastSink : ILogEventSink
{
    private readonly IMessenger _messenger;

    public ToastSink(IMessenger messenger)
    {
        _messenger = messenger;
    }

    public void Emit(LogEvent logEvent)
    {
        ToastIntent intent = logEvent.Level switch
        {
            LogEventLevel.Warning => ToastIntent.Warning,
            LogEventLevel.Error => ToastIntent.Error,
            _ => ToastIntent.Custom
        };

        if (intent is ToastIntent.Custom)
        {
            return;
        }

        _messenger.Send(new ShowToastMessage(intent, logEvent.RenderMessage()));
    }
}
