using Dev.JoshBrunton.DynamicsEndpointDiscovery.Core.Ioc;
using Dev.JoshBrunton.DynamicsEndpointDiscovery.Lib.Ax.Config;
using Dev.JoshBrunton.DynamicsEndpointDiscovery.Lib.Ax.Extensions.Microsoft.Extensions.DependencyInjection;
using Dev.JoshBrunton.DynamicsEndpointDiscovery.Lib.Ax.Services;
using Dev.JoshBrunton.DynamicsEndpointDiscovery.Lib.Ax.Services.Auth;
using Dev.JoshBrunton.DynamicsEndpointDiscovery.Lib.Ax.Services.Metadata;
using Dev.JoshBrunton.DynamicsEndpointDiscovery.Lib.Ax.Services.Soap;
using Dev.JoshBrunton.DynamicsEndpointDiscovery.Lib.Ax.Services.SvcDiscovery;
using Microsoft.Extensions.DependencyInjection;

namespace Dev.JoshBrunton.DynamicsEndpointDiscovery.Lib.Ax.Ioc;

public static class AxServiceRegistrar
{
    public static void RegisterServices(IServiceCollection sc, Func<IServiceProvider,bool> mockPredicate)
    {
        CoreServiceRegistrar.RegisterServices(sc);

        sc.AddSingleton<IAxConfig, AxConfig>(_ => new AxConfig
        {
            ClientId = string.Empty,
            ClientSecret = string.Empty,
            Resource = string.Empty,
            TokenRequestEndpoint = string.Empty,
            TenantId = string.Empty,
            AuthKind = default
        });
        sc.AddSingleton<AxCallingService>();
        
        sc.AddSingleton<ApplicationAxAuthService>();
        sc.AddSingleton<UserAxAuthService>();
        sc.AddSingleton<AxAuthFactory>();
        
        sc.AddSingleton<AxODataService>();
        sc.AddSingleton<AxMetadataLabelService>();
        sc.AddSingleton<IJsonConverterService, JsonConverterService>();

        sc.AddMockable<IAxSvcDiscoveryService, MockAxSvcDiscoveryService, AxSvcDiscoveryService>(ServiceLifetime.Singleton, mockPredicate);
        sc.AddMockable<IAxSoapService, MockAxSoapService, AxSoapService>(ServiceLifetime.Singleton, mockPredicate);
    }
}
