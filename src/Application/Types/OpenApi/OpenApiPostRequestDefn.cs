using Newtonsoft.Json;

namespace Dev.JoshBrunton.DynamicsEndpointDiscovery.Application.Types.OpenApi;

public class OpenApiPostRequestDefn
{
    [JsonProperty("description")] public string Description { get; set; } = string.Empty;
    [JsonProperty("operationId")] public string OperationId { get; set; } = string.Empty;
    [JsonProperty("requestBody")] public required OpenApiRequestBodyDefn RequestBody { get; set; }
}