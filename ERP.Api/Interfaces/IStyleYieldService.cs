using ERP.Api.Dtos;

namespace ERP.Api.Interfaces;

public interface IStyleYieldService
{
    Task<IEnumerable<StyleYieldDto>> GetAllAsync();
    Task<StyleYieldDto?> GetByIdAsync(int id);
    Task<StyleYieldDto> CreateAsync(CreateStyleYieldDto dto);
    Task<bool> UpdateAsync(int id, UpdateStyleYieldDto dto);
    Task<bool> DeleteAsync(int id);
    Task<IEnumerable<StyleYieldDto>> GetByStyleAsync(int styleId);
    Task<PagedResultDto<StyleYieldDto>> GetPagedAsync(int page, int pageSize, string? search, string? sortBy, string? sortOrder);
}
