using DynamicsEndpointDiscovery.Cli.Attributes;

namespace DynamicsEndpointDiscovery.Cli.Enums;

internal enum OutputFormats
{
    Default, 
    Postman_2_1_0,

    [NotImplemented]
    OpenApi_2_0_Json,

    [NotImplemented]
    OpenApi_2_0_Yaml,
    
    [NotImplemented]
    OpenApi_3_0_Json,
    
    [NotImplemented]
    OpenApi_3_0_Yaml,
    
    [NotImplemented]
    OpenApi_3_1_Json,
    
    [NotImplemented]
    OpenApi_3_1_Yaml,

    // aliases
    Postman = Postman_2_1_0,
    OpenApi = OpenApi_3_1_Json
}
