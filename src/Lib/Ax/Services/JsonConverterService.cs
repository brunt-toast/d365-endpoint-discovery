using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text;
using Dev.JoshBrunton.DynamicsEndpointDiscovery.Core.Extensions.Serilog;
using Newtonsoft.Json;
using Serilog;

namespace Dev.JoshBrunton.DynamicsEndpointDiscovery.Lib.Ax.Services;

internal class JsonConverterService : IJsonConverterService
{
    private readonly ILogger _logger;

    public JsonConverterService(ILogger logger)
    {
        _logger = logger;
    }


    public bool TryDeserialise<T>(string serialisation, [NotNullWhen(true)] out T? ret)
    {
        ret = default;

        try
        {
            ret = JsonConvert.DeserializeObject<T>(serialisation);
            return ret is not null;
        }
        catch (Exception ex)
        {
            _logger.LogError("An error occured when trying to deserialise data to type {typeFqn}: {errorMessage}. The raw data was: {serialisation}",
                typeof(T).FullName, ex.Message, serialisation);
            return false;
        }
    }
}

internal interface IJsonConverterService
{
    bool TryDeserialise<T>(string serialisation, [NotNullWhen(true)] out T? ret);
}