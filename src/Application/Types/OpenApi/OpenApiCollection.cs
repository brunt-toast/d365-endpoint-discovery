using Newtonsoft.Json;

namespace Dev.JoshBrunton.DynamicsEndpointDiscovery.Application.Types.OpenApi;

public class OpenApiCollection
{
    [JsonProperty("openapi")] public string ApiVersion { get; } = "3.0.0";
    [JsonProperty("info")] public required OpenApiInfo Info { get; set; }
    [JsonProperty("servers")] public OpenApiServerDefn[] Servers { get; set; } = [];
    [JsonProperty("paths")] public Dictionary<string, OpenApiPathDefn> Paths { get; set; } = [];
}