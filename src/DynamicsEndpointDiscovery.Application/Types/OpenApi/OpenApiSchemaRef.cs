using Newtonsoft.Json;

namespace DynamicsEndpointDiscovery.Application.Types.OpenApi;

public class OpenApiSchemaRef
{
    [JsonProperty("$ref")] public string Ref { get; set; } = string.Empty;
}