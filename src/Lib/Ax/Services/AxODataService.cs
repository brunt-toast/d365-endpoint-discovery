using System.Diagnostics;
using System.Net;
using System.Xml.Linq;
using Dev.JoshBrunton.DynamicsEndpointDiscovery.Core.Extensions.Serilog;
using Dev.JoshBrunton.DynamicsEndpointDiscovery.Lib.Ax.Config;
using Dev.JoshBrunton.DynamicsEndpointDiscovery.Lib.Ax.Extensions.System;
using Dev.JoshBrunton.DynamicsEndpointDiscovery.Lib.Ax.Types.OData;
using Newtonsoft.Json;
using Serilog;

namespace Dev.JoshBrunton.DynamicsEndpointDiscovery.Lib.Ax.Services;

internal class AxODataService : IAxODataService
{
    private readonly AxAuthService _authSvc;
    private readonly ILogger _logger;
    private readonly IAxConfig _config;

    public AxODataService(AxAuthService authSvc, ILogger logger, IAxConfig config)
    {
        _authSvc = authSvc;
        _logger = logger;
        _config = config;
    }

    public async Task<string> GetRawMetadata()
    {
        var ret = await GetHttp($"{_config.Resource}/data/$metadata");
        return ret;
    }

    public async Task<IEnumerable<AxSchema>> GetSchemasFromMetadata()
    {
        var metadataXml = await GetRawMetadata();
        var doc = await XDocument.LoadAsync(metadataXml.ToStream(), LoadOptions.None, CancellationToken.None);
        return AxSchema.FromODataMetadata(doc);
    }

    private async Task<string> GetHttp(string endpoint)
    {
        string bearer = await _authSvc.GetBearerToken();
        using var client = new HttpClient(new HttpClientHandler
        {
            ServerCertificateCustomValidationCallback =
                HttpClientHandler.DangerousAcceptAnyServerCertificateValidator
        });

        HttpRequestMessage request = new(HttpMethod.Get, endpoint);
        request.Headers.Clear();
        request.Headers.Add("Authorization", $"Bearer {bearer}");
        var response = await client.SendAsync(request);

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

public interface IAxODataService
{
    Task<string> GetRawMetadata();
    Task<IEnumerable<AxSchema>> GetSchemasFromMetadata();
}