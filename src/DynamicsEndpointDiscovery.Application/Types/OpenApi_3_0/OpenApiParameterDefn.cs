using Newtonsoft.Json;

namespace DynamicsEndpointDiscovery.Application.Types.OpenApi_3_0;

public class OpenApiParameterDefn
{
    [JsonProperty("type")] public string Type { get; set; } = "object";
    [JsonProperty("required")] public string[] Required { get; set; } = [];
    [JsonProperty("properties")] public Dictionary<string, OpenApiTypeDefn> Properties { get; set; } = [];
}