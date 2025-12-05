using DynamicsEndpointDiscovery.Application.Enums;
using Newtonsoft.Json;
using YamlDotNet.Serialization;

namespace DynamicsEndpointDiscovery.Application.Services;

public static class Serialiser
{
    public static string Serialise(object data, OutputFormats format)
    {
        return format switch
        {
            OutputFormats.Json => JsonConvert.SerializeObject(data, Formatting.Indented),
            OutputFormats.Yaml => new Serializer().Serialize(data),
            _ => data.ToString() ?? string.Empty
        };
    }
}
