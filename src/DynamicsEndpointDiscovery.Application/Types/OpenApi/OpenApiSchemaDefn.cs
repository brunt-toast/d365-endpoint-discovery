using Newtonsoft.Json;

namespace DynamicsEndpointDiscovery.Application.Types.OpenApi;

public class OpenApiSchemaDefn
{
    [JsonProperty("allOf")] public OpenApiParameterDefn[] Parameters { get; set; } = [];
}