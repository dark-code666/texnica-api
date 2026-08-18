using ERP.Api.Dtos;

namespace ERP.Api.Interfaces;

public interface IComponentService
{
    Task<IEnumerable<ComponentDto>> GetAllAsync();
    Task<ComponentDto?> GetByIdAsync(int id);
    Task<ComponentDto> CreateAsync(CreateComponentDto dto);
    Task<bool> UpdateAsync(int id, UpdateComponentDto dto);
    Task<bool> DeleteAsync(int id);
    Task<IEnumerable<ComponentDto>> SearchAsync(string? term);
    Task<PagedResultDto<ComponentDto>> GetPagedAsync(int page, int pageSize, string? search, string? sortBy, string? sortOrder);
}
