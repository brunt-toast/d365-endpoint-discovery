using Newtonsoft.Json;

namespace DynamicsEndpointDiscovery.Application.Types.OpenApi_3_0;

public class OpenApiComponentDefn
{
    [JsonProperty("schemas")] public Dictionary<string, OpenApiSchemaDefn> Schemas { get; set; } = [];
}