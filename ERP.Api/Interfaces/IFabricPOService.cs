using ERP.Api.Dtos;

namespace ERP.Api.Interfaces;

public interface IFabricPOService
{
    Task<IEnumerable<FabricPODto>> GetAllAsync();
    Task<FabricPODto?> GetByIdAsync(int id);
    Task<IEnumerable<FabricPODto>> GetByFgpoAsync(int fgpoId);
    Task<FabricPODto> CreateAsync(CreateFabricPODto dto);
    Task<bool> UpdateAsync(int id, UpdateFabricPODto dto);
    Task<bool> DeleteAsync(int id);
    Task<PagedResultDto<FabricPODto>> GetPagedAsync(int page, int pageSize, string? search, string? sortBy, string? sortOrder, string? fgpo, string? supplier, string? fabricMill, string? fabricComponent, string? poStatus);
}
