using Dev.JoshBrunton.DynamicsEndpointDiscovery.Application.Services.CollectionBuilders.Options;
using Dev.JoshBrunton.DynamicsEndpointDiscovery.Application.Services.Serialisers;
using Dev.JoshBrunton.DynamicsEndpointDiscovery.Lib.Ax.Types;
using Dev.JoshBrunton.DynamicsEndpointDiscovery.Lib.Ax.Types.Soap;

namespace Dev.JoshBrunton.DynamicsEndpointDiscovery.Application.Services.CollectionBuilders;

public abstract class SerialisedCollectionBuilderBase<TOptions> : CollectionBuilderBase<TOptions>
    where TOptions : SerialisedCollectionBuilderOptions
{
    private readonly SerialiserFactory _serialiserFactory;

    protected SerialisedCollectionBuilderBase(SerialiserFactory serialiserFactory)
    {
        _serialiserFactory = serialiserFactory;
    }

    public override string BuildCollection(
        IEnumerable<DynSvcGroup> groups,
        SoapTypeCollection types,
        string resource,
        string collectionName,
        TOptions options)
    {
        var collection = BuildSerializableCollection(groups, types, resource, collectionName, options);
        var serialiser = _serialiserFactory.GetSerialiser(options.OutputFormat);
        return serialiser.Serialise(collection, options.Minify);
    }

    protected abstract object BuildSerializableCollection(
        IEnumerable<DynSvcGroup> groups,
        SoapTypeCollection types,
        string resource,
        string collectionName,
        TOptions options);
}
