namespace ERP.Api.Domain;

public class Permission : BaseEntity
{
    public string Name { get; set; } = null!;
    public string Description { get; set; } = null!;
    public string Module { get; set; } = null!; // e.g., "Dashboard", "Production", "Admin"
    
    // Navigation properties
    public ICollection<RolePermission> RolePermissions { get; set; } = new List<RolePermission>();
}
