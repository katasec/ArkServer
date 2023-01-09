using ArkServer.Entities.Azure;
using ArkServer.Features.Cloudspace;
using Azure.Messaging.ServiceBus;

namespace ArkServer.Services
{
    public class ArkService //: IHostedService
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

        public async Task<bool> AddCloudSpace(AzureCloudspace cs) {
            
            var status = false;

            // Skip if project already exists
            if (Ark.AzureCloudspace.Any( x=> x.Name == cs.Name))
            {
                _logger.Log(LogLevel.Information,"Cloudspace already exists");
                status = false;
            } 
            else
            {
                 _logger.Log(LogLevel.Information,"New Cloudspace! Adding the cloudspace");
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
