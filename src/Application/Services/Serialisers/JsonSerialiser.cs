using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dev.JoshBrunton.DynamicsEndpointDiscovery.Application.Services.Serialisers;

internal class JsonSerialiser : ISerialiser
{
    public string Serialise(object data, bool doMinify)
    {
        return JsonConvert.SerializeObject(data, doMinify ? Formatting.None : Formatting.Indented);
    }
}
