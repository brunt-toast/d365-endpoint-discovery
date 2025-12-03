using Newtonsoft.Json;

namespace DynamicsEndpointDiscovery.Application.Types.Postman;

public class PostmanUrl
{
    [JsonProperty("raw")] public required string Raw { get; init; }
    [JsonProperty("host")] public required string[] Host { get; init; }
    [JsonProperty("path")] public required string[] Path { get; init; }
}