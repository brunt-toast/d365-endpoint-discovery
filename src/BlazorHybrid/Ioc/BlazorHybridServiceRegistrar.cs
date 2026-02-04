using CommunityToolkit.Maui.Storage;
using CommunityToolkit.Mvvm.Messaging;
using Dev.JoshBrunton.DynamicsEndpointDiscovery.Application.Ioc;
using Dev.JoshBrunton.DynamicsEndpointDiscovery.BlazorHybrid.Logging.Sinks;
using Dev.JoshBrunton.DynamicsEndpointDiscovery.BlazorHybrid.ViewModels;
using Microsoft.FluentUI.AspNetCore.Components;
using Serilog.Core;

namespace Dev.JoshBrunton.DynamicsEndpointDiscovery.BlazorHybrid.Ioc;

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
