using Ark.Server.Services;
using Microsoft.AspNetCore.Mvc;
using Ark.Server.Entities;
using ServiceStack.OrmLite;
using PulumiApi;
using Azure.Messaging.ServiceBus;
using Ark.Base;

namespace Ark.Server.Features.ManagedCluster;

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
    public async Task<IResult> CreateManagedCluster(CreateManagedClusterRequest req)
    {
        // Generate new cloudspace model    
        var aks = new AzureManagedCluster();

        var csName = req.Args.Cloudspace.Name;

        Console.WriteLine(req.ToString());

        Console.WriteLine($"csName:{csName}");

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

        var client = new ApiClient();
        var arkConfig = Config.Read();
        var orgName = arkConfig.PulumiDefaultOrg;
        var projectName = "azurecloudspace";
        var stackName = "dev";

        var result = await client.GetStackState(orgName, projectName, stackName);

        if (result.Deployment == null)
        {
            return Results.BadRequest($"{csName} does not exist");
        }

        var targetEnv = req.Args.Cloudspace.Environment;
        string targetVnet="";
        string targetRg = "";
        if (targetEnv != null)
        {
                targetVnet = result.Deployment.GetAzureVnet(targetEnv) ?? "";
                targetRg = result.Deployment.GetAzureVnetRg(targetEnv) ?? "";
        }


        var cluster = new AzureManagedCluster
        {
            VnetResourceGroup = targetRg,
            VnetName = targetVnet,
            SubNetName = "snet-tier2-aks",
            Aks = new Aks
            {
                Name = "aks01",
                ResourceGroup = "ark-rg-aks01",
                ServicePrincipal = "ark-k8s-sp",
                NetworkProfile = new NetworkProfile()
            }
        };


        // Queue request with worker
        var subject = req.GetType().Name;
        await _asbService.Sender.SendMessageAsync(new ServiceBusMessage(cluster.ToString()) { Subject = subject });

        // Respond with an Id
        return Results.Accepted("ok", cluster);

    }
}
