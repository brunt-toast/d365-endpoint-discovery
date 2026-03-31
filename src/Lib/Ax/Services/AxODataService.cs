using System.Xml.Linq;
using Dev.JoshBrunton.DynamicsEndpointDiscovery.Lib.Ax.Extensions.System;
using Dev.JoshBrunton.DynamicsEndpointDiscovery.Lib.Ax.Types.OData;

namespace Dev.JoshBrunton.DynamicsEndpointDiscovery.Lib.Ax.Services;

internal class AxODataService
{
    private readonly AxCallingService _axCalling;

    public AxODataService(AxCallingService axCalling)
    {
        _axCalling = axCalling;
    }

    public async Task<IEnumerable<AxSchema>> GetSchemasFromMetadata()
    {
        var metadataXml = await _axCalling.GetHttp("/data/$metadata");
        var doc = await XDocument.LoadAsync(metadataXml.ToStream(), LoadOptions.None, CancellationToken.None);
        return AxSchema.FromODataMetadata(doc);
    }
}
