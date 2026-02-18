using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Dev.JoshBrunton.DynamicsEndpointDiscovery.Application.Types.OpenApi;

public class OpenApiPostRequestDefn
{
    [JsonProperty("description")] public string Description { get; set; } = string.Empty;
    [JsonProperty("operationId")] public string OperationId { get; set; } = string.Empty;
    [JsonProperty("requestBody")] public required OpenApiRequestBodyDefn RequestBody { get; set; }
    [JsonProperty("responses")] public required Dictionary<int, OpenApiResponseDefn> Responses { get; set; }
}

public class OpenApiResponseDefn
{
    [JsonProperty("description")] public required string Description { get; set; }
    [JsonProperty("content")] public required JObject Content { get; set; }
}