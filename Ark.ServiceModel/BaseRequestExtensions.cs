using System.Text.Json;

namespace Ark.ServiceModel;

public static class BaseRequestExtensions
{
    public static string Json<T>(this T request) where T : BaseRequest
    {
        return JsonSerializer.Serialize<T>(request);
    }
}
