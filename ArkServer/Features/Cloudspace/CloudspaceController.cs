using ArkServer.Entities.Azure;
using ArkServer.Services;
using Azure.Messaging.ServiceBus;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace ArkServer.Features.Cloudspace
{
    [ApiController]
    public class CloudspaceController : ControllerBase
    {
        private readonly AsbService _asbService;
        private readonly IArkRepo _db;
        
        private readonly ArkService _arkService;
        private ILogger<CloudspaceController> _logger;

        public CloudspaceController(AsbService asbService, IArkRepo db, ArkService arkService, ILogger<CloudspaceController> logger)
        {
            _asbService = asbService;
            _db = db;
            _arkService = arkService;
            _logger = logger;
        }

        [HttpPost]
        [Route("/azure/cloudspace")]
        public async Task<IResult> Post(CloudspaceRequest req)
        {
            //var spokes = req.Spokes.ToList();
            var cs = new AzureCloudspace(
                Name: req.ProjectName,
                Hub: req.Hub,
                Spokes: req.Spokes
            );

            
            var added = await _arkService.AddCloudSpace(cs);
            if (added)
            {
                await _asbService.Sender.SendMessageAsync(new ServiceBusMessage(req.ToString()){Subject =  req.GetType().Name});
            }

            var uri = $"/azure/cloudspace/{req.ProjectName}";
            return Results.Accepted(uri);

        }
    }
}
