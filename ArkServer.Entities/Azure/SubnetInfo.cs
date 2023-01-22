using System.Text.Json.Serialization;

namespace ArkServer.Entities.Azure;

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
    public KeyValuePair<string, string> Tags = new();
}