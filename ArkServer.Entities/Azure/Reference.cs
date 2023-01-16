using System.Security.Cryptography;

namespace ArkServer.Entities.Azure;

public static partial class CidrGenerator
{
    /// <summary>
    /// Returns a list of subnets for creation for the hub
    /// </summary>
    /// <returns></returns>
    private static IEnumerable<SubnetInfo> GenerateHubSubnets()
    {
         return new List<SubnetInfo>
         {
            new SubnetInfo(
                Name:          "AzureFirewallSubnet",
                Description:   "Subnet for Azure Firewall",
                AddressPrefix: $"{HubPrefix}.0.0/26"
            ),
            new SubnetInfo(
                Name:          "AzureBastionSubnet",
                Description:   "Subnet for Bastion",
                AddressPrefix: $"{HubPrefix}.0.64/26"
            ),
            new SubnetInfo(
                Name:          "AzureFirewallManagementSubnet",
                Description:   "Subnet for VPN Gateway",
                AddressPrefix: $"{HubPrefix}.0.128/26"
            ),
            new SubnetInfo(
                Name:          "GatewaySubnet",
                Description:   "Subnet for VPN Gateway",
                AddressPrefix: $"{HubPrefix}.0.192/27"
            ),
            new SubnetInfo(
                Name:          "snet-test",
                Description:   "Subnet for Testing purposes",
                AddressPrefix: $"{HubPrefix}.0.224/27"
            ),
         };
    }

    private static IEnumerable<SubnetInfo> GenerateSpokeSubnets(int offset=0)
    {
        var prefix = $"{Octet1}.{SpokeOctet2 + offset}";
        return new List<SubnetInfo>
        {
            new SubnetInfo(
	            Name:          "snet-tier1-agw",
	            Description:   "Subnet for AGW",
	            AddressPrefix: $"{prefix}.1.0/24",
                Tags: new KeyValuePair<string,string>("ark:tier","1")   
            ),
            new SubnetInfo(
	            Name:          "snet-tier1-webin",
	            Description:   "Subnet for other LBs",
	            AddressPrefix: $"{prefix}.2.0/24",
                Tags: new KeyValuePair<string,string>("ark:tier","1")
            ),
            new SubnetInfo(
	            Name:          "snet-tier1-rsvd1",
	            Description:   "Tier 1 reserved subnet",
	            AddressPrefix: $"{prefix}.3.0/25",
                Tags: new KeyValuePair<string,string>("ark:tier","1")
            ),
            new SubnetInfo(
	            Name:          "snet-tier1-rsvd2",
	            Description:   "Tier 1 reserved subnet",
	            AddressPrefix: $"{prefix}.3.128/25",
                Tags: new KeyValuePair<string,string>("ark:tier","2")
            ),
            new SubnetInfo(
	            Name:          "snet-tier2-pckr",
	            Description:   "Subnet for packer",
	            AddressPrefix: $"{prefix}.7.0/24",
                Tags: new KeyValuePair<string,string>("ark:tier","2")
            ),
            new SubnetInfo(
	            Name:          "snet-tier2-vm",
	            Description:   "Subnet for VMs",
	            AddressPrefix: $"{prefix}.8.0/21",
                Tags: new KeyValuePair<string,string>("ark:tier","2")
            ),
            new SubnetInfo(
	            Name:          "snet-tier2-aks",
	            Description:   "Subnet for AKS",
	            AddressPrefix: $"{prefix}.16.0/20",
                Tags: new KeyValuePair<string,string>("ark:tier","2")
            ),
            new SubnetInfo(
	            Name:          "snet-tier3-mi",
	            Description:   "Subnet for managed instance",
	            AddressPrefix: $"{prefix}.32.0/26",
                Tags: new KeyValuePair<string,string>("ark:tier","3")
            ),
            new SubnetInfo(
	            Name:          "snet-tier3-dbaz",
	            Description:   "Subnet for SQL Azure",
	            AddressPrefix: $"{prefix}.32.64/26",
                Tags: new KeyValuePair<string,string>("ark:tier","3")
            ),
            new SubnetInfo(
	            Name:          "snet-tier3-dblb",
	            Description:   "Subnet for LB for SQL VM",
	            AddressPrefix: $"{prefix}.32.128/25",
                Tags: new KeyValuePair<string,string>("ark:tier","3")
            ),
            new SubnetInfo(
	            Name:          "snet-tier3-dbvm",
	            Description:   "Subnet for SQL VM",
	            AddressPrefix: $"{prefix}.33.0/25",
                Tags: new KeyValuePair<string,string>("ark:tier","3")
            ),
            new SubnetInfo(
	            Name:          "snet-tier3-strg",
	            Description:   "Subnet for storage account/fileshares",
	            AddressPrefix: $"{prefix}.33.128/25",
                Tags: new KeyValuePair<string,string>("ark:tier","3")
            ),
            new SubnetInfo(
	            Name:          "snet-tier3-redis",
	            Description:   "Subnet for redis cache",
	            AddressPrefix: $"{prefix}.34.0/25",
                Tags: new KeyValuePair<string,string>("ark:tier","3")
            ),
        };
    }

