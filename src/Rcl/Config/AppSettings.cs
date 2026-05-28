using Mapster;
using Newtonsoft.Json;

namespace Dev.JoshBrunton.DynamicsEndpointDiscovery.Rcl.Config;

public class AppSettings : SettingsBase<AppSettings>
{
    public bool UseMock { get; set; }

    public AppSettings(string fileName) : base(fileName)
    {
    }

    public AppSettings()
    {
    }
}

public abstract class SettingsBase<TSelf>
    where TSelf : SettingsBase<TSelf>, new()
{
    private readonly string? _filePath;
    private bool _hasLoaded;

    protected SettingsBase(string filePath)
    {
        _filePath = filePath;
    }

    protected SettingsBase()
    {
    }

    public void Init()
    {
        if (!_hasLoaded)
        {
            Load();
        }
    }

    public void Save()
    {
        ArgumentNullException.ThrowIfNull(_filePath);
        File.WriteAllText(_filePath, JsonConvert.SerializeObject(this));
    }

    public void Load()
    {
        ArgumentNullException.ThrowIfNull(_filePath);
        
        string content;
        try
        {
            content = File.ReadAllText(_filePath);
        }
        catch (FileNotFoundException)
        {
            return;
        }

        TSelf loaded = JsonConvert.DeserializeObject<TSelf>(content)
                       ?? throw new InvalidOperationException($"The content of {_filePath} " +
                                                              $"could not be deserialised into our type");
        loaded.Adapt((TSelf)this);

        _hasLoaded = true;
    }
}
