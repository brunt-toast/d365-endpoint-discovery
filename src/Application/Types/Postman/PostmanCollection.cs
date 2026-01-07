using Newtonsoft.Json;

namespace Dev.JoshBrunton.DynamicsEndpointDiscovery.Application.Types.Postman;

public class PostmanCollection
{
    [JsonProperty("info")] public required PostmanCollectionInfo Info { get; init; }
    [JsonProperty("item")] public required PostmanItem[] Items { get; init; }
}