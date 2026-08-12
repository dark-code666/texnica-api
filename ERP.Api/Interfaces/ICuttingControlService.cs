using ERP.Api.Dtos;

namespace ERP.Api.Interfaces;

public interface ICuttingControlService
{
    Task<IEnumerable<CuttingControlDto>> GetAllAsync();
    Task<CuttingControlDto?> GetByIdAsync(int id);
    Task<IEnumerable<CuttingControlDto>> GetByFgpoAsync(int fgpoId);
    Task<CuttingControlDto> CreateAsync(CreateCuttingControlDto dto);
    Task<bool> UpdateAsync(int id, UpdateCuttingControlDto dto);
    Task<bool> DeleteAsync(int id);
    Task<PagedResultDto<CuttingControlDto>> GetPagedAsync(int page, int pageSize, string? search, string? sortBy, string? sortOrder, string? fgpo, string? status);
}
