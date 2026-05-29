using System.CommandLine;
using Dev.JoshBrunton.DynamicsEndpointDiscovery.Lib.Ax.Enums;

namespace Dev.JoshBrunton.DynamicsEndpointDiscovery.Cli.Options;

internal class AuthKindOption : Option<AuthKind>
{
    public AuthKindOption() : base("--auth")
    {
        Description = AuthKindOptionResources.Description;
        DefaultValueFactory = _ => AuthKind.BestGuess;
    }
}
