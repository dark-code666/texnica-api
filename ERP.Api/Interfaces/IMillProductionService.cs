using ERP.Api.Dtos;

namespace ERP.Api.Interfaces;

public interface IMillProductionService
{
    Task<IEnumerable<MillProductionDto>> GetAllAsync();
    Task<MillProductionDto?> GetByIdAsync(int id);
    Task<IEnumerable<MillProductionDto>> GetByFabricPOAsync(int fabricPOId);
    Task<IEnumerable<MillProductionDto>> GetByFgpoAsync(int fgpoId);
    Task<MillProductionDto> CreateAsync(CreateMillProductionDto dto);
    Task<bool> UpdateAsync(int id, UpdateMillProductionDto dto);
    Task<bool> DeleteAsync(int id);
    Task<PagedResultDto<MillProductionDto>> GetPagedAsync(int page, int pageSize, string? search, string? sortBy, string? sortOrder, string? fabricPO, string? fgpo, string? supplier, string? status);
}
