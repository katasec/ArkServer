using System.Text.Json;
using System.Text.Json.Serialization;

namespace ArkServer.Entities;

public class AzureManagedCluster : BaseEntity
{
    [JsonPropertyName("VnetResourceGroup")]
    public string? VnetResourceGroup { get; set; }

    [JsonPropertyName("SubNetName")]
    public string? SubNetName { get; set; }

    [JsonPropertyName("VnetName")]
    public string? VnetName { get; set; }

    [JsonPropertyName("Aks")]
    public Aks Aks { get; set; } = new();

    public override string ToString()
    {
        return JsonSerializer.Serialize(this, new JsonSerializerOptions { WriteIndented = true });
    }
}

public partial class Aks
{
    [JsonPropertyName("Name")]
    public string? Name { get; set; }

    [JsonPropertyName("ResourceGroup")]
    public string? ResourceGroup { get; set; }

    [JsonPropertyName("ServicePrincipal")]
    public string? ServicePrincipal { get; set; }

    [JsonPropertyName("VmSize")]
    public string VmSize { get; set; } = "Standard_B4ms";

    [JsonPropertyName("EnablePrivateCluster")]
    public bool EnablePrivateCluster { get; set; } = true;

    [JsonPropertyName("NetworkProfile")]
    public NetworkProfile NetworkProfile { get; set; } = new();
}

public partial class NetworkProfile
{
    [JsonPropertyName("NetworkPlugin")]
    public string NetworkPlugin { get; set; } = "azure";

    [JsonPropertyName("NetworkPolicy")]
    public string NetworkPolicy { get; set; } = "calico";

    [JsonPropertyName("DockerBridgeCidr")]
    public string DockerBridgeCidr { get; set; } = "172.17.0.0/16";
}
