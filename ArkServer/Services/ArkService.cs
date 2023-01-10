using ArkServer.Entities.Azure;
using ArkServer.Repositories;

namespace ArkServer.Services
{
    public class ArkService
    {
        private Ark Ark {get; set; }

        private readonly IArkRepo _db;
        private readonly ILogger _logger;

        public ArkService(IArkRepo db, Ark ark, ILogger<ArkService> logger)
        {
            _db = db;
            Ark = _db.Get();
            _logger = logger;
        }

        public async Task<bool> AddCloudSpace(AzureCloudspace cs) 
        {
            // Skip cloudspace if exists
            var exists = Ark.AzureCloudspace.Any( x=> x.Name == cs.Name);
            if (exists)
            {
                return false;
            }

            // Else add cloudspace to DB
            Ark.AzureCloudspace.Add(cs);
            await Task.Run(() =>_db.Save(Ark));

            return true;
        }


    }
}
