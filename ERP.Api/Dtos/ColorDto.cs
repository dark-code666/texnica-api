namespace ERP.Api.Dtos;

public class ColorDto
{
    public int ID { get; set; }
    public string ColorName { get; set; } = null!;
    public string? DyeMethod { get; set; }
    public bool Active { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}
