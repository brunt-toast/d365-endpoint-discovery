using CommunityToolkit.Maui.Storage;
using Dev.JoshBrunton.DynamicsEndpointDiscovery.Rcl.Ioc;
using Microsoft.AspNetCore.Components;
using Microsoft.Maui.ApplicationModel;
using Microsoft.Maui.Storage;

namespace Dev.JoshBrunton.DynamicsEndpointDiscovery.Tests.Rcl.Tests.Tests.Ioc;

[TestClass]
public class RclServiceRegistrarTests
{
    public static IEnumerable<object[]> GetServices()
    {
        var sc = new ServiceCollection();
        RclServiceRegistrar.RegisterServices(sc);
        foreach (var defn in sc)
        {
            if (defn.ServiceType.FullName?.StartsWith("Dev.JoshBrunton.DynamicsEndpointDiscovery") == true)
            {
                if (!defn.IsKeyedService)
                {
                    yield return [defn];
                }
            }
        }
    }

    [TestMethod]
    [DynamicData(nameof(GetServices))]
    public void GetRequiredService_ShouldNotThrow(ServiceDescriptor sd)
    {
        var sc = new ServiceCollection();
        RclServiceRegistrar.RegisterServices(sc);
        RegisterTestPlatformServices(sc);
        var sp = sc.BuildServiceProvider();
        sp.GetRequiredService(sd.ServiceType);
    }

    public static IEnumerable<object[]> GetAllComponentTypes()
    {
        return typeof(RclServiceRegistrar)
            .Assembly
            .GetTypes()
            .Where(t =>
                t is { IsAbstract: false, IsInterface: false, ContainsGenericParameters: false } &&
                typeof(ComponentBase).IsAssignableFrom(t))
            .Select(x => new object[] { x });
    }

    [TestMethod]
    [DynamicData(nameof(GetAllComponentTypes))]
    public void ComponentGeneration_ShouldNotThrow(Type componentType)
    {
        IServiceCollection sc = new ServiceCollection();
        RclServiceRegistrar.RegisterServices(sc);
        RegisterTestPlatformServices(sc);
        foreach (var t in GetAllComponentTypes())
        {
            sc.AddTransient((Type)t[0]);
        }
        IServiceProvider sp = sc.BuildServiceProvider();

        sp.GetRequiredService(componentType);
    }

    private static void RegisterTestPlatformServices(IServiceCollection sc)
    {
        sc.AddSingleton<ILauncher, TestLauncher>();
        sc.AddSingleton<IFileSaver, TestFileSaver>();
        sc.AddSingleton<IFilePicker, TestFilePicker>();
        sc.AddSingleton<IFileSystem, TestFileSystem>();
        sc.AddSingleton<ISecureStorage, TestSecureStorage>();
    }

    private sealed class TestLauncher : ILauncher
    {
        public Task<bool> CanOpenAsync(Uri uri)
        {
            throw new NotImplementedException();
        }

        public Task<bool> OpenAsync(Uri uri)
        {
            throw new NotImplementedException();
        }

        public Task<bool> OpenAsync(OpenFileRequest request)
        {
            throw new NotImplementedException();
        }

        public Task<bool> TryOpenAsync(Uri uri)
        {
            throw new NotImplementedException();
        }
    }

    private sealed class TestFileSaver : IFileSaver
    {
        public Task<FileSaverResult> SaveAsync(string initialPath, string fileName, Stream stream, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<FileSaverResult> SaveAsync(string fileName, Stream stream, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<FileSaverResult> SaveAsync(string initialPath, string fileName, Stream stream, IProgress<double> progress, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<FileSaverResult> SaveAsync(string fileName, Stream stream, IProgress<double> progress, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }
    }

    private sealed class TestFilePicker : IFilePicker
    {
        public Task<FileResult?> PickAsync(PickOptions? options = null)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<FileResult>> PickMultipleAsync(PickOptions? options = null)
        {
            throw new NotImplementedException();
        }
    }

    private sealed class TestFileSystem : IFileSystem
    {
        public string CacheDirectory => throw new NotImplementedException();
        public string AppDataDirectory => throw new NotImplementedException();

        public Task<bool> AppPackageFileExistsAsync(string filename)
        {
            throw new NotImplementedException();
        }

        public Task<Stream> OpenAppPackageFileAsync(string filename)
        {
            throw new NotImplementedException();
        }
    }

    private sealed class TestSecureStorage : ISecureStorage
    {
        public Task<string?> GetAsync(string key)
        {
            throw new NotImplementedException();
        }

        public Task SetAsync(string key, string value)
        {
            throw new NotImplementedException();
        }

        public bool Remove(string key)
        {
            throw new NotImplementedException();
        }

        public void RemoveAll()
        {
            throw new NotImplementedException();
        }
    }
}
