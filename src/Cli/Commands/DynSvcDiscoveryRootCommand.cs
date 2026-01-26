using System.CommandLine;

namespace Dev.JoshBrunton.DynamicsEndpointDiscovery.Cli.Commands;

internal class DynSvcDiscoveryRootCommand : RootCommand
{
    public DynSvcDiscoveryRootCommand(
        ServiceDiscoveryCommand svcDiscoveryCommand,
        ODataCommand oDataCommand) : base("Discover Dynamics 365 service endpoints automatically.")
    {
        TreatUnmatchedTokensAsErrors = false;

        Add(svcDiscoveryCommand);
        Add(oDataCommand);

        SetAction(async arg =>
        {
            return await svcDiscoveryCommand.Parse(arg.Tokens.Select(t => t.Value).ToList()).InvokeAsync();
        });
    }
}
