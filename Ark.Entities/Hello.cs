namespace Ark.Entities;

public class HelloSuccess : BaseEntity
{
    public string? Message { get; set; }
    public override string ToString()
    {
        return JsonSerializer.Serialize(this, new JsonSerializerOptions { WriteIndented = true });
    }
}

public class HelloFail : BaseEntity
{
    public string? Message { get; set; }

    public override string ToString()
    {
        return JsonSerializer.Serialize(this, new JsonSerializerOptions { WriteIndented = true });
    }
}