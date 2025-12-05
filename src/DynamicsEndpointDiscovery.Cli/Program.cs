using DynamicsEndpointDiscovery.Cli.Commands;

await new DynSvcDiscoveryRootCommand().Parse(args).InvokeAsync();