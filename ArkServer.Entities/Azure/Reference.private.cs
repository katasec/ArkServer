using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArkServer.Entities.Azure;


public static partial class CidrGenerator
{
    
    private static bool HubGenerated;

    /// <summary>
    /// Hub is a reference Azure VNET used to model hub vnet deployments.
    /// </summary>
    private static VNetSpec? Hub;


    /// <summary>
    /// CIDR Prefix used for Hub. For e.g. if HubPrefix is "10.0" then the AzureFirewallSubnet becomes 10.0.0.0/26
    /// </summary>
    private static string? HubPrefix;

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

    /// <summary>
    /// Takes a list of 'environments' to create such as Dev, QA, Prod and generates the subnets for those environments
    /// </summary>
    /// <param name="Environments">List of environments for e.g. Dev, Prod</param>
    /// <returns></returns>
    private static HashSet<VNetSpec> GetSpokes(List<string> Environments)
    {
        if (Octet1== 0)
        {
            throw new ApplicationException("Octet1 is still 0, generate hub first by calling GetHub()");
        }
        var envs = new HashSet<VNetSpec>();

        int offset=0;
        foreach(var name in Environments)
        {
            // Use Env name for VNET Name
            var vnetName = $"vnet-{name.ToLower()}";
            var env = new VNetSpec(
                Name: $"vnet-{name}",
                AddressPrefix: $"{Octet1}.{SpokeOctet2 + offset}.0.0/16",
                SubnetsInfo: GenerateSpokeSubnets(offset)
            );
            envs.Add(env);

            offset++;
        }
        return envs;
    }

}
