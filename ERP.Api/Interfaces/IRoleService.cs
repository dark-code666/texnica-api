namespace ERP.Api.Interfaces;

using System.Threading.Tasks;
using ERP.Api.Dtos;

public interface IRoleService
{
    Task<List<RoleDto>> GetAllRolesAsync();
    Task<RoleDto?> GetRoleByIdAsync(int id);
    Task<RoleDto> CreateRoleAsync(CreateRoleDto dto);
    Task<RoleDto> UpdateRoleAsync(int id, UpdateRoleDto dto);
    Task<bool> DeleteRoleAsync(int id);
    Task<bool> AssignPermissionsToRoleAsync(int roleId, List<int> permissionIds);
}
