using Microsoft.AspNetCore.Mvc;

namespace ArkServer.Features.ManagedCluster;

[ApiController]
public class ManagedClusterController : ControllerBase
{
    [HttpGet]
    [Route("/azure/managedcluster/{name}")]
    public IResult Index(string name)
    {
        return Results.Ok(name);
    }
}
