namespace Dev.JoshBrunton.DynamicsEndpointDiscovery.Application.Services.CollectionBuilders.Options;

public interface ICollectionBuilderOptions
{
    void Validate();
    bool IsOptionDisabled(string propertyName);
}
