using Newtonsoft.Json;

namespace DynamicsEndpointDiscovery.Application.Types.Postman;

public class PostmanBody
{
    [JsonProperty("mode")] public required string Mode { get; init; }
    [JsonProperty("raw")] public required string Raw { get; init; }
}