using Newtonsoft.Json;

namespace DynamicsEndpointDiscovery.Application.Types.OpenApi_3_0;

public class OpenApiInfo
{
    [JsonProperty("version")] public string Version { get; set; } = string.Empty;
    [JsonProperty("title")] public string Title { get; set; } = string.Empty;
    [JsonProperty("description")] public string Description { get; set; } = string.Empty;
    [JsonProperty("termsOfService")] public string TermsOfService { get; set; } = string.Empty;
    [JsonProperty("contact")] public required OpenApiContactInfo Contact { get; set; }
    [JsonProperty("license")] public required OpenApiLicenseInfo License { get; set; }
}