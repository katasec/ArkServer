using System.Text.Json;
using YamlDotNet.Serialization;

namespace Ark.ServiceModel.Cloudspace;

public class CreateAzureCloudspaceRequest : BaseRequest
{

    [YamlMember(Alias = "name")]
    public string  Name { get; init; } = "default";

    [YamlMember(Alias = "environments")]
    public required List<string> Environments { get; set; }

    public override string ToString()
    {
        return JsonSerializer.Serialize(this, new JsonSerializerOptions { WriteIndented = true});
    }
}

public class CreateAzureCloudspaceResponse
{
    public string? Id { get; set;}
    public string  Name { get; init; } = "default";
}
