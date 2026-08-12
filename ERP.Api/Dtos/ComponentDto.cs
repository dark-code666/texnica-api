namespace ERP.Api.Dtos;

public class ComponentDto
{
    public int ID { get; set; }
    public string ComponentCode { get; set; } = null!;
    public string? Description { get; set; }
    public bool Active { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}
