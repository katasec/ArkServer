using ArkServer.Entities.Azure;
using ArkServer.Features.Cloudspace;

namespace ArkServer.Services
{
    public class ArkService //: IHostedService
    {
        private Ark Ark {get; set; }

        private readonly IArkRepo _db;

        public ArkService(IArkRepo db, Ark ark)
        {
            _db = db;
            Ark = db.Get();
        }

        public async Task<bool> AddCloudSpace(AzureCloudspace cs) {
            
            var status = false;

            // Skip if project already exists
            if (Ark.AzureCloudspace.Any( x=> x.ProjectName == cs.ProjectName))
            {
                Console.WriteLine("Project already exists");
                status = false;
            } 
            else
            {
                Ark.AzureCloudspace.Add(cs);
                await Task.Run(() =>
                {
                    _db.Save(Ark);
                });
                status = true;
            }


            return status;

        }
    }
}
