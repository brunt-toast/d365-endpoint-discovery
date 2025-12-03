using Newtonsoft.Json;

namespace DynamicsEndpointDiscovery.Application.Types.Postman;

public class PostmanCollection
{
    [JsonProperty("info")] public required PostmanCollectionInfo Info { get; init; }
    [JsonProperty("item")] public required PostmanItem[] Items { get; init; }
}