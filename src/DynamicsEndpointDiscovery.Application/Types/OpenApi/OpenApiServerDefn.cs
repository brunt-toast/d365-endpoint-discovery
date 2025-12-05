using Newtonsoft.Json;

namespace DynamicsEndpointDiscovery.Application.Types.OpenApi;

public class OpenApiServerDefn
{
    [JsonProperty("url")] public string Uri { get; set; } = string.Empty;
}