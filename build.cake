var target = Argument("target", "Run");
var configuration = Argument("configuration", "Release");

Task("Run").Does(() =>
{
    DotNetRun("./src/DynamicsEndpointDiscovery.Cli/DynamicsEndpointDiscovery.Cli.csproj", new DotNetRunSettings
    {
        Configuration = configuration,
    });
});

RunTarget(target);