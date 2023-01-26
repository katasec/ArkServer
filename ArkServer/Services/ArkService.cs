using ArkServer.Entities.Azure;
using ArkServer.Repositories;

namespace ArkServer.Services
{
    public class ArkService
    {
        private Ark Ark {get; set; }

        private readonly ICloudspaceRepo _db;
        private readonly ILogger _logger;

        public ArkService(ICloudspaceRepo db, Ark ark, ILogger<ArkService> logger)
        {
            _db = db;
            Ark = _db.Get();
            _logger = logger;
        }

        /// <summary>
        /// Returns false if a cloudspace already exists.
        /// </summary>
        /// <param name="cs"></param>
        /// <returns></returns>
        public async Task<bool> AddCloudSpace(AzureCloudspace cs) 
        {
            // Skip cloudspace if exists
            if (Ark.AzureCloudspaces.Count > 0) {
                _logger.Log(LogLevel.Information, "A cloudspace already exists in ark. Ark only allows one cloudspace at this time to reduce complexity.");
                return false;
            }

            //var exists = Ark.AzureCloudspace.Any( x=> x.Name == cs.Name);
            //if (exists)
            //{
            //    return false;
            //}

            // Else add cloudspace to DB
            Ark.AzureCloudspaces.Add(cs);
            await Task.Run(() =>_db.Save(Ark));

            return true;
        }


    }
}
