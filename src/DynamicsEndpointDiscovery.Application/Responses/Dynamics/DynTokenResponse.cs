using Newtonsoft.Json;

namespace DynamicsEndpointDiscovery.Application.Responses;

internal class DynTokenResponse
{
    [JsonProperty("token_type")] public string TokenType { get; set; } = string.Empty;
    [JsonProperty("expires_in")] public int ExpiresIn { get; set; }
    [JsonProperty("ext_expires_in")] public int ExtExpiresIn { get; set; }
    [JsonProperty("expires_on")] public int ExpiresOn { get; set; }
    [JsonProperty("not_before")] public int NotBefore { get; set; }
    [JsonProperty("resource")] public string Resource { get; set; } = string.Empty;
    [JsonProperty("access_token")] public string AccessToken { get; set; } = string.Empty;
}
