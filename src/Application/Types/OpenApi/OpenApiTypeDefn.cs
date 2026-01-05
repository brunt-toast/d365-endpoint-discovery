using Newtonsoft.Json;

namespace Dev.JoshBrunton.DynamicsEndpointDiscovery.Application.Types.OpenApi;

public class OpenApiTypeDefn
{
    [JsonProperty("type")] public string Type { get; set; } = string.Empty;
}