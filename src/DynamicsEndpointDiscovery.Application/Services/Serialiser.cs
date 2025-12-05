using DynamicsEndpointDiscovery.Application.Enums;
using Newtonsoft.Json;
using YamlDotNet.Serialization;

namespace DynamicsEndpointDiscovery.Application.Services;

public static class Serialiser
{
    public static string Serialise(object data, OutputFormats format, bool minify)
    {
        return format switch
        {
            OutputFormats.Json => JsonConvert.SerializeObject(data, minify ? Formatting.None : Formatting.Indented),
            OutputFormats.Yaml => new SerializerBuilder().ConfigureDefaultValuesHandling(DefaultValuesHandling.Preserve).Build().Serialize(data),
            _ => data.ToString() ?? string.Empty
        };
    }
}
