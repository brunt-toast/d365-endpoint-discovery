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

internal class AxODataService
{
    private readonly AxCallingService _axCalling;
    private readonly ILogger _logger;
    private readonly IAxConfig _config;

    public AxODataService(AxCallingService axCalling, ILogger logger, IAxConfig config)
    {
        _axCalling = axCalling;
        _logger = logger;
        _config = config;
    }

    public async Task<IEnumerable<AxSchema>> GetSchemasFromMetadata()
    {
        var metadataXml = await _axCalling.GetHttp($"{_config.Resource}/data/$metadata");
        var doc = await XDocument.LoadAsync(metadataXml.ToStream(), LoadOptions.None, CancellationToken.None);
        return AxSchema.FromODataMetadata(doc);
    }
}
