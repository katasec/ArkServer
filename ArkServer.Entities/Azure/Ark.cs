using System.Text.Json;

namespace ArkServer.Entities.Azure;

public class Ark
{
    public List<AzureCloudspace> AzureCloudspaces {get; set; } = new List<AzureCloudspace>();

    public override string ToString()
    {
        return JsonSerializer.Serialize(this, new JsonSerializerOptions{WriteIndented = true});
    }
}
