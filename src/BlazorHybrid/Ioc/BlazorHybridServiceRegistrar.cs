using CommunityToolkit.Maui.Storage;
using CommunityToolkit.Mvvm.Messaging;
using Dev.JoshBrunton.DynamicsEndpointDiscovery.Application.Ioc;
using Dev.JoshBrunton.DynamicsEndpointDiscovery.Rcl.Ioc;
using Dev.JoshBrunton.DynamicsEndpointDiscovery.Rcl.Logging.Sinks;
using Dev.JoshBrunton.DynamicsEndpointDiscovery.Rcl.ViewModels;
using Microsoft.FluentUI.AspNetCore.Components;
using Serilog.Core;

namespace Dev.JoshBrunton.DynamicsEndpointDiscovery.BlazorHybrid.Ioc;

internal static class BlazorHybridServiceRegistrar
{
    public static void RegisterServices(IServiceCollection sc)
    {
        RclServiceRegistrar.RegisterServices(sc);

        sc.AddMauiBlazorWebView();

        sc.AddSingleton<ILogEventSink, ToastSink>();
        sc.AddSingleton<ILogEventSink>(x => new AppdataFileSink(x.GetRequiredService<IFileSystem>(), 
            x.GetRequiredService<IMessenger>()).Init());


#if DEBUG
        sc.AddBlazorWebViewDeveloperTools();
#endif
    }
}
