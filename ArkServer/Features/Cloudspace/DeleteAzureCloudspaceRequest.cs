using System.Text.Json;
namespace ArkServer.Features.Cloudspace;

public class DeleteAzureCloudspaceRequest : BaseRequest
{

    public string  Name { get; init; } = "default";
    public override string ToString()
    {
        return JsonSerializer.Serialize(this, new JsonSerializerOptions { WriteIndented = true});
    }
}
