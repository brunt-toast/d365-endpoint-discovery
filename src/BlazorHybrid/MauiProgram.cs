using BlazorHybrid.ViewModels;
using CommunityToolkit.Maui;
using CommunityToolkit.Maui.Storage;
using Dev.JoshBrunton.DynamicsEndpointDiscovery.Application.Ioc;
using Microsoft.Extensions.Logging;
using Microsoft.FluentUI.AspNetCore.Components;

namespace BlazorHybrid;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder
            .UseMauiApp<App>()
            .UseMauiCommunityToolkit()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
            });

        builder.Services.AddFluentUIComponents();
        builder.Services.AddMauiBlazorWebView();

        builder.Services.AddSingleton<IFileSaver>(_ => FileSaver.Default);

        ApplicationServiceRegistrar.RegisterServices(builder.Services);
        builder.Services.AddTransient<IServiceDiscoveryViewModel, ServiceDiscoveryViewModel>();

#if DEBUG
		builder.Services.AddBlazorWebViewDeveloperTools();
		builder.Logging.AddDebug();
#endif

        return builder.Build();
    }
}
