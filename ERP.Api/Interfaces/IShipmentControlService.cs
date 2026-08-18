using ERP.Api.Dtos;

namespace ERP.Api.Interfaces;

public interface IShipmentControlService
{
    Task<IEnumerable<ShipmentControlDto>> GetAllAsync();
    Task<ShipmentControlDto?> GetByIdAsync(int id);
    Task<IEnumerable<ShipmentControlDto>> GetByFgpoAsync(int fgpoId);
    Task<ShipmentControlDto> CreateAsync(CreateShipmentControlDto dto);
    Task<bool> UpdateAsync(int id, UpdateShipmentControlDto dto);
    Task<bool> DeleteAsync(int id);
    Task<PagedResultDto<ShipmentControlDto>> GetPagedAsync(int page, int pageSize, string? search, string? sortBy, string? sortOrder, string? fgpo, string? status);
}
