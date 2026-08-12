using ERP.Api.Dtos;

namespace ERP.Api.Interfaces;

public interface IPpSampleService
{
    Task<IEnumerable<PpSampleDto>> GetAllAsync();
    Task<PpSampleDto?> GetByIdAsync(int id);
    Task<IEnumerable<PpSampleDto>> GetByFgpoAsync(int fgpoId);
    Task<PpSampleDto> CreateAsync(CreatePpSampleDto dto);
    Task<bool> UpdateAsync(int id, UpdatePpSampleDto dto);
    Task<bool> DeleteAsync(int id);
    Task<PagedResultDto<PpSampleDto>> GetPagedAsync(int page, int pageSize, string? search, string? sortBy, string? sortOrder, string? fgpo, string? status);
}
