using System.Net;
using System.Security.Authentication;
using Dev.JoshBrunton.DynamicsEndpointDiscovery.Core.Consts;
using Dev.JoshBrunton.DynamicsEndpointDiscovery.Core.Extensions.Serilog;
using Dev.JoshBrunton.DynamicsEndpointDiscovery.Lib.Ax.Config;
using Dev.JoshBrunton.DynamicsEndpointDiscovery.Lib.Ax.Services.Auth;
using Serilog;

namespace Dev.JoshBrunton.DynamicsEndpointDiscovery.Lib.Ax.Services;

internal class AxCallingService
{
    private readonly AxAuthFactory _authFactory;
    private readonly ILogger _logger;
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly IAxConfig _axConfig;

    public AxCallingService(AxAuthFactory authFactory, ILogger logger, IHttpClientFactory httpClientFactory, IAxConfig axConfig)
    {
        _authFactory = authFactory;
        _logger = logger;
        _httpClientFactory = httpClientFactory;
        _axConfig = axConfig;
    }

    public async Task<string> GetHttp(string endpoint)
    {
        var auth = _authFactory.GetAuth();
        string bearer = await auth.GetBearerToken();

        HttpRequestMessage request = new(HttpMethod.Get, endpoint);
        request.Headers.Clear();
        request.Headers.Add("Authorization", $"Bearer {bearer}");

        var client = _httpClientFactory.CreateClient(HttpClientIdConsts.UserConfigurable);
        client.BaseAddress = new Uri(_axConfig.Resource);

        HttpResponseMessage response;
        try
        {
            response = await client.SendAsync(request);
        }
        catch (HttpRequestException ex) when (ex.InnerException is AuthenticationException
                                              {
                                                  Message: "The remote certificate is invalid " +
                                                           "because of errors in the certificate chain: " +
                                                           "UntrustedRoot"
                                              })
        {
            _logger.Error("Couldn't connect to {resource} because the SSL certificate came from an untrusted root. " +
                          "Try skipping SSL validation or importing a certificate. " +
                          "Expect cascading errors from this failure.", endpoint);
            return string.Empty;
        }
        catch (HttpRequestException ex) when (ex.InnerException is AuthenticationException
                                              {
                                                  Message: "The remote certificate was rejected " +
                                                           "by the provided RemoteCertificateValidationCallback."
                                              })
        {
            _logger.Error("Couldn't connect to {resource} because the SSL certificate failed validation. " +
                          "Try skipping SSL validation or specifying an acceptable thumbprint. " +
                          "Expect cascading errors from this failure.", endpoint);
            return string.Empty;
        }

        string content = await response.Content.ReadAsStringAsync();

        if (response.StatusCode == HttpStatusCode.TooManyRequests)
        {
            _logger.LogError("Too many requests! ({endpoint})", endpoint);
        }
        else if (!response.IsSuccessStatusCode)
        {
            _logger.LogError("A request to {endpoint} returned HTTP status {statusInt} ({status}). Content was: {newLine}{content}", endpoint, (int)response.StatusCode, response.StatusCode, Environment.NewLine, content);
        }

        return content;
    }
}
