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

            return Results.Accepted("Hello");

        }
    }
}
