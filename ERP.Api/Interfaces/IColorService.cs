using ERP.Api.Dtos;

namespace ERP.Api.Interfaces;

public interface IColorService
{
    Task<IEnumerable<ColorDto>> GetAllAsync();
    Task<ColorDto?> GetByIdAsync(int id);
    Task<ColorDto> CreateAsync(CreateColorDto dto);
    Task<bool> UpdateAsync(int id, UpdateColorDto dto);
    Task<bool> DeleteAsync(int id);
    Task<IEnumerable<ColorDto>> SearchAsync(string? term);
    Task<PagedResultDto<ColorDto>> GetPagedAsync(int page, int pageSize, string? search, string? sortBy, string? sortOrder);
}
