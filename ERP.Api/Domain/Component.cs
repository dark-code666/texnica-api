namespace ERP.Api.Domain;

public class Component : BaseEntity
{
    public string ComponentCode { get; set; } = null!;
    public string? Description { get; set; }
}
