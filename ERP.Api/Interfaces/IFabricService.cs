using ERP.Api.Dtos;

namespace ERP.Api.Interfaces;

public interface IFabricService
{
    Task<IEnumerable<FabricDto>> GetAllAsync();
    Task<FabricDto?> GetByIdAsync(int id);
    Task<FabricDto> CreateAsync(CreateFabricDto dto);
    Task<bool> UpdateAsync(int id, UpdateFabricDto dto);
    Task<bool> DeleteAsync(int id);
    Task<IEnumerable<FabricDto>> SearchAsync(string? term);
    Task<PagedResultDto<FabricDto>> GetPagedAsync(int page, int pageSize, string? search, string? sortBy, string? sortOrder);
}
