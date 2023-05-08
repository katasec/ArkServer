using PulumiApi.Models;
using System.Text.Json;

namespace Ark.Server.Features;

//public class BaseRequest
//{
//    public string Id { get; set; }
//    public string? UpdateId { get; set; }
//    public DateTime DtTimeStamp { get; }
//    public BaseRequest()
//    {
//        Id = Guid.NewGuid().ToString();
//        DtTimeStamp = DateTime.UtcNow;
//    }
//}

//public static class BaseRequestExtensions
//{
//    public static string ToJson<T>(this T self) where T : BaseRequest?
//    {
//        return JsonSerializer.Serialize(self, new JsonSerializerOptions { WriteIndented = true });
//    }
//}