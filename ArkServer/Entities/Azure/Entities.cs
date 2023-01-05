using System.Runtime.CompilerServices;

namespace ArkServer.Entities.Azure;


public interface ICloudspace
{

}


public class VnetInfo
{
    public string? Name { get; set; }
    public string? AddressPrefix { get; set; }
    public SubnetInfo[]? SubnetsInfo { get; set; }
}

public class SubnetInfo
{
    public string? Name { get; set; }
    public string? AddressPrefix { get; set; }
    public string? Description { get; set; }
    public KeyValuePair<string, string>? Tags { get; set; }
}

public class AzureCloudspace : ICloudspace
{
    public int? Id { get; set; }
    public string? Name { get; set; }

    public VnetInfo? Hub { get; set; }
    public List<VnetInfo>? Spokes { get; set; }
}

