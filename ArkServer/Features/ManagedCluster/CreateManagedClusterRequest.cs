using System.Diagnostics.Contracts;
using System.Text.Json.Serialization;

namespace Ark.Server.Features.ManagedCluster;

public class CreateManagedClusterRequest
{
    public string? Name { get; set; } = "aks";
    public ManagedClusterArgs Args { get; set; } = new();

}

public class ManagedClusterArgs
{
    public CloudspaceArgs Cloudspace { get; set; } = new();
    public string VmSize { get; set; } = "Standard_B4ms";
}

public class CloudspaceArgs
{
    public string? Name;
    public string? Environment;
}