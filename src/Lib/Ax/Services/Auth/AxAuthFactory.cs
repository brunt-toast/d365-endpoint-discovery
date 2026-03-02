using Dev.JoshBrunton.DynamicsEndpointDiscovery.Lib.Ax.Config;
using Microsoft.Extensions.DependencyInjection;

namespace Dev.JoshBrunton.DynamicsEndpointDiscovery.Lib.Ax.Services.Auth;

internal class AxAuthFactory
{
    private readonly IAxConfig _axConfig;
    private readonly IServiceProvider _services;

    public AxAuthFactory(IAxConfig axConfig, IServiceProvider services)
    {
        _axConfig = axConfig;
        _services = services;
    }

    public IAxAuthService GetAuth()
    {
        if (!string.IsNullOrWhiteSpace(_axConfig.ClientSecret))
        {
            return _services.GetRequiredService<ApplicationAxAuthService>();
        }

        if (!string.IsNullOrWhiteSpace(_axConfig.TenantId))
        {
            return _services.GetRequiredService<UserAxAuthService>();
        }

        throw new InvalidOperationException($"Either {nameof(_axConfig.ClientSecret)} (application flow) " +
                                            $"or {nameof(_axConfig.TenantId)} (user flow) " +
                                            $"is required to build an auth provider!");
    }
}
