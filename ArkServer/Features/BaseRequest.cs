namespace ArkServer.Features;

public class BaseRequest
{
    public string Id { get; set; }
    public string? UpdateId { get; set; }
    public DateTime DtTimeStamp { get; }
    public BaseRequest()
    {
        Id = Guid.NewGuid().ToString();
        DtTimeStamp = DateTime.UtcNow;
    }
}
