using ArkServer.Entities.Azure;

namespace ArkServer.Test
{

    
    public class Tests
    {
        private readonly Ark ark;

        public Tests()
        {
            ark = new Ark();
        }
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void OutputAzureCloudSpace()
        {

            //var subnet1 = new SubnetInfo(Name:"snet-tier2-vm", AddressPrefix:"10.0.1.0/24", Description:"VMs");
            //var subnets = new HashSet<SubnetInfo> {subnet1};

            //var a = new AzureCloudspace
            //(
            //    ProjectName:"Something",
            //    Hub:new VnetInfo("snet-tier2-vm","10.0.1.0/24",subnets),
            //    Spokes:new HashSet<VnetInfo>()
            //);
            
            //Console.WriteLine(a);


        }
    }
}