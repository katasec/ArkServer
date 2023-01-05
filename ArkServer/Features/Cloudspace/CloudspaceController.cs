using ArkServer.Entities.Azure;
using ArkServer.Services;
using Azure.Messaging.ServiceBus;
using Microsoft.AspNetCore.Mvc;

namespace ArkServer.Features.Cloudspace
{
    [ApiController]
    public class CloudspaceController : ControllerBase
    {
        private readonly AsbService _asbService;
        private readonly IAzureCsRepo _db;
        
        public CloudspaceController(AsbService asbService, IAzureCsRepo db)
        {
            _asbService = asbService;
            _db = db;
        }

        [HttpPost]
        [Route("/azure/cloudspace")]
        public async Task<IResult> Post(CloudspaceRequest req)
        {
            // Create cloudspace in DB
            var azureCloudspace= new AzureCloudspace
            {
                ProjectName = req.ProjectName,
                Name = req.Name
            };

            if (_db.Create(azureCloudspace) == null) 
                return Results.Problem("Could not save to DB", null, 500);

            // Create message
            var message = new ServiceBusMessage(req.ToString()) { Subject = "CloudSpaceRequest" };

            // Send message
            await _asbService.Sender.SendMessageAsync(message);
            var url = $"https://{Request.Host}/projects/{req.ProjectName}";

            return Results.Accepted(url);

        }
    }
}
