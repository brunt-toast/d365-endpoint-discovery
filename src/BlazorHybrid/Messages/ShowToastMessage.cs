using Microsoft.FluentUI.AspNetCore.Components;

namespace Dev.JoshBrunton.DynamicsEndpointDiscovery.BlazorHybrid.Messages;

public class ShowToastMessage
{
    public ToastIntent Intent { get; }
    public string Message { get; }

    public ShowToastMessage(ToastIntent intent, string message)
    {
        Intent = intent;
        Message = message;
    }
}
