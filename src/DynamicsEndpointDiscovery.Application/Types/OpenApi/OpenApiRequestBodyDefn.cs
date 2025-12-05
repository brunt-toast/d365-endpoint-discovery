using Newtonsoft.Json;

namespace DynamicsEndpointDiscovery.Application.Types.OpenApi;

public class OpenApiRequestBodyDefn
{
    [JsonProperty("description")] public string Description { get; set; } = string.Empty;
    [JsonProperty("required")] public bool IsRequired { get; set; }
    [JsonProperty("content")] public Dictionary<string, OpenApiSchema> ContentTypesToSchemaRefs { get; set; } = [];
}