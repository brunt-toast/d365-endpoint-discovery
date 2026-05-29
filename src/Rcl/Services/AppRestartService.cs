namespace Dev.JoshBrunton.DynamicsEndpointDiscovery.Rcl.Services;

public interface IAppRestartService
{
    Task RestartAsync();
}

internal sealed class NoOpAppRestartService : IAppRestartService
{
    public Task RestartAsync()
    {
        return Task.CompletedTask;
    }
}
