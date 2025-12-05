using Newtonsoft.Json;

namespace DynamicsEndpointDiscovery.Application.Types.OpenApi_3_0;

public class OpenApiServerDefn
{
    [JsonProperty("url")] public string Uri { get; set; } = string.Empty;
}