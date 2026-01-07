using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.FluentUI.AspNetCore.Components;

namespace BlazorHybrid.Messages;

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