    /// <summary>
    /// Generates CIDRs for the hub based on octet preferences in the passed in cloudspace.
    /// </summary>
    /// <param name="acs">The cloudspace where environments need to be created</param>
    /// <returns></returns>
    /// 
    public static AzureCloudspace GenerateHub(AzureCloudspace acs)
    {
        // Use Octet definitions from ACS
        Octet1 = acs.Octet1;
        HubOctet2 = acs.HubOctet2;

        // Generate Hub prefix
        HubPrefix = $"{Octet1}.{HubOctet2}";

        // Generate Hub CIDRs
        Hub = new VNetSpec(
            Name: "vnet-hub",
            AddressPrefix: $"{HubPrefix}.0.0/24",
            SubnetsInfo: GenerateHubSubnets()
        );
        HubGenerated = true;

        acs.Hub = Hub;
        return acs;
    }

    /// <summary>
    /// Generates CIDRs for creating/adding Environments to the provided cloudspace.
    /// The provided cloudspace is scanned to ensure generated CIDRs do not clash with 
    /// the existing address spaces.
    /// </summary>
    /// <param name="Environments"></param>
    /// <param name="acs"></param>
    /// <returns></returns>
    /// <exception cref="ApplicationException"></exception>
    public static AzureCloudspace GenerateSpokes(HashSet<string> Environments, AzureCloudspace acs)
    {
        // Cannot generate spokes before Hub, return error
        if (!HubGenerated) throw new ApplicationException("Please generate hub first");

        // Initialize empty list of spokes.
        var spokes = new HashSet<VNetSpec>();

        // Get list of environments not already in the spoke
        var vnetsToAdd = Environments.Except(acs.Spokes.Select(x => x.Name));

        // Find max 2nd octet in the cloudspace
        var maxOctet = acs.Spokes.Select(x => int.Parse(x.AddressPrefix.Split(".")[1])).Max();

        foreach (var name in vnetsToAdd)
        {
            var vnet = new VNetSpec(
                Name: $"vnet={name}",
                AddressPrefix: $"{acs.Octet1}.{maxOctet+1}.0.0/16"
            );
            
            acs.Spokes.Add(vnet);
        }


        return acs;
    }

