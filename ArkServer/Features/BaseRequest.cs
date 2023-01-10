namespace ArkServer.Features;

public class BaseRequest
{
    public DateTime DtTimeStamp { get; }
    public BaseRequest()
    {
        DtTimeStamp = DateTime.UtcNow;
    }
}
