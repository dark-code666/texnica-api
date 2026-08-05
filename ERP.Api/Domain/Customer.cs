namespace ERP.Api.Domain;

public class Customer : BaseEntity
{
    public string Name { get; set; } = null!;
    public string? Contact { get; set; }
    public string? Phone { get; set; }
    public string? Email { get; set; }
    public string? Address { get; set; }
}
