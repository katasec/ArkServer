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
    }
}