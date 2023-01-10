using Microsoft.Extensions.Diagnostics.HealthChecks;
using System.Diagnostics;

namespace ArkServer.Entities.Azure;

public class AzureCloudspace
{
    public string Name { get; set; } = "";
    public VNetInfo Hub { get; set; } 
    public List<VNetInfo>? Env { get; set; }
}


//public record AzureCloudspace(string Name, List<Env> Envs);
