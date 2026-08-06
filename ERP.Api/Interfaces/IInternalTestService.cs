using ERP.Api.Dtos;

namespace ERP.Api.Interfaces;

public interface IInternalTestService
{
    Task<IEnumerable<InternalTestDto>> GetAllAsync();
    Task<InternalTestDto?> GetByIdAsync(int id);
    Task<IEnumerable<InternalTestDto>> GetByFabricPOAsync(int fabricPOId);
    Task<IEnumerable<InternalTestDto>> GetByFgpoAsync(int fgpoId);
    Task<InternalTestDto> CreateAsync(CreateInternalTestDto dto);
    Task<bool> UpdateAsync(int id, UpdateInternalTestDto dto);
    Task<bool> DeleteAsync(int id);
    Task<PagedResultDto<InternalTestDto>> GetPagedAsync(int page, int pageSize, string? search, string? sortBy, string? sortOrder, string? fabricPO, string? fgpo, string? testResult);
}
