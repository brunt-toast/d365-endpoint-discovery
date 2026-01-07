using System;
using System.Collections.Generic;
using System.Text;
using YamlDotNet.Serialization;

namespace Dev.JoshBrunton.DynamicsEndpointDiscovery.Application.Services.Serialisers;

internal class YamlSerialiser : ISerialiser
{
    public string Serialise(object data, bool doMinify)
    {
        return new SerializerBuilder().ConfigureDefaultValuesHandling(DefaultValuesHandling.Preserve).Build().Serialize(data);
    }
}
