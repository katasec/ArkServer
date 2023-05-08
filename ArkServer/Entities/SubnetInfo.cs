using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using YamlDotNet.Serialization;

namespace Ark.Server.Entities;

//public record SubnetInfo(
//    string Name, string AddressPrefix, string Description,
//    KeyValuePair<string, string> Tags = new KeyValuePair<string, string>()
//);

public class SubnetInfo
{
    [JsonPropertyName("name")]
    [YamlMember(Alias = "name")]
    public required string Name { get; set; }

    [JsonPropertyName("addressprefix")]
    [YamlMember(Alias = "addressprefix")]
    public required string AddressPrefix { get; set; }

    [JsonPropertyName("description")]
    [YamlMember(Alias = "description")]
    public required string Description { get; set; }

    [JsonPropertyName("tags")]
    [YamlMember(Alias = "tags")]
    public List<KeyValuePair<string, string>> Tags = new();

    public void stuff()
    {

    }
}