using ERP.Api.Dtos;

namespace ERP.Api.Interfaces;

public interface IFabricReceivingService
{
    Task<IEnumerable<FabricReceivingDto>> GetAllAsync();
    Task<FabricReceivingDto?> GetByIdAsync(int id);
    Task<IEnumerable<FabricReceivingDto>> GetByFabricPOAsync(int fabricPOId);
    Task<IEnumerable<FabricReceivingDto>> GetByFgpoAsync(int fgpoId);
    Task<FabricReceivingDto> CreateAsync(CreateFabricReceivingDto dto);
    Task<bool> UpdateAsync(int id, UpdateFabricReceivingDto dto);
    Task<bool> DeleteAsync(int id);
    Task<PagedResultDto<FabricReceivingDto>> GetPagedAsync(int page, int pageSize, string? search, string? sortBy, string? sortOrder, string? fabricPO, string? fgpo, string? status);
}
