using Newtonsoft.Json;

namespace DynamicsEndpointDiscovery.Application.Types.OpenApi;

public class OpenApiTypeDefn
{
    [JsonProperty("type")] public string Type { get; set; } = string.Empty;
}