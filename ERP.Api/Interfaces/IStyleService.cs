using ERP.Api.Dtos;

namespace ERP.Api.Interfaces;

public interface IStyleService
{
    Task<IEnumerable<StyleDto>> GetAllAsync();
    Task<StyleDto?> GetByIdAsync(int id);
    Task<StyleDto> CreateAsync(CreateStyleDto dto);
    Task<bool> UpdateAsync(int id, UpdateStyleDto dto);
    Task<bool> DeleteAsync(int id);
    Task<IEnumerable<StyleDto>> SearchAsync(string? term);
    Task<PagedResultDto<StyleDto>> GetPagedAsync(int page, int pageSize, string? search, string? sortBy, string? sortOrder);
}
