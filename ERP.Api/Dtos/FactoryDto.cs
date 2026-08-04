namespace ERP.Api.Dtos;

public class FactoryDto
{
    public int ID { get; set; }
    public string Name { get; set; } = null!;
    public string? Location { get; set; }
    public string? Contact { get; set; }
    public string? Phone { get; set; }
    public string? Email { get; set; }
    public bool Active { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}
