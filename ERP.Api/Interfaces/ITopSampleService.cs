using ERP.Api.Dtos;

namespace ERP.Api.Interfaces;

public interface ITopSampleService
{
    Task<IEnumerable<TopSampleDto>> GetAllAsync();
    Task<TopSampleDto?> GetByIdAsync(int id);
    Task<IEnumerable<TopSampleDto>> GetByFgpoAsync(int fgpoId);
    Task<TopSampleDto> CreateAsync(CreateTopSampleDto dto);
    Task<bool> UpdateAsync(int id, UpdateTopSampleDto dto);
    Task<bool> DeleteAsync(int id);
    Task<PagedResultDto<TopSampleDto>> GetPagedAsync(int page, int pageSize, string? search, string? sortBy, string? sortOrder, string? fgpo, string? status);
}
