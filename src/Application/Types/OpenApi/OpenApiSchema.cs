using Newtonsoft.Json;

namespace Dev.JoshBrunton.DynamicsEndpointDiscovery.Application.Types.OpenApi;

public class OpenApiSchema
{
    [JsonProperty("schema")] public required OpenApiSchemaRef Schema { get; set; }
}