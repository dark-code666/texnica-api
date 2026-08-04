namespace ERP.Api.Dtos;

public class CustomerDto
{
    public int ID { get; set; }
    public string Name { get; set; } = null!;
    public string? Contact { get; set; }
    public string? Phone { get; set; }
    public string? Email { get; set; }
    public string? Address { get; set; }
    public bool Active { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}
