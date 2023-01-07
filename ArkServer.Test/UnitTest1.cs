using ArkServer.Entities.Azure;
using ArkServer.Features.Cloudspace;
using ArkServer.Services;

namespace ArkServer.Test
{

    
    public class Tests
    {
        private readonly Ark ark;
        private readonly IArkRepo db;
        private readonly ArkService svc;

        public Tests()
        {
            ark = new Ark();
            db = new ArkJsonRepo();
            svc = new ArkService(db,ark);

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
            Console.WriteLine("Hub CIDR:" + spoke.AddressPrefix);

            foreach( var subnet in spoke.SubnetsInfo)
            {
                Console.WriteLine(subnet.Name + ": " + subnet.AddressPrefix);
            }

        }
    }
}