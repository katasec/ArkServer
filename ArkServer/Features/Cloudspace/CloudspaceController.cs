using ArkServer.Entities.Azure;
using ArkServer.Services;
using Azure.Messaging.ServiceBus;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net;

namespace ArkServer.Features.Cloudspace
{
    [ApiController]
    public class CloudspaceController : ControllerBase
    {
        private readonly AsbService _asbService;
        private readonly ArkService _arkService;
        private ILogger<CloudspaceController> _logger;

        public CloudspaceController(AsbService asbService, IArkRepo db, ArkService arkService, ILogger<CloudspaceController> logger)
        {
            _asbService = asbService;
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

            
            var host= HttpContext.Request.Host.ToString();
            var uri = $"https://{host}/azure/cloudspace/{req.ProjectName}";
            var notExists = await _arkService.AddCloudSpace(cs);
            if (notExists)
            {
                await _asbService.Sender.SendMessageAsync(new ServiceBusMessage(req.ToString()){Subject =  req.GetType().Name});
                return Results.Accepted(uri);
            }
            else
            {
                HttpContext.Response.Headers.Location= uri;
                return Results.Conflict();

            }

            
            

        }
    }
}
