using ERP.Api.Dtos;

namespace ERP.Api.Interfaces;

public interface IPackingControlService
{
    Task<IEnumerable<PackingControlDto>> GetAllAsync();
    Task<PackingControlDto?> GetByIdAsync(int id);
    Task<IEnumerable<PackingControlDto>> GetByFgpoAsync(int fgpoId);
    Task<PackingControlDto> CreateAsync(CreatePackingControlDto dto);
    Task<bool> UpdateAsync(int id, UpdatePackingControlDto dto);
    Task<bool> DeleteAsync(int id);
    Task<PagedResultDto<PackingControlDto>> GetPagedAsync(int page, int pageSize, string? search, string? sortBy, string? sortOrder, string? fgpo);
}
