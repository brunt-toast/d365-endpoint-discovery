using Newtonsoft.Json;

namespace Dev.JoshBrunton.DynamicsEndpointDiscovery.Application.Types.Postman;

public class PostmanBody
{
    [JsonProperty("mode")] public required string Mode { get; init; }
    [JsonProperty("raw")] public required string Raw { get; init; }
    [JsonProperty("options")] public required PostmanBodyOptions Options { get; init; }
}

public class PostmanBodyOptions
{
    [JsonProperty("raw")] public required RawPostmanBodyOptions Raw { get; init; }
}

public class RawPostmanBodyOptions
{
    [JsonProperty("language")] public required string Language { get; init; }
}