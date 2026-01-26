using BlazorHybrid.ViewModels;
using CommunityToolkit.Maui.Storage;
using Dev.JoshBrunton.DynamicsEndpointDiscovery.Application.Ioc;
using Dev.JoshBrunton.DynamicsEndpointDiscovery.Application.Services;
using Dev.JoshBrunton.DynamicsEndpointDiscovery.Application.Services.CollectionBuilders;
using Dev.JoshBrunton.DynamicsEndpointDiscovery.Lib.Ax.Ioc;
using Serilog;
using System;
using System.Collections.Generic;
using System.Text;
using BlazorHybrid.Logging.Sinks;
using CommunityToolkit.Mvvm.Messaging;
using Microsoft.FluentUI.AspNetCore.Components;
using Serilog.Core;

namespace BlazorHybrid.Ioc;

internal static class BlazorHybridServiceRegistrar
{
    public static void RegisterServices(IServiceCollection sc)
    {
        ApplicationServiceRegistrar.RegisterServices(sc);

        sc.AddFluentUIComponents();
        sc.AddMauiBlazorWebView();

        sc.AddSingleton<ILauncher>(_ => Launcher.Default);
        sc.AddSingleton<IFileSaver>(_ => FileSaver.Default);
        sc.AddSingleton<IFileSystem>(_ => FileSystem.Current);
        sc.AddSingleton<ISecureStorage>(_ => SecureStorage.Default);
        sc.AddSingleton<IMessenger, WeakReferenceMessenger>();

        sc.AddSingleton<ILogEventSink, ToastSink>();
        sc.AddSingleton<ILogEventSink>(x => new AppdataFileSink(x.GetRequiredService<IFileSystem>(), 
            x.GetRequiredService<IMessenger>()).Init());

        sc.AddTransient<ICredentialsViewModel, CredentialsViewModel>();
        sc.AddTransient<ISelectGroupsViewModel, SelectGroupsViewModel>();
        sc.AddTransient<ISelectServicesViewModel, SelectServicesViewModel>();
        sc.AddTransient<ISelectOperationsViewModel, SelectOperationsViewModel>();
        sc.AddTransient<IBuildCollectionViewModel, BuildCollectionViewModel>();


#if DEBUG
        sc.AddBlazorWebViewDeveloperTools();
#endif
    }
}
