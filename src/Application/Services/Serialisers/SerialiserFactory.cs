using Dev.JoshBrunton.DynamicsEndpointDiscovery.Application.Enums;

namespace Dev.JoshBrunton.DynamicsEndpointDiscovery.Application.Services.Serialisers;

internal class SerialiserFactory
{
    public ISerialiser GetSerialiser(OutputFormats format)
    {
        return format switch
        {
            OutputFormats.Json => new JsonSerialiser(),
            OutputFormats.Yaml => new YamlSerialiser(),
            _ => new StringSerialiser()
        };
    }
}
