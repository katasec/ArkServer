namespace Ark.Entities;


public class SubnetInfo
{
    [JsonPropertyName("name")]
    [YamlMember(Alias = "name")]
    public string Name { get; set; } = "";

    [JsonPropertyName("addressprefix")]
    [YamlMember(Alias = "addressprefix")]
    public string AddressPrefix { get; set; } = "";

    [JsonPropertyName("description")]
    [YamlMember(Alias = "description")]
    public string Description { get; set; } = "";

    [JsonPropertyName("tags")]
    [YamlMember(Alias = "tags")]
    public List<KeyValuePair<string, string>> Tags = new();

    public void stuff()
    {

    }
}