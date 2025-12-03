using System;
using System.Collections.Generic;
using System.CommandLine;
using System.Text;

namespace DynamicsEndpointDiscovery.Cli.Flags;

internal class Flag : Option<bool>
{
    public Flag(string name, params string[] aliases) : base(name, aliases)
    {
    }
}
