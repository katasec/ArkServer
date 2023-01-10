using System.Text.Json;

namespace ArkServer.Entities.Azure;

public class AzureCloudspace
{
    public string Name { get; set; } = "";
    public VNetInfo Hub { get; set; } 
    public List<VNetInfo>? Env { get; set; }

    public override string ToString()
    {
        return JsonSerializer.Serialize(this, new JsonSerializerOptions{WriteIndented = true});
    }
}


//public record AzureCloudspace(string Name, List<Env> Envs);
