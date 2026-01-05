using System;
using System.Collections.Generic;
using System.Text;

namespace Dev.JoshBrunton.DynamicsEndpointDiscovery.Application.Services.Serialisers;

internal class StringSerialiser : ISerialiser
{
    public string Serialise(object data, bool doMinify)
    {
        return data.ToString() ?? string.Empty;
    }
}
