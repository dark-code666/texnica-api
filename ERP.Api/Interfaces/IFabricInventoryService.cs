using ERP.Api.Dtos;

namespace ERP.Api.Interfaces;

public interface IFabricInventoryService
{
    Task<IEnumerable<FabricInventoryDto>> GetAllAsync();
    Task<FabricInventoryDto?> GetByIdAsync(int id);
    Task<IEnumerable<FabricInventoryDto>> GetByFgpoAsync(int fgpoId);
    Task<FabricInventoryDto> CreateAsync(CreateFabricInventoryDto dto);
    Task<bool> UpdateAsync(int id, UpdateFabricInventoryDto dto);
    Task<bool> DeleteAsync(int id);
    Task<PagedResultDto<FabricInventoryDto>> GetPagedAsync(int page, int pageSize, string? search, string? sortBy, string? sortOrder, string? fgpo, string? status);
}
