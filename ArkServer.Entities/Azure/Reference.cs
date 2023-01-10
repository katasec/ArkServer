namespace ArkServer.Entities.Azure;

public class CIDRGenerator
{

    /// <summary>
    /// Hub is a reference Azure VNET used to model hub vnet deployments.
    /// </summary>
    public VNetInfo Hub;


    /// <summary>
    ///  We used to use 172.16 but it's to close to the Docker bridge CIDR and the spokes create an overlap. Hance switching to 10.x
    /// </summary>
    private readonly string HubPrefix;


    /// <summary>
    /// The first octet used by the CIDRs for all networks. This defaults to 10. For e.g. 10.16.0.0/24
    /// </summary>
    private readonly int _octet1;

    /// <summary>
    /// The second octet used by the CIDRs for the Hub Subnet. This defaults to 16. For e.g. 10.16.0.0/24
    /// </summary>
    private readonly int _octet2Hub;

    /// <summary>
    /// The second octet used by the CIDRs for the Spoke Subnet. This defaults to 17. For e.g. 10.17.0.0/24
    /// </summary>
    private readonly int _octet2Spoke;

    public CIDRGenerator(int octet1=10, int octet2=16)
    {
        // Choose 1st from parameter or default
        _octet1 = octet1;

        // Choose 2nd Octet for Hub from parameter or default
        _octet2Hub = octet2;

        // Set 2nd Octet of spoke to be hub + 1
        _octet2Spoke = _octet2Hub +1;

        // Generate Hub prefix
        HubPrefix = $"{octet1}.{_octet2Hub}";

        // Generate Hub CIDRs
        Hub = new VNetInfo(
            Name: "vnet-hub",
            AddressPrefix: $"{HubPrefix}.0.0/24",
            SubnetsInfo: HubSubnets()
        );

    }
    
    /// <summary>
    /// Takes a list of 'environments' to create such as Dev, QA, Prod and generates the subnets for those environments
    /// </summary>
    /// <param name="Environments">List of environments for e.g. Dev, Prod</param>
    /// <returns></returns>
    public List<VNetInfo> Spokes(List<string> Environments)
    {
        var envs = new List<VNetInfo>();

        int offset=0;
        foreach(var name in Environments)
        {
            var env = new VNetInfo(
                Name: $"vnet-{name}",
                AddressPrefix: $"{_octet1}.{_octet2Spoke + offset}.0.0/16",
                SubnetsInfo: SpokeSubnets(offset)
            );
            envs.Add(env);

            offset++;
        }
        return envs;
    }

    /// <summary>
    /// Returns a list of subnets for creation for the hub
    /// </summary>
    /// <returns></returns>
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

    /// <summary>
    /// Return a list of subnets for creatin for a spoke network
    /// </summary>
    /// <param name="offset"></param>
    /// <returns></returns>
    private IEnumerable<SubnetInfo> SpokeSubnets(int offset=0)
    {
        var prefix = $"{_octet1}.{_octet2Spoke + offset}";
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
