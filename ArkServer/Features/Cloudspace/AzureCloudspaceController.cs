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

            // Add the cloudspace to Ark DB and send the worker a message to create it
            if (await _arkService.AddCloudSpace(cs))
            {
                // Send message to worker to run the pulumi handler program & return a 202 (Accepted)
                await _asbService.Sender.SendMessageAsync(new ServiceBusMessage(cs.ToString()) { Subject = cs.GetType().Name });
                return Results.Accepted(uri);
            }
            else
            {
                // Send message to worker to run the pulumi handler program but return a 409 (Conflict) with Location header to resource
                await _asbService.Sender.SendMessageAsync(new ServiceBusMessage(cs.ToString()) { Subject = cs.GetType().Name });

                HttpContext.Response.Headers.Location = uri;
                return Results.Conflict();
            }

        }


        [HttpDelete]
        [Route("/azure/cloudspace")]
        public async Task<IResult> Delete(AzureCloudspaceRequest req)
        {
            _logger.Log(LogLevel.Information,"Running delete");
            var uri = $"https://{ApiHost}/azure/cloudspace/{req.Name}";


            // Transform user request to an equivalent cloudpsace spec
            var svc = new AzureCloudspaceService(req);
            var cs = svc.GenAzureCloudspace();
            cs.Action = HttpContext.Request.Method.ToLower();  


            // Add the cloudspace to Ark DB and send the worker a message to create it
            var subject = cs.GetType().Name;
            if (await _arkService.AddCloudSpace(cs))
            {
                // Send message to worker to run the pulumi handler program & return a 202 (Accepted)
                await _asbService.Sender.SendMessageAsync(new ServiceBusMessage(cs.ToString()) { Subject = subject });
                return Results.Accepted(uri);
            }
            else
            {
                // Send message to worker to run the pulumi handler program but return a 409 (Conflict) with Location header to resource
                await _asbService.Sender.SendMessageAsync(new ServiceBusMessage(cs.ToString()) { Subject = subject });

                HttpContext.Response.Headers.Location = uri;
                return Results.Conflict();
            }

        }
    }
}
