using ERP.Api.Dtos;

namespace ERP.Api.Interfaces;

public interface ISewingProductionService
{
    Task<IEnumerable<SewingProductionDto>> GetAllAsync();
    Task<SewingProductionDto?> GetByIdAsync(int id);
    Task<IEnumerable<SewingProductionDto>> GetByFgpoAsync(int fgpoId);
    Task<SewingProductionDto> CreateAsync(CreateSewingProductionDto dto);
    Task<bool> UpdateAsync(int id, UpdateSewingProductionDto dto);
    Task<bool> DeleteAsync(int id);
    Task<PagedResultDto<SewingProductionDto>> GetPagedAsync(int page, int pageSize, string? search, string? sortBy, string? sortOrder, string? fgpo, string? line);
}
