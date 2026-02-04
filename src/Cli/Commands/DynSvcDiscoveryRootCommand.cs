using System.CommandLine;

namespace Dev.JoshBrunton.DynamicsEndpointDiscovery.Cli.Commands;

internal class DynSvcDiscoveryRootCommand : RootCommand
{
    public DynSvcDiscoveryRootCommand(
        ServiceDiscoveryCommand svcDiscoveryCommand) : base("Discover Dynamics 365 service endpoints automatically.")
    {
        TreatUnmatchedTokensAsErrors = false;

        Add(svcDiscoveryCommand);

        SetAction(async arg =>
        {
            return await svcDiscoveryCommand.Parse(arg.Tokens.Select(t => t.Value).ToList()).InvokeAsync();
        });
    }
}
