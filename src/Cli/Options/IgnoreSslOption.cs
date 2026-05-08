using System.CommandLine;
using Dev.JoshBrunton.DynamicsEndpointDiscovery.Cli.Extensions.System.CommandLine;
using Microsoft.Extensions.Localization;

namespace Dev.JoshBrunton.DynamicsEndpointDiscovery.Cli.Options;

internal class IgnoreSslOption : Option<bool>
{
    public IgnoreSslOption(ResourceOption resourceOption,
        TokenRequestEndpointOption tokenRequestEndpointOption,
        IStringLocalizer<IgnoreSslOptionResources> localizer) 
        : base("--ignore-ssl")
    {
        Description = localizer[nameof(IgnoreSslOptionResources.Description),
            resourceOption.NameAndAliases(),
            tokenRequestEndpointOption.NameAndAliases()];
    }
}
