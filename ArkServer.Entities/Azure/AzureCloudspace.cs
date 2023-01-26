using System.Security.Cryptography;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace ArkServer.Entities.Azure;

/// <summary>
/// An Azure Cloudspace has one hub and one or more 'Environments' or VNETs
/// </summary>
public class AzureCloudspace
{
    [JsonPropertyName("id")]
    public string Id { get; set; }

    /// <summary>
    /// Name of cloudspace. Defaulting to 'default'
    /// </summary>
    [JsonPropertyName("name")]
    public string Name { get; set; } = "default";

    /// <summary>
    /// One hub per cloudspace
    /// </summary>
    [JsonPropertyName("hub")]
    public VNetSpec Hub { get;set;}

    /// <summary>
    /// One or more Environments or VNETs per cloudspace
    /// </summary>
    [JsonPropertyName("spokes")]
    public HashSet<VNetSpec> Spokes { get; set; } = new HashSet<VNetSpec>();

    /// <summary>
    /// Creation status
    /// </summary>
    [JsonPropertyName("status")]
    public string? Status {get;set;}

    // Defult Octets
    private static int DefaultOctet1 {get; } = 10;
    private static int DefaultOctet2 {get; } = 16;

    private static HashSet<int> AllOctet2 =  Enumerable.Range(17,200).ToHashSet<int>();

    /// <summary>
    /// Octet1 is common for Hub & Spokes. It is used to generate the CIDRs for
    /// all the hubs and spokes. This defaults to the value of DefaultOctet1 
    /// unless overridden via the constructor
    /// </summary>
    internal int Octet1 {get;init;}

    /// <summary>
    /// Used to generate the CIDRs for hub. This defaults to the value 
    /// of DefaultOctet2 unless overridden via the constructor
    /// </summary>
    internal int HubOctet2 {get;init;}

    /// <summary>
    /// SpokeOctet2Start is used to generate the CIDRs for the spokes. The second Octet of the first 
    /// spoke will be SpokeOctet2Start (Calculated from HubOctet2 + 1). Every subsequent spoke will 
    /// have a 2nd Octet >  SpokeOctet2Start
    /// </summary>
    internal int SpokeOctet2Start {get;init;}

    public override string ToString()
    {
        return JsonSerializer.Serialize(this, new JsonSerializerOptions{WriteIndented = true});
    }

    /// <summary>
    /// DO NOT REMOVE the JsonConstructor. Removing will cause issues with reading DB which 
    /// can be dangerous.
    /// </summary>
    [JsonConstructor]
    public AzureCloudspace() : this(0, 0) { }

    public AzureCloudspace(int octet1 = 0, int octet2=0)
    {
        Id = Guid.NewGuid().ToString();
        Octet1 = octet1 == 0 ? DefaultOctet1 : octet1;
        HubOctet2 = octet2 == 0 ? DefaultOctet2 : octet2;
        SpokeOctet2Start = HubOctet2 +1;

        Hub = new VNetSpec{
            Name= "vnet-hub", 
            AddressPrefix= $"{Octet1}.{HubOctet2}.0.0/24",
            SubnetsInfo= CidrGenerator.GenerateHubSubnets(Octet1, HubOctet2)
        };
    }

    public AzureCloudspace AddSpoke(string name)
    {
        // Return if spoke already exists
        if (Spokes.Any(s => s.Name == name)) return this;

        // Throw exception if number of spokes more than 30
        // Artificial limit
        if (Spokes.Count > 30)
            throw new ApplicationException("Can't have more than 30 Envs.");

        // Determine 2nd octet ti use for new spoke
        int octet2;
        if (NoSpokes())
        {
            // If this is the 1st spoke, then Octet2 is SpokeOctet2Start
            octet2 = SpokeOctet2Start;
        } 
        else
        {
            // Get list of used octets
            var usedOctet2= Spokes.Select(x => int.Parse(x.AddressPrefix.Split(".")[1]));

            // Remove used octets from list off all octets
            // Select the minimum octet from the list.           
            octet2 = AllOctet2.Except(usedOctet2).Min();

        }

        // Generate spoke with octet2
        var newSpoke = new VNetSpec{
            Name= name, 
            AddressPrefix= $"{Octet1}.{octet2}.0.0/16",
            SubnetsInfo= CidrGenerator.GenerateSpokeSubnets(Octet1,octet2)
        };

        Spokes.Add(newSpoke);
        return this;
    }

    public AzureCloudspace DelSpoke(string name)
    {
        if (NoSpokes())
        {
            return this;
        }

        Spokes.RemoveWhere(x => x.Name.ToLower() == name);
        return this;
    }
    public bool NoSpokes()
    {
        return Spokes.Count == 0;
    }
}