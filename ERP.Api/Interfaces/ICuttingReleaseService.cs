using ERP.Api.Dtos;

namespace ERP.Api.Interfaces;

public interface ICuttingReleaseService
{
    Task<IEnumerable<CuttingReleaseDto>> GetAllAsync();
    Task<CuttingReleaseDto?> GetByIdAsync(int id);
    Task<IEnumerable<CuttingReleaseDto>> GetByFgpoAsync(int fgpoId);
    Task<CuttingReleaseDto> CreateAsync(CreateCuttingReleaseDto dto);
    Task<bool> UpdateAsync(int id, UpdateCuttingReleaseDto dto);
    Task<bool> DeleteAsync(int id);
    Task<PagedResultDto<CuttingReleaseDto>> GetPagedAsync(int page, int pageSize, string? search, string? sortBy, string? sortOrder, string? fgpo, string? status);
}
