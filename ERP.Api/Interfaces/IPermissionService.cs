namespace ERP.Api.Interfaces;

using System.Threading.Tasks;
using ERP.Api.Dtos;

public interface IPermissionService
{
    Task<List<PermissionDto>> GetAllPermissionsAsync();
    Task<List<PermissionDto>> GetPermissionsByModuleAsync(string module);
    Task<PermissionDto?> GetPermissionByIdAsync(int id);
    Task<PermissionDto> CreatePermissionAsync(CreatePermissionDto dto);
    Task<bool> DeletePermissionAsync(int id);
}
