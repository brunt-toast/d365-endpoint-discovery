using System.CommandLine;

namespace Dev.JoshBrunton.DynamicsEndpointDiscovery.Cli.Options;

internal class IgnoreSslOption : Option<bool>
{
    public IgnoreSslOption() : base("--ignore-ssl")
    {
        Description = $"Don't validate the SSL certificate for {new ResourceOption().Name}. " +
                      $"The certificate for {new TokenRequestEndpointOption().Name} will always be validated.";
    }
}
