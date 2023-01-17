namespace ArkServer.Features;

public class BaseRequest
{
    public string RequestType {get;set; }
    public DateTime DtTimeStamp { get; }
    public BaseRequest()
    {
        DtTimeStamp = DateTime.UtcNow;
    }
}
