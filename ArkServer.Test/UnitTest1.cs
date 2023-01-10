using ArkServer.Entities.Azure;
using ArkServer.Features.Cloudspace;
using ArkServer.Repositories;
using ArkServer.Services;
using Microsoft.Extensions.Logging;
using Moq;
using System.Text.Json;

namespace ArkServer.Test
{


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
        public void AddCloudSpace()
        {
            //var hub = new VnetInfo()
            ////var cs = new AzureCloudspace("Hello",)
        }

        [Test]
        public void ListHubCidrs()
        {
            var hub = new NetworkGenerator().Hub;

            Console.WriteLine("Name:" + hub.Name);
            Console.WriteLine("Hub CIDR:" + hub.AddressPrefix);

            foreach( var subnet in hub.SubnetsInfo)
            {
                Console.WriteLine(subnet.Name + ": " + subnet.AddressPrefix);
            }

        }

        [Test]
        public void ListSpokeCidrs()
        {
            var envs = new List<string>{"prod","dev"};

            var spokes = new NetworkGenerator().Spokes(envs);

            foreach(var spoke in spokes)
            {
                Console.WriteLine(spoke.AddressPrefix);
                var subnets = spoke.SubnetsInfo;

                foreach(var subnet in subnets)
                {
                    Console.WriteLine($"{subnet.Name}: {subnet.AddressPrefix}");
                }
            }

        }

        [Test]
        public void ReadAMessage()
        {
            var asbService = new AsbService();

            var message = asbService.Receiver.ReceiveMessageAsync().Result;
            
            Console.WriteLine(message.Body);
        }

        [Test] 
        public void Cloudspace_Dto_To_Entity()
        {

            var req = new AzureCloudspaceRequest
            {
                Name = "coolspace",
                Environments = new List<string> {"Prod","Dev"}
            };
           
            var svc = new AzureCloudspaceService(req);
            svc.GenAzureCloudspace();
        }
    }
}