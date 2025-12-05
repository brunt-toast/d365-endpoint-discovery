using Newtonsoft.Json;

namespace DynamicsEndpointDiscovery.Application.Types.OpenApi;

public class OpenApiComponentDefn
{
    [JsonProperty("schemas")] public Dictionary<string, OpenApiSchemaDefn> Schemas { get; set; } = [];
}