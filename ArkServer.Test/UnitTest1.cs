using PulumiApi;
using PulumiApi.Models;
using ServiceStack;
using Ark.Entities;
using Ark.Server.Features.ManagedCluster;
using Ark.Base;

namespace ArkServer.Test;


public class ScratchPad
{
    private ApiClient client;
    private string? orgName;
    private string? projectName;
    private string? stackName;

    private readonly Entities.Azure.Ark ark;

    public ScratchPad()
    {
        ark = new Entities.Azure.Ark();
        
    }
    [SetUp]
    public async Task Setup()
    {
        (orgName, projectName, stackName) = await GetStackInfo();
        client = new ApiClient();
        projectName = "azurecloudspace";
    }

    [Test]
    public void CheckHub()
    {
        var acs = new Entities.Azure.AzureCloudspace()
                    .AddSpoke("ameer")
                    .AddSpoke("egal");

        

       acs.Spokes.ToList().ForEach(spoke =>
        {
            Console.WriteLine($"Name:{spoke.Name}, AddressPrefix:{spoke.AddressPrefix}");

            spoke.SubnetsInfo.ToList().ForEach((x) =>
            {
                Console.WriteLine($"Spoke Subnet:{x.Name}, AddressPrefix:{x.AddressPrefix}");
            });
            Console.WriteLine();
        });

        Console.WriteLine();
        Console.WriteLine("Delete Ameer");
        Console.WriteLine();

        acs.DelSpoke("ameer")
            .AddSpoke("alex")
            .AddSpoke("ameer");
        

        acs.Spokes.ToList().ForEach(spoke =>
        {
            Console.WriteLine($"Name:{spoke.Name}, AddressPrefix:{spoke.AddressPrefix}");

            spoke.SubnetsInfo.ToList().ForEach((x) =>
            {
                Console.WriteLine($"Spoke Subnet:{x.Name}, AddressPrefix:{x.AddressPrefix}");
            });
            Console.WriteLine();
        });

    }

    [Test]
    public void ReadYamlConfig()
    {
        var arkConfig = Config.Read();
        Console.WriteLine(arkConfig.AzureConfig.MqConfig.MqName);
    }

    [Test]
    public void HashsetStuff()
    {
        var nets = new HashSet<Entities.Azure.VNetSpec>();

        var spoke1 = new Entities.Azure.VNetSpec{
            Name= "hi",
            AddressPrefix="1.1.1.1",
            SubnetsInfo= new List<Entities.Azure.SubnetInfo>
            {
                new Entities.Azure.SubnetInfo{
                    Name="subnet1",
                    AddressPrefix="10.0.0.0",
                    Description="cool subet" 
                }
            }
        };

        Console.WriteLine(nets.Add(spoke1));
        Console.WriteLine(nets.Add(spoke1));
        Console.WriteLine(nets.Add(spoke1));


    }

    //[Test]
    //public void PrintJson()
    //{
    //    var acs = new CreateAzureCloudspaceRequest
    //    {
    //        Environments = new List<string>{"dev"}
    //    };

    //    Console.WriteLine(acs.ToString());

    //    var dacs = new DeleteAzureCloudspaceRequest();
    //    Console.WriteLine(dacs.ToString());

    //}

    [Test]
    public void HelloMsg()
    {
        var hello = new HelloSuccess { Message="sas" };
        Console.WriteLine(hello.ToString());
    }

    public async Task<Tuple<string?, string?, string?>> GetStackInfo()
    {
        var result = await client.ListStacks();
        if (result.Stacks != null)
        {
            var myStack = result.Stacks[0];

            var orgName = myStack.OrgName;
            var projectName = myStack.ProjectName;
            var stackName = myStack.StackName;

            return Tuple.Create(orgName, projectName, stackName);
        }
        else
        {
            throw new ApplicationException("No stacks found");
        }
    }

    //[Test]
    //public async Task GetDeploymentResource()
    //{
    //    var result = await client.GetStackState(orgName, projectName, stackName);

    //    if (result.Deployment == null) throw new ApplicationException("Deployment was null");

    //    var rg = result.Deployment.GetAzureResourceGroup("rg-ameer");
    //    Console.WriteLine(rg);

    //    var x = result.Deployment.GetAzureVnetSpec("ameer");

    //    if (x.Outputs == null) throw new ApplicationException("VNET was null");

    //    Console.WriteLine(x.Outputs?.AddressSpace?.AddressPrefixes[0]);

    //    foreach (var subnet in x.Outputs.Subnets)
    //    {
    //        Console.WriteLine($"{subnet.Name} {subnet.AddressPrefix}");
    //    }

    //}

    [Test]
    public void CreateManagedClusterJson()
    {
        var x = new CreateManagedClusterRequest
        {
            Name = "myaks01",
            Args = new ManagedClusterArgs
            {
                Cloudspace = new CloudspaceArgs
                {
                    Name = "ameer",
                    Environment = "dev"
                }
            }
        };

        Console.WriteLine(x.ToJson());
        
    }
}

