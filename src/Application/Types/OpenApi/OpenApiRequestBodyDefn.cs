using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Dev.JoshBrunton.DynamicsEndpointDiscovery.Application.Types.OpenApi;

public class OpenApiRequestBodyDefn
{
    [JsonProperty("description")] public string Description { get; set; } = string.Empty;
    [JsonProperty("required")] public bool IsRequired { get; set; }
    [JsonProperty("content")] public required JObject Content { get; set; }
}