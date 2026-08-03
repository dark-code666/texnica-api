namespace ERP.Api.Dtos;

public class RoleDto
{
    public int ID { get; set; }
    public string Name { get; set; } = null!;
    public string Description { get; set; } = null!;
    public bool Active { get; set; }
    public List<PermissionDto>? Permissions { get; set; }
}
