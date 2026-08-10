namespace Dev.JoshBrunton.DynamicsEndpointDiscovery.Lib.Ax.Types.Soap;

public class SoapTypeCollection
{
    public required Dictionary<string, string> Samples { get; init; }
    public required IReadOnlyCollection<AxDataContractDefn> Definitions { get; init; }
    public IReadOnlyCollection<AxLabelLocalisation> Localisations { get; init; } = [];
}
