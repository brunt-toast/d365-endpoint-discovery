using Newtonsoft.Json;

namespace DynamicsEndpointDiscovery.Application.Types.OpenApi_3_0;

public class OpenApiSchemaDefn
{
    [JsonProperty("allOf")] public OpenApiParameterDefn[] Parameters { get; set; } = [];
}