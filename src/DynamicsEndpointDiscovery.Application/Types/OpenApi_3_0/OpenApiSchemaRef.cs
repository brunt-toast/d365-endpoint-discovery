using Newtonsoft.Json;

namespace DynamicsEndpointDiscovery.Application.Types.OpenApi_3_0;

public class OpenApiSchemaRef
{
    [JsonProperty("$ref")] public string Ref { get; set; } = string.Empty;
}