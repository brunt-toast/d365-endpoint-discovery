using Newtonsoft.Json;

namespace Dev.JoshBrunton.DynamicsEndpointDiscovery.Application.Types.OpenApi;

public class OpenApiSchemaDefn
{
    [JsonProperty("allOf")] public OpenApiParameterDefn[] Parameters { get; set; } = [];
}