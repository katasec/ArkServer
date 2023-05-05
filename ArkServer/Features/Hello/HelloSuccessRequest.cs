namespace Ark.Server.Features.Hello;

public class HelloSuccessRequest : BaseRequest
{
    public string? Message { get; set; }
}

public class HelloSuccessResponse
{
    public string? Id { get; set; }
}