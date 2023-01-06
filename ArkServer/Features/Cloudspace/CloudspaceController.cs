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
        private readonly IArkRepo _db;
        
        private readonly ArkService _arkService;


        public CloudspaceController(AsbService asbService, IArkRepo db, ArkService arkService)
        {
            _asbService = asbService;
            _db = db;
            _arkService = arkService;
        }

        [HttpPost]
        [Route("/azure/cloudspace")]
        public async Task<IResult> Post(CloudspaceRequest req)
        {
            //var spokes = req.Spokes.ToList();
            var cs = new AzureCloudspace(
                ProjectName: req.ProjectName,
                Hub: req.Hub,
                Spokes: req.Spokes
            );

            _arkService.AddCloudSpace(cs);

            //_ark.AzureCloudspace.Add(req.)
            //// Create cloudspace in DB
            //var azureCloudspace = new AzureCloudspace
            //{
            //    ProjectName = req.ProjectName,
            //};

            //if (_db.Create(azureCloudspace) == null)
            //    return Results.Problem("Could not save to DB", null, 500);

            //// Create message
            //var message = new ServiceBusMessage(req.ToString()) { Subject = "CloudSpaceRequest" };

            //// Send message
            //await _asbService.Sender.SendMessageAsync(message);
            //var url = $"https://{Request.Host}/projects/{req.ProjectName}";

            return Results.Accepted("Hello");

        }
    }
}
