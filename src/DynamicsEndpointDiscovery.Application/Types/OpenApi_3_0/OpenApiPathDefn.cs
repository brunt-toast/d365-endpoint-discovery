using Newtonsoft.Json;

namespace DynamicsEndpointDiscovery.Application.Types.OpenApi_3_0;

public class OpenApiPathDefn
{
    [JsonProperty("post")] public required OpenApiPostRequestDefn Post { get; set; }
}