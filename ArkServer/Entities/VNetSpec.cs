using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json;
using System.Text.Json.Serialization;
using YamlDotNet.Serialization;

namespace Ark.Server.Entities;

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
    [YamlMember(Alias = "name")]
    public required string Name { get; set; }

    [JsonPropertyName("addressprefix")]
    [YamlMember(Alias = "addressprefix")]
    public required string AddressPrefix { get; set; }

    [JsonPropertyName("subnetsInfo")]
    [YamlMember(Alias = "subnetsInfo")]
    public required IEnumerable<SubnetInfo> SubnetsInfo { get; set; }

    [JsonPropertyName("Octet2")]
    [YamlMember(Alias = "Octet2")]
    public string Octet2
    {
        get
        {
            return AddressPrefix.Split(".")[1];
        }
        init { }
    }

    [JsonPropertyName("Octet1")]
    [YamlMember(Alias = "Octet1")]
    public string Octet1
    {
        get
        {
            return AddressPrefix.Split(".")[0];
        }
        init { }
    }
}