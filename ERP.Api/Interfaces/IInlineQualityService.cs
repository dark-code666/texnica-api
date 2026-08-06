using ERP.Api.Dtos;

namespace ERP.Api.Interfaces;

public interface IInlineQualityService
{
    Task<IEnumerable<InlineQualityDto>> GetAllAsync();
    Task<InlineQualityDto?> GetByIdAsync(int id);
    Task<IEnumerable<InlineQualityDto>> GetByFgpoAsync(int fgpoId);
    Task<IEnumerable<InlineQualityDto>> GetByLineAsync(string line);
    Task<InlineQualityDto> CreateAsync(CreateInlineQualityDto dto);
    Task<bool> UpdateAsync(int id, UpdateInlineQualityDto dto);
    Task<bool> DeleteAsync(int id);
    Task<PagedResultDto<InlineQualityDto>> GetPagedAsync(int page, int pageSize, string? search, string? sortBy, string? sortOrder, string? fgpo, string? line, string? result);
}
