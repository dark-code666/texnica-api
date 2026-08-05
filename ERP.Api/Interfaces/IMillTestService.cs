using ERP.Api.Dtos;

namespace ERP.Api.Interfaces;

public interface IMillTestService
{
    Task<IEnumerable<MillTestDto>> GetAllAsync();
    Task<MillTestDto?> GetByIdAsync(int id);
    Task<IEnumerable<MillTestDto>> GetByFabricPOAsync(int fabricPOId);
    Task<IEnumerable<MillTestDto>> GetByFgpoAsync(int fgpoId);
    Task<MillTestDto> CreateAsync(CreateMillTestDto dto);
    Task<bool> UpdateAsync(int id, UpdateMillTestDto dto);
    Task<bool> DeleteAsync(int id);
    Task<PagedResultDto<MillTestDto>> GetPagedAsync(int page, int pageSize, string? search, string? sortBy, string? sortOrder, string? fabricPO, string? fgpo, string? lotNumber, string? testResult);
}
