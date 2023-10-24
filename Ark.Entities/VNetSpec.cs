namespace Ark.Entities;


public class VNetSpec
{
    [JsonPropertyName("name")]
    [YamlMember(Alias = "Name")]
    public string Name { get; set; } = "";

    [JsonPropertyName("addressprefix")]
    [YamlMember(Alias = "AddressPrefix")]
    public string AddressPrefix { get; set; } = "";

    [JsonPropertyName("subnetsInfo")]
    [YamlMember(Alias = "SubnetsInfo")]
    public IEnumerable<SubnetInfo> SubnetsInfo { get; set; } = new List<SubnetInfo>();

    [JsonPropertyName("Octet2")]
    [YamlMember(Alias = "Octet2")]
    public string Octet2
    {
        get
        {
            return AddressPrefix.Split(".")[1];
        }
        private set { }
    }

    [JsonPropertyName("Octet1")]
    [YamlMember(Alias = "Octet1")]
    public string Octet1
    {
        get
        {
            return AddressPrefix.Split(".")[0];
        }
        private set { }
    }
}