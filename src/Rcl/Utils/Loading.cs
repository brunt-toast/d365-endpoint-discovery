namespace Dev.JoshBrunton.DynamicsEndpointDiscovery.Rcl.Utils;

public interface ILoading
{
    bool IsLoading { get; set; }

    public static IAsyncDisposable UseLoadingAsync(ILoading loading)
    {
        return new LoadingDisposable(loading);
    }

    public static IDisposable UseLoading(ILoading loading)
    {
        return new LoadingDisposable(loading);
    }

    private class LoadingDisposable : IDisposable, IAsyncDisposable
    {
        private readonly ILoading _loading;

        public LoadingDisposable(ILoading loading)
        {
            _loading = loading;
            _loading.IsLoading = true;
        }

        public ValueTask DisposeAsync()
        {
            Dispose();
            return ValueTask.CompletedTask;
        }

        public void Dispose()
        {
            _loading.IsLoading = false;
        }
    }
}