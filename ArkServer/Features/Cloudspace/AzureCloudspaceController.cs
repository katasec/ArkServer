using ArkServer.Entities.Azure;
using ArkServer.Repositories;
using ArkServer.Services;
using Azure.Messaging.ServiceBus;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net;

namespace ArkServer.Features.Cloudspace
{
    [ApiController]
    public class AzureCloudspaceController : ControllerBase
    {
        private readonly AsbService _asbService;
        private readonly ArkService _arkService;

        private ILogger<AzureCloudspaceController> _logger;
        private string ApiHost => HttpContext.Request.Host.ToString();
        
        public AzureCloudspaceController(ILogger<AzureCloudspaceController> logger,AsbService asbService, ArkService arkService)
        {
            _asbService = asbService;
            _arkService = arkService;
            _logger = logger;
        }

        [HttpPost]
        [Route("/azure/cloudspace")]
        public async Task<IResult> Post(AzureCloudspaceRequest req)
        {
            var uri = $"https://{ApiHost}/azure/cloudspace/{req.Name}";


            // Transform user request to an equivalent cloudpsace spec
            var svc = new AzureCloudspaceService(req);
            var cs = svc.GenAzureCloudspace();


            // Add the cloudspace to Ark DB and send the worker a message to create it
            if (await _arkService.AddCloudSpace(cs))
            {
                await _asbService.Sender.SendMessageAsync(new ServiceBusMessage(cs.ToString()) { Subject = req.GetType().Name });
                return Results.Accepted(uri);
            }
            else
            {
                HttpContext.Response.Headers.Location = uri;
                return Results.Conflict();
            }

        }
    }
}
