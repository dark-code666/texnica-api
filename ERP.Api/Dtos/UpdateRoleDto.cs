namespace ERP.Api.Dtos;

public class UpdateRoleDto
{
    public string Name { get; set; } = null!;
    public string Description { get; set; } = null!;
    public List<int>? PermissionIds { get; set; }
}
