using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using Dev.JoshBrunton.DynamicsEndpointDiscovery.Application.Config;
using Dev.JoshBrunton.DynamicsEndpointDiscovery.Core.Consts;
using Dev.JoshBrunton.DynamicsEndpointDiscovery.Core.Extensions.Serilog;
using Microsoft.Extensions.DependencyInjection;
using Serilog;

namespace Dev.JoshBrunton.DynamicsEndpointDiscovery.Application.Services;

internal class ConfigurableHttpClientFactory : IHttpClientFactory
{
    private readonly ILogger _logger;
    private readonly HttpClientOptions _opts;
    private readonly ServiceProvider _sp;

    public ConfigurableHttpClientFactory(HttpClientOptions opts, ILogger logger)
    {
        _opts = opts;
        _logger = logger;
        IServiceCollection sc = new ServiceCollection();
        sc.AddHttpClient();
        sc.AddHttpClient(HttpClientIdConsts.UserConfigurable)
            .ConfigurePrimaryHttpMessageHandler(sp =>
            {
                var handler = new SocketsHttpHandler();

                if (opts.MaxConnectionsPerServer > 0)
                {
                    handler.MaxConnectionsPerServer = opts.MaxConnectionsPerServer;
                }

                handler.SslOptions.RemoteCertificateValidationCallback = SslOptionsRemoteCertificateValidationCallback;

                return handler;
            });
        _sp = sc.BuildServiceProvider();
    }

    public HttpClient CreateClient(string name)
    {
        return _sp.GetRequiredService<IHttpClientFactory>().CreateClient(name);
    }

    private bool SslOptionsRemoteCertificateValidationCallback(object o, X509Certificate? x509Cert, X509Chain? x509Chain, SslPolicyErrors sslErrors)
    {
        if (_opts.AcceptAnySsl)
        {
            if (x509Cert is X509Certificate2 x509Cert_2)
            {
                _logger.LogInformation("Auto-accepting SSL certificate thumbprint {tp}", x509Cert_2.Thumbprint);
            }

            return true;
        }

        if (!string.IsNullOrWhiteSpace(_opts.AcceptableThumbprint) && x509Cert is X509Certificate2 x509Cert2)
        {
            if (x509Cert2.Thumbprint.Equals(_opts.AcceptableThumbprint, StringComparison.InvariantCultureIgnoreCase))
            {
                return true;
            }

            _logger.LogWarning("The server's X.509 certificate has thumbprint {serverThumbprint}, " +
                               "which does not match our expected thumbprint {ourThumbprint}. " +
                               "The certificate may still pass validation if it is trusted by the system.", 
                x509Cert2.Thumbprint, _opts.AcceptableThumbprint);
        }

        return sslErrors == SslPolicyErrors.None;
    }
}

