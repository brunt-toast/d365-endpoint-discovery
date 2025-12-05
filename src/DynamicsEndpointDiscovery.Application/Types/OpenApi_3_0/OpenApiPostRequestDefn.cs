using Newtonsoft.Json;

namespace DynamicsEndpointDiscovery.Application.Types.OpenApi_3_0;

public class OpenApiPostRequestDefn
{
    [JsonProperty("description")] public string Description { get; set; } = string.Empty;
    [JsonProperty("operationId")] public string OperationId { get; set; } = string.Empty;
    [JsonProperty("requestBody")] public required OpenApiRequestBodyDefn RequestBody { get; set; }
}