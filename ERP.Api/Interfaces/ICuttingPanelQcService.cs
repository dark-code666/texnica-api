using ERP.Api.Dtos;

namespace ERP.Api.Interfaces;

public interface ICuttingPanelQcService
{
    Task<IEnumerable<CuttingPanelQcDto>> GetAllAsync();
    Task<CuttingPanelQcDto?> GetByIdAsync(int id);
    Task<IEnumerable<CuttingPanelQcDto>> GetByFgpoAsync(int fgpoId);
    Task<CuttingPanelQcDto> CreateAsync(CreateCuttingPanelQcDto dto);
    Task<bool> UpdateAsync(int id, UpdateCuttingPanelQcDto dto);
    Task<bool> DeleteAsync(int id);
    Task<PagedResultDto<CuttingPanelQcDto>> GetPagedAsync(int page, int pageSize, string? search, string? sortBy, string? sortOrder, string? fgpo, string? result);
}
