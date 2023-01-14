namespace ArkServer.Entities.Azure;

public record VNetInfo(
    string Name, 
    string? AddressPrefix = "", 
    IEnumerable<SubnetInfo>? SubnetsInfo = null
)
{
    internal int SpokeOctet2Offset = 0;
    public string Octet2{
        get
        {
            return AddressPrefix.Split(".")[1];
        }
    }

    public string Octet1{
        get
        {
            return AddressPrefix.Split(".")[0];
        }
    }
};

