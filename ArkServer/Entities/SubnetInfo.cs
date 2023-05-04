using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace ArkServer.Entities;

//public record SubnetInfo(
//    string Name, string AddressPrefix, string Description,
//    KeyValuePair<string, string> Tags = new KeyValuePair<string, string>()
//);

public class SubnetInfo
{
    [JsonPropertyName("name")]
    public required string Name { get; set; }

    [JsonPropertyName("addressprefix")]
    public required string AddressPrefix { get; set; }

    [JsonPropertyName("description")]
    public required string Description { get; set; }

    [JsonPropertyName("tags")]
    public List<KeyValuePair<string, string>> Tags = new();

    public void stuff()
    {

    }
}