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
    public void CheckSpokesNull()
    {
        var acs = new AzureCloudspace();
        Console.WriteLine(acs.Spokes == null);
    }

    [Test]
    public void GetFromDb()
    {
        // Create an empty azure cloudspace
        var emptyAcs = new AzureCloudspace();

        // Pass the cloudspace to a generator to generate CIDRs
        var hub = CidrGenerator.GetHub(emptyAcs);
        var spokes = CidrGenerator.GetSpokes(new HashSet<string>{"dev","prod"},emptyAcs);

        var acs = new AzureCloudspace
        {
            Hub = hub,
            Spokes = spokes     
        };

        acs.Spokes.ToList().ForEach(x => Console.WriteLine(x.AddressPrefix));
    }

    [Test]
    public void Stuff()
    {
        var request = new AzureCloudspaceRequest
        {
            Environments = new(){ "dev", "prod"}
        };

        //var acs = new AzureCloudspace();


		//var generator = new CidrGenerator(octet1, octet2);

		
   //     var cs = new AzureCloudspace
   //     {
			//Hub = generator.Hub,
   //         Env = generator.Spokes(_request.Environments)
   //     };

		var emptyAcs = new AzureCloudspace();
        
        
        //var hub = CidrGenerator.GetHub(emptyAcs);
        // var spokes = CidrGenerator.GetSpokes(emptyAcs);

        var spokes1 = new List<VNetInfo>()
        {
            new VNetInfo("dev") { AddressPrefix="10.1.10.0/24" },
            new VNetInfo("prod") { AddressPrefix="10.2.10.0/24"},
            new VNetInfo("prod") { AddressPrefix="10.3.10.0/24"},
            new VNetInfo("prod") { AddressPrefix="10.4.10.0/24"},
            new VNetInfo("prod") { AddressPrefix="10.5.10.0/24"},
            new VNetInfo("prod") { AddressPrefix="10.6.10.0/24"},
            new VNetInfo("prod") { AddressPrefix="10.7.10.0/24"},
        };
        
       var spokes2 = new List<VNetInfo>()
        {
            new VNetInfo("dev")
            {
                AddressPrefix="10.2.10.0/24"
            },
        };

        var allOctets= spokes1.Select(x => x.Octet2);
        var octetsInUse = spokes2.Select(x => x.Octet2);

        var availableOctets = allOctets.Except(octetsInUse);

        availableOctets.ToList().ForEach(Console.WriteLine);
    }
}

