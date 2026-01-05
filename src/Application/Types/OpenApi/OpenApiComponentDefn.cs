using Newtonsoft.Json;

namespace Dev.JoshBrunton.DynamicsEndpointDiscovery.Application.Types.OpenApi;

public class OpenApiComponentDefn
{
    [JsonProperty("schemas")] public Dictionary<string, OpenApiSchemaDefn> Schemas { get; set; } = [];
}