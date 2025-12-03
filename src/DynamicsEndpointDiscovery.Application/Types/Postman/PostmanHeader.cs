using Newtonsoft.Json;

namespace DynamicsEndpointDiscovery.Application.Types.Postman;

public class PostmanHeader
{
    [JsonProperty("key")] public required string Key { get; init; }
    [JsonProperty("value")] public required string Value { get; init; }
    [JsonProperty("type")] public required string Type { get; init; }
}