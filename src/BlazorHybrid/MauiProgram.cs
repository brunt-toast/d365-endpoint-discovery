using BlazorHybrid.Ioc;
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

       BlazorHybridServiceRegistrar.RegisterServices(builder.Services);

#if DEBUG
		builder.Logging.AddDebug();
#endif

        return builder.Build();
    }
}
