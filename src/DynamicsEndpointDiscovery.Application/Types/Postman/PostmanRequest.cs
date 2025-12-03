using Newtonsoft.Json;

namespace DynamicsEndpointDiscovery.Application.Types.Postman;

public class PostmanRequest
{
    [JsonProperty("method")] public required string Method { get; init; }
    [JsonProperty("header")] public required PostmanHeader[] Headers { get; init; }
    [JsonProperty("body")] public required PostmanBody Body { get; init; }
    [JsonProperty("url")] public required PostmanUrl Url { get; init; }
}