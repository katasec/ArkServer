using ArkServer.Entities.Azure;
using ArkServer.Features.Cloudspace;
using ArkServer.Services;
using Microsoft.Extensions.Logging;
using Moq;

namespace ArkServer.Test
{

    
    public class Tests
    {
        private readonly Ark ark;
        private readonly IArkRepo db;
        private readonly ArkService svc;

        public Tests()
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
            var hub = new ReferenceNetwork().Hub;

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
            var spoke = new ReferenceNetwork().Spoke;

            Console.WriteLine("Name:" + spoke.Name);
            Console.WriteLine("Spoke CIDR:" + spoke.AddressPrefix);

            foreach( var subnet in spoke.SubnetsInfo)
            {
                Console.WriteLine(subnet.Name + ": " + subnet.AddressPrefix);
            }

        }

        [Test]
        public void ReadAMessage()
        {
            var asbService = new AsbService();

            var message = asbService.Receiver.ReceiveMessageAsync().Result;
            
            Console.WriteLine(message.Body);

        }
    }
}