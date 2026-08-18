using ERP.Api.Dtos;

namespace ERP.Api.Interfaces;

public interface IFgpoLineService
{
    Task<IEnumerable<FgpoLineDto>> GetAllAsync();
    Task<FgpoLineDto?> GetByIdAsync(int id);
    Task<FgpoLineDto> CreateAsync(CreateFgpoLineDto dto);
    Task<bool> UpdateAsync(int id, UpdateFgpoLineDto dto);
    Task<bool> DeleteAsync(int id);
    Task<IEnumerable<FgpoLineDto>> GetByFgpoAsync(int fgpoId);
    Task<PagedResultDto<FgpoLineDto>> GetPagedAsync(int page, int pageSize, string? search, string? sortBy, string? sortOrder);
}
