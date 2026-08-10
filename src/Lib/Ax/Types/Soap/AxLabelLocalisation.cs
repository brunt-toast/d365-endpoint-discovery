namespace Dev.JoshBrunton.DynamicsEndpointDiscovery.Lib.Ax.Types.Soap;

public record AxLabelLocalisation
{
    public required string LabelId { get; init; }
    public required string Language { get; init; }
    public required string Value { get; init; }
}
