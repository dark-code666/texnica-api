using ERP.Api.Dtos;

namespace ERP.Api.Interfaces;

public interface IFabricShipmentService
{
    Task<IEnumerable<FabricShipmentDto>> GetAllAsync();
    Task<FabricShipmentDto?> GetByIdAsync(int id);
    Task<IEnumerable<FabricShipmentDto>> GetByFabricPOAsync(int fabricPOId);
    Task<IEnumerable<FabricShipmentDto>> GetByFgpoAsync(int fgpoId);
    Task<IEnumerable<FabricShipmentDto>> GetByLotAsync(string lotNumber);
    Task<FabricShipmentDto> CreateAsync(CreateFabricShipmentDto dto);
    Task<bool> UpdateAsync(int id, UpdateFabricShipmentDto dto);
    Task<bool> DeleteAsync(int id);
    Task<PagedResultDto<FabricShipmentDto>> GetPagedAsync(int page, int pageSize, string? search, string? sortBy, string? sortOrder, string? fabricPO, string? fgpo, string? lotNumber, string? status);
}
