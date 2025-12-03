using Newtonsoft.Json;

namespace DynamicsEndpointDiscovery.Application.Types.Postman;

public class PostmanCollectionInfo
{
    [JsonProperty("__postman_id")] public required string PostmanId { get; init; }
    [JsonProperty("name")] public required string Name { get; init; }
    [JsonProperty("schema")] public required string Schema { get; init; }
    [JsonProperty("_exporter_id")] public required string ExporterId { get; init; }
}