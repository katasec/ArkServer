using YamlDotNet.Serialization;

namespace Ark.ServiceModel;

public class BaseRequest
{
    [YamlMember(Alias = "kind")]
    public string Kind { get; set; } = "";
    public string Id { get; set; }
    public string? UpdateId { get; set; }
    public DateTime DtTimeStamp { get; }
    public BaseRequest()
    {
        Id = Guid.NewGuid().ToString();
        DtTimeStamp = DateTime.UtcNow;
    }
}

