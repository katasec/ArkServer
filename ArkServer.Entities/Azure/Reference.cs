using System.Security.Cryptography;

namespace ArkServer.Entities.Azure;

public static class CidrGenerator
{

    /// <summary>
    /// Hub is a reference Azure VNET used to model hub vnet deployments.
    /// </summary>
    public static VNetInfo Hub;


    /// <summary>
    /// CIDR Prefix used for Hub. For e.g. if HubPrefix is "10.0" then the AzureFirewallSubnet becomes 10.0.0.0/26
    /// </summary>
    private static string HubPrefix;


    /// <summary>
    /// The first octet used by the CIDRs for all networks. This defaults to 10. For e.g. 10.16.0.0/24
    /// </summary>
    private static int Octet1 = 10;

    /// <summary>
    /// The second octet used by the CIDRs for the Hub Subnet. This defaults to 16. For e.g. 10.16.0.0/24
    /// </summary>
    private static int HubOctet2 = 16;

    /// <summary>
    /// The second octet used by the CIDRs for the Spoke Subnet. This defaults to 17. For e.g. 10.17.0.0/24
    /// </summary>
    private static int SpokeOctet2 = HubOctet2 +1 ;

    //static CidrGenerator(int octet1=10, int octet2=16)
    //{
    //    // Choose 1st from parameter or default
    //    _octet1Common = octet1;

    //    // Choose 2nd Octet for Hub from parameter or default
    //    _octet2Hub = octet2;

    //    // Set 2nd Octet of spoke to be hub + 1
    //    _octet2Spoke = _octet2Hub +1;

    //    // Generate Hub prefix
    //    HubPrefix = $"{octet1}.{_octet2Hub}";

    //    // Generate Hub CIDRs
    //    Hub = new VNetInfo(
    //        Name: "vnet-hub",
    //        AddressPrefix: $"{HubPrefix}.0.0/24",
    //        SubnetsInfo: HubSubnets()
    //    );

    //}
    
    //public GetHub()
    //{

    //}
    //public CidrGenerator(AzureCloudspace cs)
    //{
    //    // Choose 1st from parameter or default
    //    _octet1Common = cs.Octet1;

    //    // Choose 2nd Octet for Hub from parameter or default
    //    _octet2Hub = cs.Octet2;

    //    // Set 2nd Octet of spoke to be hub + 1
    //    _octet2Spoke = _octet2Hub +1;

    //    // Generate Hub prefix
    //    HubPrefix = $"{_octet1Common}.{_octet2Hub}";

    //    // Generate Hub CIDRs
    //    Hub = new VNetInfo(
    //        Name: "vnet-hub",
    //        AddressPrefix: $"{HubPrefix}.0.0/24",
    //        SubnetsInfo: HubSubnets()
    //    );
    //}

    /// <summary>
    /// Takes a list of 'environments' to create such as Dev, QA, Prod and generates the subnets for those environments
    /// </summary>
    /// <param name="Environments">List of environments for e.g. Dev, Prod</param>
    /// <returns></returns>
    public static HashSet<VNetInfo> GetSpokes(List<string> Environments)
    {
        if (Octet1== 0)
        {
            throw new ApplicationException("Octet1 is still 0, generate hub first by calling GetHub()");
        }
        var envs = new HashSet<VNetInfo>();

        int offset=0;
        foreach(var name in Environments)
        {
            // Use Env name for VNET Name
            var vnetName = $"vnet-{name.ToLower()}";

            //
            
            var env = new VNetInfo(
                Name: $"vnet-{name}",
                AddressPrefix: $"{Octet1}.{SpokeOctet2 + offset}.0.0/16",
                SubnetsInfo: GenerateSpokeSubnets(offset)
            );
            envs.Add(env);

            offset++;
        }
        return envs;
    }

    public static bool CheckEmpty(AzureCloudspace acs) 
    {
        if (acs == null)
        {
            return true;
        } 
        else if (acs.Spokes == null)
        {
            return true;
        } 
        else
        {
            return false;
        }
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
    public static HashSet<VNetInfo> GetSpokes(HashSet<string> Environments, AzureCloudspace acs)
    {
        // Cannot generate spokes before Hub, return error
        if (Octet1== 0)
        {
            throw new ApplicationException("Octet1 is still 0, generate hub first by calling GetHub()");
        }

        // Initialize empty list of spokes.
        var spokes = new HashSet<VNetInfo>();

        // Check if the cloudspace is empty
        var emptyAcs = CheckEmpty(acs);

        if (emptyAcs)
        {
            /* 
             * If cloudspace is empty, no need to check for overlapping addresses.
             * Generate CIDRs using the preferences defined within the cloudspace.
             */

            int offset=0;
            foreach(var name in Environments)
            {

                // Generate Virtual Network CIDRs
                var vnetName = $"vnet-{name}";
                var addressPrefix = $"{acs.Octet1}.{acs.SpokeOctet2Start + offset}.0.0/16";
                var subnets = GenerateSpokeSubnets(offset);
                var vnetInfo = new VNetInfo(vnetName, addressPrefix, subnets){
                    SpokeOctet2Offset= offset,
                };

                // Add to list of spokes
                spokes.Add(vnetInfo);

                offset++;
            }

            return spokes;
        } 
        else
        {
            /* 
             * If cloudspace already has existing networks, we need to
             */
        }
        //var octetsInUse = acs.Spokes.Select(x=>x.Octet2);
        //Console.WriteLine("We need to generate some spokes");
        return null;
    }

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
    public static VNetInfo GetHub(AzureCloudspace acs)
    {
        // Use Octet definitions from ACS
        Octet1 = acs.Octet1;
        HubOctet2 = acs.HubOctet2;

        // Generate Hub prefix
        HubPrefix = $"{Octet1}.{HubOctet2}";

        // Generate Hub CIDRs
        Hub = new VNetInfo(
            Name: "vnet-hub",
            AddressPrefix: $"{HubPrefix}.0.0/24",
            SubnetsInfo: GenerateHubSubnets()
        );
        return Hub;
    }
}
