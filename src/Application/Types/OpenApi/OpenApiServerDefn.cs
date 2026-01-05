using Newtonsoft.Json;

namespace Dev.JoshBrunton.DynamicsEndpointDiscovery.Application.Types.OpenApi;

public class OpenApiServerDefn
{
    [JsonProperty("url")] public string Uri { get; set; } = string.Empty;
}