    public static IEnumerable<SubnetInfo> GenerateSpokeSubnets(int Octet1, int Octet2)
    {
        var prefix = $"{Octet1}.{Octet2}";
        return new List<SubnetInfo>
        {
            new SubnetInfo(
	            Name:          "snet-tier1-agw",
	            Description:   "Subnet for AGW",
	            AddressPrefix: $"{prefix}.1.0/24",
                Tags: new KeyValuePair<string,string>("ark:tier","1")   
            ),
            new SubnetInfo(
	            Name:          "snet-tier1-webin",
	            Description:   "Subnet for other LBs",
	            AddressPrefix: $"{prefix}.2.0/24",
                Tags: new KeyValuePair<string,string>("ark:tier","1")
            ),
            new SubnetInfo(
	            Name:          "snet-tier1-rsvd1",
	            Description:   "Tier 1 reserved subnet",
	            AddressPrefix: $"{prefix}.3.0/25",
                Tags: new KeyValuePair<string,string>("ark:tier","1")
            ),
            new SubnetInfo(
	            Name:          "snet-tier1-rsvd2",
	            Description:   "Tier 1 reserved subnet",
	            AddressPrefix: $"{prefix}.3.128/25",
                Tags: new KeyValuePair<string,string>("ark:tier","2")
            ),
            new SubnetInfo(
	            Name:          "snet-tier2-pckr",
	            Description:   "Subnet for packer",
	            AddressPrefix: $"{prefix}.7.0/24",
                Tags: new KeyValuePair<string,string>("ark:tier","2")
            ),
            new SubnetInfo(
	            Name:          "snet-tier2-vm",
	            Description:   "Subnet for VMs",
	            AddressPrefix: $"{prefix}.8.0/21",
                Tags: new KeyValuePair<string,string>("ark:tier","2")
            ),
            new SubnetInfo(
	            Name:          "snet-tier2-aks",
	            Description:   "Subnet for AKS",
	            AddressPrefix: $"{prefix}.16.0/20",
                Tags: new KeyValuePair<string,string>("ark:tier","2")
            ),
            new SubnetInfo(
	            Name:          "snet-tier3-mi",
	            Description:   "Subnet for managed instance",
	            AddressPrefix: $"{prefix}.32.0/26",
                Tags: new KeyValuePair<string,string>("ark:tier","3")
            ),
            new SubnetInfo(
	            Name:          "snet-tier3-dbaz",
	            Description:   "Subnet for SQL Azure",
	            AddressPrefix: $"{prefix}.32.64/26",
                Tags: new KeyValuePair<string,string>("ark:tier","3")
            ),
            new SubnetInfo(
	            Name:          "snet-tier3-dblb",
	            Description:   "Subnet for LB for SQL VM",
	            AddressPrefix: $"{prefix}.32.128/25",
                Tags: new KeyValuePair<string,string>("ark:tier","3")
            ),
            new SubnetInfo(
	            Name:          "snet-tier3-dbvm",
	            Description:   "Subnet for SQL VM",
	            AddressPrefix: $"{prefix}.33.0/25",
                Tags: new KeyValuePair<string,string>("ark:tier","3")
            ),
            new SubnetInfo(
	            Name:          "snet-tier3-strg",
	            Description:   "Subnet for storage account/fileshares",
	            AddressPrefix: $"{prefix}.33.128/25",
                Tags: new KeyValuePair<string,string>("ark:tier","3")
            ),
            new SubnetInfo(
	            Name:          "snet-tier3-redis",
	            Description:   "Subnet for redis cache",
	            AddressPrefix: $"{prefix}.34.0/25",
                Tags: new KeyValuePair<string,string>("ark:tier","3")
            ),
        };
    }


    public static AzureCloudspace AddSpoke(AzureCloudspace acs, string name)
    {
        int octet2;

        // Find max 2nd octet in the cloudspace
        if (acs.NoSpokes())
        {
            // If first spoke, get Octet2 from SpokeOctet2Start
            octet2 = acs.SpokeOctet2Start;
        } else
        {
            // Eles Get Max Octet2 from existing spokes
            octet2 = acs.Spokes.Select(x => int.Parse(x.AddressPrefix.Split(".")[1])).Max();

            // Add 1
            octet2 = octet2 + 1;
        }


        var newSpoke = new VNetSpec(
            Name: name, 
            AddressPrefix: $"{acs.Octet1}.{octet2}.0.0/16",
            SubnetsInfo: GenerateSpokeSubnets(acs.Octet1,octet2)
        );

        acs.Spokes.Add(newSpoke);
        return acs;
    }
}

