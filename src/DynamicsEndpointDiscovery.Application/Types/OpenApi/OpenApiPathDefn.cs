using Newtonsoft.Json;

namespace DynamicsEndpointDiscovery.Application.Types.OpenApi;

public class OpenApiPathDefn
{
    [JsonProperty("post")] public required OpenApiPostRequestDefn Post { get; set; }
}