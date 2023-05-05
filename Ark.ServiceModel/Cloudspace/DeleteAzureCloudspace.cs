using System.Text.Json;

namespace Ark.ServiceModel.Cloudspace;

public class DeleteAzureCloudspaceRequest : BaseRequest
{

    public string  Name { get; init; } = "default";
    public override string ToString()
    {
        return JsonSerializer.Serialize(this, new JsonSerializerOptions { WriteIndented = true});
    }
}

public class DeleteAzureCloudspaceResponse: BaseRequest
{

}
