using ArkServer.Services;
using Microsoft.AspNetCore.Mvc;
using ArkServer.Entities.Azure;
using ServiceStack.OrmLite;

namespace ArkServer.Features.ManagedCluster;

[ApiController]
public class ManagedClusterController : ControllerBase
{
    private readonly AsbService _asbService;
    private ILogger<ManagedClusterController> _logger;

    private string ApiHost => HttpContext.Request.Host.ToString();
    private System.Data.IDbConnection _db;

    public ManagedClusterController(ILogger<ManagedClusterController> logger, AsbService asbService, OrmLiteConnectionFactory dbFactory)
    {
        _asbService = asbService;
        _logger = logger;
        _db = dbFactory.Open();
    }

    [HttpGet]
    [Route("/azure/managedcluster/{name}")]
    public IResult Index(string name)
    {
        return Results.Ok(name);
    }

    [HttpPost]
    [Route("/azure/managedcluster")]
    public IResult CreateManagedCluster(CreateManagedClusterRequest req)
    {
        // Generate new cloudspace model    
        var aks = new AzureManagedCluster();

        var csName = req.Args.Cloudspace.Name;

        var cloudspace = _db.LoadSelect<AzureCloudspace>(x => x.Name == csName).FirstOrDefault();
        if (cloudspace == null)
        {
            return Results.BadRequest($"Cloudspace {csName} does not exist"); // Conflict(cloudspace[0]);
        }

        var vnet = cloudspace.Spokes.Where(x => x.Name == req.Args.Cloudspace.Environment).FirstOrDefault();
        if (vnet == null)
        {
            return Results.BadRequest($"Environment {req.Args.Cloudspace.Environment} does not exist in cloudspace {csName}");
        }

        //var envName = vnet.Name;

        //aks.VnetName = vnet.Name;
        //aks.VnetResourceGroup = vnet.r;

        //// Add vnets to cloudspace
        //req.Environments.ForEach(spoke => acs.AddSpoke(spoke));

        //// Queue request with worker
        //var subject = req.GetType().Name;
        //await _asbService.Sender.SendMessageAsync(new ServiceBusMessage(acs.ToString()) { Subject = subject });

        //// Respond with an Id
        //return Results.Accepted("ok", new CreateManagedClusterResponse
        //{
        //    Id = acs.Id,
        //    Name = acs.Name,
        //});

        return null;
    }
}
