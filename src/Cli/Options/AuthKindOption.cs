using System.CommandLine;
using Dev.JoshBrunton.DynamicsEndpointDiscovery.Lib.Ax.Enums;

namespace Dev.JoshBrunton.DynamicsEndpointDiscovery.Cli.Options;

internal class AuthKindOption : Option<AuthKind>
{
    public AuthKindOption() : base("--auth")
    {
        Description = "If more than one auth flow is available using the given parameters, which one to use.";
        DefaultValueFactory = _ => AuthKind.BestGuess;
    }
}
