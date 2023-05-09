using Ark.ServiceModel.Cloudspace;
using Ark.Server.Entities;
using Ark.Server.Services;
using Azure.Messaging.ServiceBus;
using Microsoft.AspNetCore.Mvc;
using ServiceStack.OrmLite;
using ServiceStack.OrmLite.Legacy;

namespace Ark.Server.Features.Cloudspace;

[ApiController]
public class AzureCloudspaceController : ControllerBase
{
    private readonly AsbService _asbService;
    private ILogger<AzureCloudspaceController> _logger;
    private string ApiHost => HttpContext.Request.Host.ToString();
    private System.Data.IDbConnection _db;

    public AzureCloudspaceController(ILogger<AzureCloudspaceController> logger,AsbService asbService, OrmLiteConnectionFactory dbFactory)
    {
        _asbService = asbService;
        _logger = logger;
        _db = dbFactory.Open();
    }

    [HttpPost]
    [Route("/azure/cloudspace")]
    public async Task<IResult> CreateCloudSpace(CreateAzureCloudspaceRequest req)
    {

        _logger.LogInformation($"The kind was: {req.Kind}");

        // Generate new cloudspace model    
        var acs = new AzureCloudspace
        {
            Kind = req.Kind,
            Name = req.Name
        };

        // Add vnets to cloudspace
        req.Environments.ForEach(spoke => acs.AddSpoke(spoke));

        // Queue request with worker
        var subject = req.GetType().Name;
        await _asbService.Sender.SendMessageAsync(new ServiceBusMessage(acs.JsonString()) { Subject = subject });

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

        //// Return if not exists
        //var cs = _db.LoadSelect<AzureCloudspace>(x => x.Name == req.Name);
        //if (cs.Count == 0)
        //{
        //    return Results.NotFound(new CreateAzureCloudspaceResponse
        //    {
        //        Name= req.Name,
        //    });
        //}

        //var acs = cs[0];


        // Send Message to queue
        var subject = req.GetType().Name;
        await _asbService.Sender.SendMessageAsync(new ServiceBusMessage(req.ToString()) { Subject = subject });

        // Send message to worker to run the pulumi handler program & return a 202 (Accepted)
        return Results.Accepted();
    }

    [HttpGet]
    [Route("/azure/cloudspaces")]
    public IResult GetCloudspace()
    {
        //return Results.Ok(Ark.AzureCloudspaces);
        //return new HttpResult(_db.LoadSelect<AzureCloudspace>());
        return Results.Ok(_db.LoadSelect<AzureCloudspace>());
    }

    [HttpPost]
    [Route("/azure/cloudspaces/{name}")]
    public  IResult CreateCloudspace(string name, [FromBody] AzureCloudspace azureCloudpace)
    {
        // Return if exists
        var cloudspace =  _db.LoadSelect<AzureCloudspace>(x => x.Name == name);
        if (cloudspace.Count > 0)
        {
            return Results.Conflict(cloudspace[0]);
        }

        // Insert new
        _db.Save(azureCloudpace);
        
        return Results.Ok(azureCloudpace);
    }

    [HttpDelete]
    [Route("/azure/cloudspaces/{name}")]
    public IResult DeleteCloudspace(string name, [FromBody] AzureCloudspace azureCloudpace)
    {
        // Return if exists
        var cloudspace = _db.LoadSelect<AzureCloudspace>(x => x.Name == name);
        if (cloudspace.Count == 0)
        {
            return Results.NotFound(cloudspace[0]);
        }

        // Delete cloudspace
        try
        {
            _db.Delete<AzureCloudspace>(x => x.Id == cloudspace[0].Id);
            _db.Save(azureCloudpace);
        }
        catch (Exception ex)
        {
            return Results.Problem(ex.Message);
        }

        return Results.NoContent();
    }

    [HttpPatch]
    [Route("/azure/cloudspaces/{name}")]
    public  IResult UpdateCloudspace(string name, [FromBody] AzureCloudspace azureCloudpace)
    {
        // Return if not exists
        var cloudspace =  _db.LoadSelect<AzureCloudspace>(x => x.Name == name);
        if (cloudspace.Count == 0)
        {
            return Results.NotFound(azureCloudpace);
        }

        // update existing 
        _db.Update(azureCloudpace);

        return Results.Ok(azureCloudpace);
    }

    [HttpPost]
    [Route("/hello")]
    public List<string> Hello(CreateAzureCloudspaceRequest request)
    {
        return request.Environments;
    }
}


