using System.Text.Json;

namespace ArkServer.Entities.Azure;

/// <summary>
/// An Azure Cloudspace has one hub and one or more 'Environments' or VNETs
/// </summary>
public class AzureCloudspace
{

    //TODO: Add support for additional cloudspaces. Only 'default' supported for MVP
    public string Name { get; set; } = "default";

    /// <summary>
    /// One hub per cloudspace
    /// </summary>
    public VNetInfo? Hub { get;set;}

    /// <summary>
    /// One or more Environments or VNETs per cloudspace
    /// </summary>
    public HashSet<VNetInfo>? Spokes { get; set; }

    /// <summary>
    /// Creation status
    /// </summary>
    public string? Status {get;set;}

    // Default action is "POST" or create vs. delete
    // This is a hack - will change later
    public string Action {get;set;} = "post"; 


    // Defult Octets
    private static int DefaultOctet1 {get; } = 10;
    private static int DefaultOctet2 {get; } = 16;

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

    public AzureCloudspace(int Octet1 = 0, int Octet2=0)
    {
        this.Octet1 = Octet1 == 0 ? DefaultOctet1 : Octet1;
        this.HubOctet2 = Octet2 == 0 ? DefaultOctet2 : Octet2;
        this.SpokeOctet2Start = HubOctet2 +1;

        Hub = new VNetInfo("vnet-hub");
    }
}