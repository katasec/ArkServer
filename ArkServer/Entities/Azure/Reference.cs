namespace ArkServer.Entities.Azure;

public class ReferenceNetwork
{

    /// <summary>
    /// Hub is a reference Azure VNET used to model hub vnet deployments.
    /// </summary>
    public Env Hub;

    /// <summary>
    /// Hub is a reference Azure VNET used to model spokes vnet deployments.
    /// </summary>
    public Env Spoke;


    /// <summary>
    ///  We used to use 172.16 but it's to close to the Docker bridge CIDR and the spokes create an overlap. Hance switching to 10.x
    /// </summary>
    private readonly string HubPrefix = "10.16";
    private readonly string SpokePrefix = "10.x";

    public ReferenceNetwork(string hubPrefix="10.0", string spokePrefix="10.x")
    {
        HubPrefix = hubPrefix;
        SpokePrefix = spokePrefix;

        Hub = new Env(
            Name: "vnet-hub",
            AddressPrefix: $"{hubPrefix}.0.0/24",
            SubnetsInfo: HubSubnets()
        );

        Spoke = new Env(
            Name: "vnet-nprod",
            AddressPrefix: $"{spokePrefix}.0.0/16",
            SubnetsInfo: SpokeSubnets()
        );
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

    private IEnumerable<SubnetInfo> SpokeSubnets()
    {
         return new List<SubnetInfo>
         {
            new SubnetInfo(
			    Name:          "snet-tier1-agw",
			    Description:   "Subnet for AGW",
			    AddressPrefix: $"{SpokePrefix}.1.0/24"
            ),
            new SubnetInfo(
			    Name:          "snet-tier1-webin",
			    Description:   "Subnet for other LBs",
			    AddressPrefix: $"{SpokePrefix}.2.0/24"
            ),
            new SubnetInfo(
			    Name:          "snet-tier1-rsvd1",
			    Description:   "Tier 1 reserved subnet",
			    AddressPrefix: $"{SpokePrefix}.3.0/25"
            ),
            new SubnetInfo(
			    Name:          "snet-tier1-rsvd2",
			    Description:   "Tier 1 reserved subnet",
			    AddressPrefix: $"{SpokePrefix}.3.128/25"
            ),
            new SubnetInfo(
			    Name:          "snet-tier2-pckr",
			    Description:   "Subnet for packer",
			    AddressPrefix: $"{SpokePrefix}.7.0/24"
            ),
            new SubnetInfo(
			    Name:          "snet-tier2-vm",
			    Description:   "Subnet for VMs",
			    AddressPrefix: $"{SpokePrefix}.8.0/21"
            ),
            new SubnetInfo(
			    Name:          "snet-tier2-aks",
			    Description:   "Subnet for AKS",
			    AddressPrefix: $"{SpokePrefix}.16.0/20"
            ),
            new SubnetInfo(
			    Name:          "snet-tier3-mi",
			    Description:   "Subnet for managed instance",
			    AddressPrefix: $"{SpokePrefix}.32.0/26"
            ),
            new SubnetInfo(
			    Name:          "snet-tier3-dbaz",
			    Description:   "Subnet for SQL Azure",
			    AddressPrefix: $"{SpokePrefix}.32.64/26"
            ),
            new SubnetInfo(
			    Name:          "snet-tier3-dblb",
			    Description:   "Subnet for LB for SQL VM",
			    AddressPrefix: $"{SpokePrefix}.32.128/25"
            ),
            new SubnetInfo(
			    Name:          "snet-tier3-dbvm",
			    Description:   "Subnet for SQL VM",
			    AddressPrefix: $"{SpokePrefix}.33.0/25"
            ),
            new SubnetInfo(
			    Name:          "snet-tier3-strg",
			    Description:   "Subnet for storage account/fileshares",
			    AddressPrefix: $"{SpokePrefix}.33.128/25"
            ),
            new SubnetInfo(
			    Name:          "snet-tier3-redis",
			    Description:   "Subnet for redis cache",
			    AddressPrefix: $"{SpokePrefix}.34.0/25"
            ),

         };
    }

}
