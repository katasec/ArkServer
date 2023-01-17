using ArkServer.Entities.Azure;
using ArkServer.Repositories;
using ArkServer.Services;
using Azure.Messaging.ServiceBus;
using Microsoft.AspNetCore.Mvc;

namespace ArkServer.Features.Cloudspace;

[ApiController]
public class AzureCloudspaceController : ControllerBase
{
    private readonly AsbService _asbService;
    private readonly Ark Ark;
    private ILogger<AzureCloudspaceController> _logger;
    private string ApiHost => HttpContext.Request.Host.ToString();
    private IArkRepo _arkRepo;

    public AzureCloudspaceController(ILogger<AzureCloudspaceController> logger,AsbService asbService,IArkRepo arkRepo)
    {
        _asbService = asbService;
        _logger = logger;
        _arkRepo = arkRepo;
        Ark= arkRepo.Get();
    }

    [HttpPost]
    [Route("/azure/cloudspace")]
    public async Task<IResult> Post(CreateAzureCloudspaceRequest req)
    {
        var exists = true;
        var uri = $"https://{ApiHost}/azure/cloudspace/{req.Name}";
        
        // Ensure cloudspace name is 'default' only
        if (req.Name.ToLower() !=  "default")
        {
            return Results.Problem($"Cloudspace name was {req.Name}. Only 'default' is accepted as a cloudspace name");
        }
        
        // If Cloudspace doesn't exist, create it.
        if (Ark.AzureCloudspaces.Count == 0)
        {
            Ark.AzureCloudspaces = new List<AzureCloudspace> { new AzureCloudspace()};
            exists= false;
        } 
 
        // Get Azure Cloudspace from Ark
        var acs = Ark.AzureCloudspaces[0];

        // Add Environments to cloudspace and Save to DB
        req.Environments.ToList().ForEach(x => acs.AddSpoke(x));
        _arkRepo.Save(Ark);

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
        // Ensure cloudspace name is 'default' only
        if (req.Name.ToLower() != "default")
        {
            return Results.Problem($"Cloudspace name was {req.Name}. Only 'default' is accepted as a cloudspace name");
        }
        var uri = $"https://{ApiHost}/azure/cloudspace/{req.Name}";

        //If Cloudspace doesn't exist, return OK
        if (Ark.AzureCloudspaces.Count == 0)
        {
            return Results.Ok("No cloudspace to delete");
        }

        var acs = Ark.AzureCloudspaces[0];

        //// Send Message to queue
        var subject = req.GetType().Name;
        await _asbService.Sender.SendMessageAsync(new ServiceBusMessage(acs.ToString()) { Subject = subject });

        // Remove cloudspace and save in DB
        Ark.AzureCloudspaces.RemoveAll(x => x.Name == req.Name.ToLower());
        _arkRepo.Save(Ark);

        // Send message to worker to run the pulumi handler program & return a 202 (Accepted)
        return Results.Accepted();
    }
}
