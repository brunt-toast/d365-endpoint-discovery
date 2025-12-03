using Newtonsoft.Json;

namespace DynamicsEndpointDiscovery.Application.Types.Postman;

public class PostmanItem
{
    [JsonProperty("name")] public required string Name { get; init; } = string.Empty;
    [JsonProperty("item")] public required PostmanItem[]? Items { get; init; }
    [JsonProperty("request")] public required PostmanRequest? Request { get; init; }
    [JsonProperty("response")] public required object[]? Response { get; init; }
}