using System.Security.Cryptography.Xml;

namespace ArkServer.Entities.Azure;

public class ReferenceNetwork
{

    /// <summary>
    /// Hub is a reference Azure VNET used to model hub vnet deployments.
    /// </summary>
    public VNetInfo Hub;


    /// <summary>
    ///  We used to use 172.16 but it's to close to the Docker bridge CIDR and the spokes create an overlap. Hance switching to 10.x
    /// </summary>
    private readonly string HubPrefix = "10.16";
    private readonly string SpokePrefix = "10.x";
    private readonly static int spokeStart = 17;
    public ReferenceNetwork(string hubPrefix="10.0", string spokePrefix="10.x")
    {
        HubPrefix = hubPrefix;
        SpokePrefix = spokePrefix;

        Hub = new VNetInfo(
            Name: "vnet-hub",
            AddressPrefix: $"{hubPrefix}.0.0/24",
            SubnetsInfo: HubSubnets()
        );

        //Spoke = new Env(
        //    Name: "vnet-nprod",
        //    AddressPrefix: $"{spokePrefix}.0.0/16",
        //    SubnetsInfo: Spoke()
        //);
    }

    private IEnumerable<SubnetInfo> HubSubnets()
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
    ////VnetInfo Hub = new VnetInfo("hub","172.17.0.0/24",)


    public List<VNetInfo> Spokes(List<string> Environments)
    {
        var envs = new List<VNetInfo>();

        int offset=0;
        foreach(var name in Environments)
        {
            var env = new VNetInfo(
                Name: name,
                AddressPrefix: GenPrefix(offset),
                SubnetsInfo: SubnetsInfo(offset)
            );
            envs.Add(env);

            offset++;
        }
        return envs;
    }

    private static string GenPrefix(int offset)
    {
        return $"10.{spokeStart + offset}.0.0/16";
    }

    private IEnumerable<SubnetInfo> SubnetsInfo(int offset=0)
    {
        var prefix = $"10.{spokeStart + offset}";
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

}
