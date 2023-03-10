using System.Text.Json.Serialization;

namespace ArkServer.Entities.Azure;

public class BaseEntity
{

    //[JsonPropertyName("Id")]
    public string Id { get; set; }

    /// <summary>
    /// Entity creation  is a long running operation. Operation status is interrogated using an entity's UpdateId 
    /// </summary>
    //[JsonPropertyName("UpdateId")]
    public string? UpdateId { get; set; }

    public BaseEntity()
    {
        Id = Guid.NewGuid().ToString();
    }
}
