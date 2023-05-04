using System.Security.Cryptography;

namespace ArkServer.Entities;

internal static class CidrGenerator
{

    internal static IEnumerable<SubnetInfo> GenerateHubSubnets(int octet1, int octet2)
    {
        var prefix = $"{octet1}.{octet2}";
        return new List<SubnetInfo>
         {
            new SubnetInfo{
                Name = "AzureFirewallSubnet",
                Description = "Subnet for Azure Firewall",
                AddressPrefix = $"{prefix}.0.0/26"
            },
            new SubnetInfo{
                Name = "AzureBastionSubnet",
                Description = "Subnet for Bastion",
                AddressPrefix = $"{prefix}.0.64/26"
            },
            new SubnetInfo{
                Name = "AzureFirewallManagementSubnet",
                Description = "Subnet for VPN Gateway",
                AddressPrefix = $"{prefix}.0.128/26"
            },
            new SubnetInfo{
                Name = "GatewaySubnet",
                Description = "Subnet for VPN Gateway",
                AddressPrefix = $"{prefix}.0.192/27"
            },
            new SubnetInfo{
                Name = "snet-test",
                Description = "Subnet for Testing purposes",
                AddressPrefix = $"{prefix}.0.224/27"
            },
         };
    }
    internal static IEnumerable<SubnetInfo> GenerateSpokeSubnets(int Octet1, int Octet2)
    {
        var prefix = $"{Octet1}.{Octet2}";
        return new List<SubnetInfo>
        {
            new SubnetInfo{
                Name =          "snet-tier1-agw",
                Description =   "Subnet for AGW",
                AddressPrefix = $"{prefix}.1.0/24",
                Tags = new ()
                {
                    new KeyValuePair<string,string>("ark:tier","1")
                }
            },
            new SubnetInfo{
                Name = "snet-tier1-webin",
                Description = "Subnet for other LBs",
                AddressPrefix = $"{prefix}.2.0/24",
                Tags = new ()
                {
                    new KeyValuePair<string,string>("ark:tier","1")
                }

            },
            new SubnetInfo{
                Name = "snet-tier1-rsvd1",
                Description = "Tier 1 reserved subnet",
                AddressPrefix = $"{prefix}.3.0/25",
                Tags = new ()
                {
                    new KeyValuePair<string,string>("ark:tier","1")
                }

            },
            new SubnetInfo{
                Name = "snet-tier1-rsvd2",
                Description = "Tier 1 reserved subnet",
                AddressPrefix = $"{prefix}.3.128/25",
                Tags = new ()
                {
                    new KeyValuePair<string,string>("ark:tier","1")
                }

            },
            new SubnetInfo{
                Name = "snet-tier2-pckr",
                Description = "Subnet for packer",
                AddressPrefix = $"{prefix}.7.0/24",
                Tags = new ()
                {
                    new KeyValuePair<string,string>("ark:tier","1")
                }

            },
            new SubnetInfo{
                Name = "snet-tier2-vm",
                Description = "Subnet for VMs",
                AddressPrefix = $"{prefix}.8.0/21",
                Tags = new ()
                {
                    new KeyValuePair<string,string>("ark:tier","1")
                }

            },
            new SubnetInfo{
                Name = "snet-tier2-aks",
                Description = "Subnet for AKS",
                AddressPrefix = $"{prefix}.16.0/20",
                Tags = new ()
                {
                    new KeyValuePair<string,string>("ark:tier","1")
                }

            },
            new SubnetInfo{
                Name = "snet-tier3-mi",
                Description = "Subnet for managed instance",
                AddressPrefix = $"{prefix}.32.0/26",
                Tags = new ()
                {
                    new KeyValuePair<string,string>("ark:tier","1")
                }

            },
            new SubnetInfo{
                Name = "snet-tier3-dbaz",
                Description = "Subnet for SQL Azure",
                AddressPrefix = $"{prefix}.32.64/26",
                Tags = new ()
                {
                    new KeyValuePair<string,string>("ark:tier","1")
                }

            },
            new SubnetInfo{
                Name = "snet-tier3-dblb",
                Description = "Subnet for LB for SQL VM",
                AddressPrefix = $"{prefix}.32.128/25",
                Tags = new ()
                {
                    new KeyValuePair<string,string>("ark:tier","1")
                }

            },
            new SubnetInfo{
                Name = "snet-tier3-dbvm",
                Description = "Subnet for SQL VM",
                AddressPrefix = $"{prefix}.33.0/25",
                Tags = new ()
                {
                    new KeyValuePair<string,string>("ark:tier","1")
                }

            },
            new SubnetInfo{
                Name = "snet-tier3-strg",
                Description = "Subnet for storage account/fileshares",
                AddressPrefix = $"{prefix}.33.128/25",
                Tags = new ()
                {
                    new KeyValuePair<string,string>("ark:tier","1")
                }

            },
            new SubnetInfo{
                Name = "snet-tier3-redis",
                Description = "Subnet for redis cache",
                AddressPrefix = $"{prefix}.34.0/25",
                Tags = new ()
                {
                    new KeyValuePair<string,string>("ark:tier","1")
                }
            },
        };
    }

}

