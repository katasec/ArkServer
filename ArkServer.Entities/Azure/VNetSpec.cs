using System.Text.Json;
using System.Text.Json.Serialization;

namespace ArkServer.Entities.Azure;

//public record VNetSpec(string Name,  string AddressPrefix, IEnumerable<SubnetInfo> SubnetsInfo)
//{
//    internal int SpokeOctet2Offset = 0;
//    public string Octet2{
//        get
//        {
//            return AddressPrefix.Split(".")[1];
//        }
//    }

//    public string Octet1{
//        get
//        {
//            return AddressPrefix.Split(".")[0];
//        }
//    }
//};



public class VNetSpec
{
    [JsonPropertyName("name")]
    public required string Name {get;set;}

    [JsonPropertyName("addressprefix")]
    public required string AddressPrefix {get;set;}

    [JsonPropertyName("subnetsInfo")]
    public required IEnumerable<SubnetInfo> SubnetsInfo {get;set;}

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
}