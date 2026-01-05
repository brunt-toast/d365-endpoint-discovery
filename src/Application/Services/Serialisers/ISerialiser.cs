using System;
using System.Collections.Generic;
using System.Text;

namespace Dev.JoshBrunton.DynamicsEndpointDiscovery.Application.Services.Serialisers;

internal interface ISerialiser
{
    string Serialise(object data, bool doMinify);
}
