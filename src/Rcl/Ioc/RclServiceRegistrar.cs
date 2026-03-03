using CommunityToolkit.Maui.Storage;
using CommunityToolkit.Mvvm.Messaging;
using Dev.JoshBrunton.DynamicsEndpointDiscovery.Application.Ioc;
using Dev.JoshBrunton.DynamicsEndpointDiscovery.Rcl.ViewModels;
using Microsoft.FluentUI.AspNetCore.Components;

namespace Dev.JoshBrunton.DynamicsEndpointDiscovery.Rcl.Ioc;

public static class RclServiceRegistrar
{
    public static void RegisterServices(IServiceCollection sc)
    {
        ApplicationServiceRegistrar.RegisterServices(sc);

        sc.AddFluentUIComponents();

        sc.AddSingleton<ILauncher>(_ => Launcher.Default);
        sc.AddSingleton<IFileSaver>(_ => FileSaver.Default);
        sc.AddSingleton<IFileSystem>(_ => FileSystem.Current);
        sc.AddSingleton<ISecureStorage>(_ => SecureStorage.Default);
        sc.AddSingleton<IMessenger, WeakReferenceMessenger>();

        sc.AddTransient<ICredentialsViewModel, CredentialsViewModel>();
        sc.AddTransient<ISelectGroupsViewModel, SelectGroupsViewModel>();
        sc.AddTransient<ISelectServicesViewModel, SelectServicesViewModel>();
        sc.AddTransient<ISelectOperationsViewModel, SelectOperationsViewModel>();
        sc.AddTransient<IBuildCollectionViewModel, BuildCollectionViewModel>();
    }
}
