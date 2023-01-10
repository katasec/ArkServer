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
        private readonly AzureCloudspaceService _cloudspaceSvc;

        private ILogger<AzureCloudspaceController> _logger;
        private string ApiHost => HttpContext.Request.Host.ToString();
        
        public AzureCloudspaceController(ILogger<AzureCloudspaceController> logger,AsbService asbService, ArkService arkService, AzureCloudspaceService cloudspaceSvc)
        {
            _asbService = asbService;
            _arkService = arkService;
            _logger = logger;
            _cloudspaceSvc = cloudspaceSvc;
        }

        [HttpPost]
        [Route("/azure/cloudspace")]
        public async Task<IResult> Post(AzureCloudspaceRequest req)
        {
            var uri = $"https://{ApiHost}/azure/cloudspace/{req.Name}";


            var cs = _cloudspaceSvc.GenAzureCloudspace();


            if (await _arkService.AddCloudSpace(cs))
            {
                await _asbService.Sender.SendMessageAsync(new ServiceBusMessage(req.ToString()) { Subject = req.GetType().Name });
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
