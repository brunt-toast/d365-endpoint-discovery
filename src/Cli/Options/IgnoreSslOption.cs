using System.CommandLine;
using Dev.JoshBrunton.DynamicsEndpointDiscovery.Cli.Extensions.System.CommandLine;

namespace Dev.JoshBrunton.DynamicsEndpointDiscovery.Cli.Options;

internal class IgnoreSslOption : Option<bool>
{
    public IgnoreSslOption(ResourceOption resourceOption,
        TokenRequestEndpointOption tokenRequestEndpointOption) 
        : base("--ignore-ssl")
    {
        Description = $"Don't validate the SSL certificate (if any) for {resourceOption.NameAndAliases()}. " +
                      $"The certificate for {tokenRequestEndpointOption.NameAndAliases()} will always be validated.";
    }
}
