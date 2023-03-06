using ArkServer.Entities.Azure;
using ArkServer.Features.Cloudspace;
using ArkServer.Repositories;
using ArkServer.Services;
using Microsoft.Extensions.Configuration.EnvironmentVariables;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework.Constraints;
using PulumiApi;
using PulumiApi.Models;
using System.Text.Json;

namespace ArkServer.Test;


public class ScratchPad
{
    private ApiClient client;
    private string orgName;
    private string projectName;
    private string stackName;

    private readonly Ark ark;
    private readonly ICloudspaceRepo db;
    private readonly ArkService svc;

    public ScratchPad()
    {
        ILogger<CloudspaceJsonRepo> ArkJsonRepoLogger = (new Mock<ILogger<CloudspaceJsonRepo>>()).Object;
        ILogger<ArkService> ArkServiceLogger = (new Mock<ILogger<ArkService>>()).Object;

        ark = new Ark();
        db = new CloudspaceJsonRepo(ArkJsonRepoLogger);
        svc = new ArkService(db,ark,ArkServiceLogger);
    }
    [SetUp]
    public async Task Setup()
    {
        client = new ApiClient();
        (orgName, projectName, stackName) = await GetStackInfo();
        projectName = "azurecloudspace";
    }

    [Test]
    public void CheckHub()
    {
        var acs = new AzureCloudspace()
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
        var arkConfig = ArkConfig.Read();
        Console.WriteLine(arkConfig.AzureConfig.MqConfig.MqName);
    }

    [Test]
    public void HashsetStuff()
    {
        var nets = new HashSet<VNetSpec>();

        var spoke1 = new VNetSpec{
            Name= "hi",
            AddressPrefix="1.1.1.1",
            SubnetsInfo= new List<SubnetInfo>
            {
                new SubnetInfo{
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

    [Test]
    public void PrintJson()
    {
        var acs = new CreateAzureCloudspaceRequest
        {
            Environments = new List<string>{"dev"}
        };

        Console.WriteLine(acs.ToString());

        var dacs = new DeleteAzureCloudspaceRequest();
        Console.WriteLine(dacs.ToString());

    }

    [Test]
    public void HelloMsg()
    {
        var hello = new HelloSuccess { Message="sas" };
        Console.WriteLine(hello.ToString());
    }

    public async Task<Tuple<string?, string?, string?>?> GetStackInfo()
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
        return null;
    }

    [Test]
    public async Task GetDeploymentResource()
    {
        var result = await client.GetStackState(orgName, projectName, stackName);

        if (result.Deployment != null)
        {
            var resource = result.GetResourceByName(
                type: "azure-native:network:VirtualNetwork",
                pulumiName: "vnet-hub"
            );

            var myjson = resource.ToJson();

            //Console.Write(myjson);
            var x = JsonSerializer.Deserialize<Something>(myjson);

            var y = x.Outputs.AddressSpace.AddressPrefixes[0];
            Console.WriteLine(y);
        }
    }
}

