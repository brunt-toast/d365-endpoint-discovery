using CommunityToolkit.Maui;
using Dev.JoshBrunton.DynamicsEndpointDiscovery.BlazorHybrid.Ioc;

namespace Dev.JoshBrunton.DynamicsEndpointDiscovery.BlazorHybrid;

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
