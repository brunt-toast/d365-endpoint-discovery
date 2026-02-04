using Dev.JoshBrunton.DynamicsEndpointDiscovery.Core.Ioc;
using Dev.JoshBrunton.DynamicsEndpointDiscovery.Lib.Ax.Config;
using Dev.JoshBrunton.DynamicsEndpointDiscovery.Lib.Ax.Services;
using Dev.JoshBrunton.DynamicsEndpointDiscovery.Lib.Ax.Services.Soap;
using Microsoft.Extensions.DependencyInjection;

namespace Dev.JoshBrunton.DynamicsEndpointDiscovery.Lib.Ax.Ioc;

public static class AxServiceRegistrar
{
    public static void RegisterServices(IServiceCollection sc)
    {
        CoreServiceRegistrar.RegisterServices(sc);

        sc.AddSingleton<IAxConfig, AxConfig>(_ => new AxConfig
        {
            ClientId = string.Empty,
            ClientSecret = string.Empty,
            Resource = string.Empty,
            TokenRequestEndpoint = string.Empty
        });
        sc.AddSingleton<AxCallingService>();
        sc.AddSingleton<AxAuthService>();
        sc.AddSingleton<AxODataService>();
        sc.AddSingleton<IAxSvcDiscoveryService, AxSvcDiscoveryService>();
        sc.AddSingleton<IJsonConverterService, JsonConverterService>();
        sc.AddSingleton<IAxSoapService, AxSoapService>();
    }
}
