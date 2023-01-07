using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace ArkServer.Entities.Azure;


public class Ark
{
    public List<AzureCloudspace> AzureCloudspace {get; set; } = new List<AzureCloudspace>();
}



public record VnetInfo(string Name, string AddressPrefix, IEnumerable<SubnetInfo> SubnetsInfo);

public record SubnetInfo(string Name, string AddressPrefix, string Description, KeyValuePair<string,string> Tags = new KeyValuePair<string,string>());

public record AzureCloudspace(string ProjectName, VnetInfo Hub, List<VnetInfo> Spokes);

