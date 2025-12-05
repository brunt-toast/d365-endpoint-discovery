using Newtonsoft.Json;

namespace DynamicsEndpointDiscovery.Application.Types.OpenApi_3_0;

public class OpenApiLicenseInfo
{
    [JsonProperty("name")] public string Name { get; set; } = string.Empty;
    [JsonProperty("url")] public string Url { get; set; } = string.Empty;
}