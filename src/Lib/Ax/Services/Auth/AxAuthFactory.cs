using Dev.JoshBrunton.DynamicsEndpointDiscovery.Lib.Ax.Config;
using Dev.JoshBrunton.DynamicsEndpointDiscovery.Lib.Ax.Enums;
using Dev.JoshBrunton.DynamicsEndpointDiscovery.Lib.Ax.Validators.AxCredentials;
using Microsoft.Extensions.DependencyInjection;
using Serilog;

namespace Dev.JoshBrunton.DynamicsEndpointDiscovery.Lib.Ax.Services.Auth;

internal class AxAuthFactory
{
    private readonly IAxConfig _axConfig;
    private readonly IServiceProvider _services;
    private readonly ILogger _logger;

    public AxAuthFactory(IAxConfig axConfig, IServiceProvider services, ILogger logger)
    {
        _axConfig = axConfig;
        _services = services;
        _logger = logger;
    }

    public IAxAuthService GetAuth()
    {
        return _axConfig.AuthKind switch
        {
            AuthKind.Application => _services.GetRequiredService<ApplicationAxAuthService>(),
            AuthKind.User => _services.GetRequiredService<UserAxAuthService>(),
            _ => Guess()
        };
    }

    private IAxAuthService Guess()
    {
        var appErrors = new ApplicationAxCredentialValidator().ValidateConfig(_axConfig).ToList();
        if (appErrors.Count == 0)
        {
            _logger.Information("Using {name}", nameof(ApplicationAxAuthService));
            return _services.GetRequiredService<ApplicationAxAuthService>();
        }

        var userErrors = new UserAxCredentialValidator().ValidateConfig(_axConfig).ToList();
        if (userErrors.Count == 0)
        {
            _logger.Information("Using {name}", nameof(UserAxAuthService));
            return _services.GetRequiredService<UserAxAuthService>();
        }

        _logger.Error("Tried to generate a best guess {interfaceName} based on {configName}, " +
                      "but no suitable implementation was found based on config.",
            nameof(IAxAuthService), nameof(IAxConfig));
        _logger.Error("Errors for application flow: \n{errors}", string.Join('\n', appErrors));
        _logger.Error("Errors for user flow: \n{errors}", string.Join('\n', userErrors));

        throw new InvalidOperationException($"Could not name a best-guess {nameof(IAxAuthService)} from current {nameof(IAxConfig)}.");
    }
}
