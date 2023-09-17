using Ark.ServiceModel;

namespace Ark.Server.Routes.Hello;

public class HelloSuccessRequest : BaseRequest
{
    public string? Message { get; set; }
}

public class HelloSuccessResponse
{
    public string? Id { get; set; }
}