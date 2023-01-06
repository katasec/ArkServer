using ArkServer.Entities.Azure;
using ArkServer.Features.Cloudspace;

namespace ArkServer.Services
{
    public class ArkService : IHostedService
    {
        private Ark _ark {get; set; }

        private IArkRepo _db;

        public ArkService(IArkRepo db, Ark ark)
        {
            _db = db;
            _ark = db.Get();
        }


        public async Task StartAsync(CancellationToken cancellationToken)
        {
            await Task.Run( ()=> {
                _ark = _db.Get();
            });
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public bool AddCloudSpace(AzureCloudspace cs) {

            // Skip if project already exists
            if (_ark.AzureCloudspace.Any( x=> x.ProjectName == cs.ProjectName))
            {
                Console.WriteLine("Project already exists");
                return false;
            } 

            _ark.AzureCloudspace.Add(cs);
            _db.Save(_ark);
            return true;
        }
    }
}
