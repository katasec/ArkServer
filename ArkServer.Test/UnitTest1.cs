using ArkServer.Entities.Azure;
using ArkServer.Features.Cloudspace;
using ArkServer.Repositories;
using ArkServer.Services;
using Microsoft.Extensions.Configuration.EnvironmentVariables;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework.Constraints;
using System.Text.Json;

namespace ArkServer.Test;



public class ScratchPad
{
    private readonly Ark ark;
    private readonly IArkRepo db;
    private readonly ArkService svc;

    public ScratchPad()
    {
        ILogger<ArkJsonRepo> ArkJsonRepoLogger = (new Mock<ILogger<ArkJsonRepo>>()).Object;
        ILogger<ArkService> ArkServiceLogger = (new Mock<ILogger<ArkService>>()).Object;

        ark = new Ark();
        db = new ArkJsonRepo(ArkJsonRepoLogger);
        svc = new ArkService(db,ark,ArkServiceLogger);
    }
    [SetUp]
    public void Setup()
    {
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
        
        
        //Console.WriteLine($"Hub: {acs.Hub.AddressPrefix}");

        ////Assert.True(acs.Hub.SubnetsInfo.Count() == 5);
        //Assert.That(acs.Hub.SubnetsInfo.Count() >= 4,"Hub does not have 4 subnets");

        //acs.Hub.SubnetsInfo.ToList().ForEach((x) =>
        //{
        //    Console.WriteLine($"Hub Subnet:{x.Name}, AddressPrefix:{x.AddressPrefix}");
        //});
        //Console.WriteLine();


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
    public void HashsetStuff()
    {
        var nets = new HashSet<VNetSpec>();

        var spoke1 = new VNetSpec(
            Name: "hi",
            AddressPrefix:"1.1.1.1",
            SubnetsInfo: new List<SubnetInfo>
            {
                new SubnetInfo("subnet1","10.0.0.0","cool subet")
            }
        );

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
}

