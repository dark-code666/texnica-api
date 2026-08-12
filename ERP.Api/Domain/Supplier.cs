namespace ERP.Api.Domain;

public class Supplier : BaseEntity
{
    public string Name { get; set; } = null!;
    public string? SupplierCode { get; set; }
    public string? Category { get; set; }
    public string? Contact { get; set; }
    public string? Phone { get; set; }
    public string? Email { get; set; }
    public string? Address { get; set; }
    public string? Remarks { get; set; }
}
