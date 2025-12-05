using Newtonsoft.Json;

namespace DynamicsEndpointDiscovery.Application.Types.OpenApi_3_0;

public class OpenApiTypeDefn
{
    [JsonProperty("type")] public string Type { get; set; } = string.Empty;
}