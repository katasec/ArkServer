using ArkServer.Entities.Azure;
using ArkServer.Repositories;
using ArkServer.Services;
using Azure.Messaging.ServiceBus;
using Microsoft.AspNetCore.Mvc;
using ServiceStack.Data;
using ServiceStack.OrmLite;

namespace ArkServer.Features.Cloudspace;

[ApiController]
public class AzureCloudspaceController : ControllerBase
{
    private readonly AsbService _asbService;
    private readonly Ark Ark;
    private ILogger<AzureCloudspaceController> _logger;
    private string ApiHost => HttpContext.Request.Host.ToString();
    private ICloudspaceRepo _repo;
    private System.Data.IDbConnection _db;

    public AzureCloudspaceController(ILogger<AzureCloudspaceController> logger,AsbService asbService,ICloudspaceRepo repo, OrmLiteConnectionFactory dbFactory)
    {
        _asbService = asbService;
        _logger = logger;
        _repo = repo;
        Ark= repo.Get();
        _db = dbFactory.Open();
    }

    [HttpPost]
    [Route("/azure/cloudspace")]
    public async Task<IResult> CreateCloudSpace(CreateAzureCloudspaceRequest req)
    {
        //var uri = $"https://{ApiHost}/azure/cloudspace/{req.Name.ToLower()}";

        // Generate a model for the AzureCloudSpace
        // that needs to be created
        if (Ark.AzureCloudspaces.Count == 0)
        {
            Ark.AzureCloudspaces = new List<AzureCloudspace> { new AzureCloudspace() };
        }

        
        // Assign a request ID to the generated model
        var acs = Ark.AzureCloudspaces[0];
        acs.Id = Guid.NewGuid().ToString();


        //// Add Environments to cloudspace and Save to DB
        //req.Environments.ToList().ForEach(x => acs.AddSpoke(x));
        //_arkRepo.Save(Ark);

        // Send model to queue so worker can create it.
        var subject = req.GetType().Name;
        await _asbService.Sender.SendMessageAsync(new ServiceBusMessage(acs.ToString()) { Subject = subject });


        // Respond with an Id
        return Results.Accepted("ok",new CreateAzureCloudspaceResponse
        {
            Id= acs.Id,
            Name= acs.Name,
        });

    }


    [HttpDelete]
    [Route("/azure/cloudspace")]
    public async Task<IResult> Delete(DeleteAzureCloudspaceRequest req)
    {
        var uri = $"https://{ApiHost}/azure/cloudspace/{req.Name.ToLower()}";

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
        //Ark.AzureCloudspaces.RemoveAll(x => x.Name == req.Name.ToLower());
        //_arkRepo.Save(Ark);

        // Send message to worker to run the pulumi handler program & return a 202 (Accepted)
        return Results.Accepted();
    }

    [HttpGet]
    [Route("/azure/cloudspaces")]
    public IResult GetCloudspace()
    {
        //return Results.Ok(Ark.AzureCloudspaces);
        return Results.Ok(_db.LoadSelect<AzureCloudspace>());
    }

    [HttpPost]
    [Route("/azure/cloudspaces/{name}")]
    public  IResult UpdateCloudspace(string name, [FromBody] AzureCloudspace azureCloudpace)
    {
        var cloudspace = Ark.AzureCloudspaces.Where(x => x.Name == name );
        if (cloudspace != null)
        {
            return Results.Conflict(cloudspace);
        }

        
        Ark.AzureCloudspaces.Add(azureCloudpace);
        _repo.Save(Ark);

        return Results.Ok(Ark.AzureCloudspaces);
    }
}



