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
            cs.Action = HttpContext.Request.Method.ToLower();  

            // Add the cloudspace to Ark DB
            var exists = !await _arkService.AddCloudSpace(cs);

            // Send Message to queue
            var subject = cs.GetType().Name;
            await _asbService.Sender.SendMessageAsync(new ServiceBusMessage(cs.ToString()) { Subject = subject });

            // Return status
            if (exists)
            {
                // Send message to worker to run the pulumi handler program but return a 409 (Conflict) with Location header to resource
                HttpContext.Response.Headers.Location = uri;
                return Results.Conflict();
            }

            // Send message to worker to run the pulumi handler program & return a 202 (Accepted)
            return Results.Accepted(uri);
        }


        [HttpDelete]
        [Route("/azure/cloudspace")]
        public async Task<IResult> Delete(AzureCloudspaceRequest req)
        {
            var uri = $"https://{ApiHost}/azure/cloudspace/{req.Name}";

            // Transform user request to an equivalent cloudpsace spec
            var svc = new AzureCloudspaceService(req);
            var cs = svc.GenAzureCloudspace();
            cs.Action = HttpContext.Request.Method.ToLower();  

            // Add the cloudspace to Ark DB
            var exists = !await _arkService.AddCloudSpace(cs);

            // Send Message to queue
            var subject = cs.GetType().Name;
            await _asbService.Sender.SendMessageAsync(new ServiceBusMessage(cs.ToString()) { Subject = subject });

            // Return status
            if (exists)
            {
                // Send message to worker to run the pulumi handler program but return a 409 (Conflict) with Location header to resource
                HttpContext.Response.Headers.Location = uri;
                return Results.Conflict();
            }

            // Send message to worker to run the pulumi handler program & return a 202 (Accepted)
            return Results.Accepted(uri);
        }
    }
}
