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
                    .AddSpoke("egal")
                    .DelSpoke("ameer")
                    .AddSpoke("alex");
        
        Console.WriteLine($"Hub: {acs.Hub.AddressPrefix}");

        acs.Spokes.ToList().ForEach(x =>
        {
            Console.WriteLine($"Name:{x.Name}, AddressPrefix:{x.AddressPrefix}");
        });

    }

    [Test]
    public void CheckSpokesNull()
    {
        //var acs = new AzureCloudspace();
        //Console.WriteLine(acs.Spokes == null);
    }

    [Test]
    public void PlayingWithAcs()
    {
        // Create an empty azure cloudspace
        var acs = new AzureCloudspace();

        // Pass the cloudspace to a generator to generate CIDRs
        acs = CidrGenerator.GenerateHub(acs);
        acs = CidrGenerator.AddSpoke(acs,"prod");
        acs = CidrGenerator.AddSpoke(acs,"dev");
        
        acs.Spokes.ToList().ForEach(x => Console.WriteLine(x.AddressPrefix));
    }

}

