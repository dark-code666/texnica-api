namespace ERP.Api.Domain;

public class Factory : BaseEntity
{
    public string Name { get; set; } = null!;
    public string? Location { get; set; }
    public string? Contact { get; set; }
    public string? Phone { get; set; }
    public string? Email { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}
