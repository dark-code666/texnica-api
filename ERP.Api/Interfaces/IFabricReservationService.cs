using ERP.Api.Dtos;

namespace ERP.Api.Interfaces;

public interface IFabricReservationService
{
    Task<IEnumerable<FabricReservationDto>> GetAllAsync();
    Task<FabricReservationDto?> GetByIdAsync(int id);
    Task<IEnumerable<FabricReservationDto>> GetByFgpoAsync(int fgpoId);
    Task<FabricReservationDto> CreateAsync(CreateFabricReservationDto dto);
    Task<bool> UpdateAsync(int id, UpdateFabricReservationDto dto);
    Task<bool> DeleteAsync(int id);
    Task<PagedResultDto<FabricReservationDto>> GetPagedAsync(int page, int pageSize, string? search, string? sortBy, string? sortOrder, string? fgpo, string? status);
}
