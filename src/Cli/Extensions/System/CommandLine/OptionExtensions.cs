using System;
using System.Collections.Generic;
using System.CommandLine;
using System.Text;

namespace Dev.JoshBrunton.DynamicsEndpointDiscovery.Cli.Extensions.System.CommandLine;

internal static class OptionExtensions
{
    extension<T>(Option<T> source)
    {
        public string NameAndAliases()
        {
            return string.Join('|', [source.Name, ..source.Aliases]);
        }
    }
}
