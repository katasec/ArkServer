using System.Text.Json;
using YamlDotNet;
using YamlDotNet.Serialization;


namespace Ark.Server.Entities;


public static class BaseEntityExtensions
{
    /// <summary>
    /// Returns a JSON representation of the object
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="request"></param>
    /// <returns></returns>
    public static string JsonString<T>(this T request) where T : BaseEntity
    {
        return JsonSerializer.Serialize(request);
    }

    /// <summary>
    /// Returns a YAML representation of the object
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="request"></param>
    /// <returns></returns>
    public static string YamlString<T>(this T request) where T : BaseEntity
    {
        var YamlSerializer = new SerializerBuilder().Build();
        return YamlSerializer.Serialize(request);
    }

}
