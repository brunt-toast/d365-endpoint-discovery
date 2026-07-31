namespace Dev.JoshBrunton.DynamicsEndpointDiscovery.Application.Services.CollectionBuilders.Options;

public class CSharpCollectionBuilderOptions : ICollectionBuilderOptions
{
    public void Validate()
    {
    }

    public bool IsOptionDisabled(string propertyName)
    {
        return false;
    }
}
