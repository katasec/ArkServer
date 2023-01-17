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
        private readonly Ark _ark;
        private ILogger<AzureCloudspaceController> _logger;
        private string ApiHost => HttpContext.Request.Host.ToString();
        private IArkRepo _arkRepo;

        public AzureCloudspaceController(ILogger<AzureCloudspaceController> logger,AsbService asbService,IArkRepo arkRepo)
        {
            _asbService = asbService;
            _logger = logger;
            _arkRepo = arkRepo;
            _ark= arkRepo.Get();
        }

        [HttpPost]
        [Route("/azure/cloudspace")]
        public async Task<IResult> Post(CreateAzureCloudspaceRequest req)
        {
            var exists = true;
            var uri = $"https://{ApiHost}/azure/cloudspace/{req.Name}";
            
            
            // If Cloudspace doesn't exist, create it.
            if (_ark.AzureCloudspaces.Count == 0)
            {
                _ark.AzureCloudspaces = new List<AzureCloudspace> { new AzureCloudspace()};
                exists= false;
            } 
     
            // Get Azure Cloudspace from Ark
            var acs = _ark.AzureCloudspaces[0];

            // Add Environments to cloudspace
            foreach (var name in req.Environments)
            {
                acs.AddSpoke(name);
            }

            _arkRepo.Save(_ark);


            // Send Message to queue
            var subject = req.GetType().Name;
            await _asbService.Sender.SendMessageAsync(new ServiceBusMessage(acs.ToString()) { Subject = subject });

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
        public async Task<IResult> Delete(DeleteAzureCloudspaceRequest req)
        {

            if (req.Name.ToLower() !=  "default")
            {
                return Results.Problem($"Cloudspace name was {req.Name}. Only 'default' is accepted as a cloudspace name");
            }
            var uri = $"https://{ApiHost}/azure/cloudspace/{req.Name}";

            //var name = req.Name.ToLower();
            //_ark.AzureCloudspaces[0].DelSpoke(name);

            Console.WriteLine($"This is a DeleteAzureCloudspaceRequest for {req.Name}");
            
            return null;
            //// Add the cloudspace to Ark DB
            //var exists = !await _arkService.AddCloudSpace(cs);

            //// Send Message to queue
            //var subject = cs.GetType().Name;
            //await _asbService.Sender.SendMessageAsync(new ServiceBusMessage(cs.ToString()) { Subject = subject });

            //// Return status
            //if (exists)
            //{
            //    // Send message to worker to run the pulumi handler program but return a 409 (Conflict) with Location header to resource
            //    HttpContext.Response.Headers.Location = uri;
            //    return Results.Conflict();
            //}

            //// Send message to worker to run the pulumi handler program & return a 202 (Accepted)
            //return Results.Accepted(uri);
        }
    }
}
