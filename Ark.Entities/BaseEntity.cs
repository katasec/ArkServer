namespace Ark.Entities;

public class BaseEntity
{

    [JsonPropertyName("Kind")]
    public string Kind { get; set; } = "";
    
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
