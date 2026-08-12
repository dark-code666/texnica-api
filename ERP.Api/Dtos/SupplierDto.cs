namespace ERP.Api.Dtos;

public class SupplierDto
{
    public int ID { get; set; }
    public string Name { get; set; } = null!;
    public string? SupplierCode { get; set; }
    public string? Category { get; set; }
    public string? Contact { get; set; }
    public string? Phone { get; set; }
    public string? Email { get; set; }
    public string? Address { get; set; }
    public string? Remarks { get; set; }
    public bool Active { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}
