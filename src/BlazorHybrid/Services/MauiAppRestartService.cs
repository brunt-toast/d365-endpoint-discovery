using System.Diagnostics;
using Dev.JoshBrunton.DynamicsEndpointDiscovery.Rcl.Services;

namespace Dev.JoshBrunton.DynamicsEndpointDiscovery.BlazorHybrid.Services;

internal sealed class MauiAppRestartService : IAppRestartService
{
    public Task RestartAsync()
    {
        string? executablePath = Environment.ProcessPath;
        if (!string.IsNullOrWhiteSpace(executablePath))
        {
            Process.Start(new ProcessStartInfo
            {
                FileName = executablePath,
                UseShellExecute = true
            });
        }

        global::Microsoft.Maui.Controls.Application.Current?.Quit();
        Environment.Exit(0);

        return Task.CompletedTask;
    }
}
