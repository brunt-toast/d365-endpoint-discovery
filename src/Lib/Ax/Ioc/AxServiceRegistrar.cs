using Dev.JoshBrunton.DynamicsEndpointDiscovery.Lib.Ax.Config;
using Dev.JoshBrunton.DynamicsEndpointDiscovery.Lib.Ax.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Dev.JoshBrunton.DynamicsEndpointDiscovery.Lib.Ax.Ioc;

public static class AxServiceRegistrar
{
    public static void RegisterServices(IServiceCollection sc)
    {
        sc.AddSingleton<IAxConfig, AxConfig>(_ => new AxConfig
        {
            ClientId = string.Empty,
            ClientSecret = string.Empty,
            Resource = string.Empty,
            TokenRequestEndpoint = string.Empty
        });
        sc.AddSingleton<AxAuthService>();
        sc.AddSingleton<IAxScvDiscoveryService, AxSvcDiscoveryService>();
    }
}
