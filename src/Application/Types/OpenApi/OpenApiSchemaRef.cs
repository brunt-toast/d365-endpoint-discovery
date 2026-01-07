using Newtonsoft.Json;

namespace Dev.JoshBrunton.DynamicsEndpointDiscovery.Application.Types.OpenApi;

public class OpenApiSchemaRef
{
    [JsonProperty("$ref")] public string Ref { get; set; } = string.Empty;
}