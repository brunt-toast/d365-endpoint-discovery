using Newtonsoft.Json;

namespace DynamicsEndpointDiscovery.Application.Types.OpenApi_3_0;

public class OpenApiSchema
{
    [JsonProperty("schema")] public required OpenApiSchemaRef Schema { get; set; }
}