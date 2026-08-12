using ERP.Api.Dtos;

namespace ERP.Api.Interfaces;

public interface ITrimsControlService
{
    Task<IEnumerable<TrimsControlDto>> GetAllAsync();
    Task<TrimsControlDto?> GetByIdAsync(int id);
    Task<IEnumerable<TrimsControlDto>> GetByFgpoAsync(int fgpoId);
    Task<TrimsControlDto> CreateAsync(CreateTrimsControlDto dto);
    Task<bool> UpdateAsync(int id, UpdateTrimsControlDto dto);
    Task<bool> DeleteAsync(int id);
    Task<PagedResultDto<TrimsControlDto>> GetPagedAsync(int page, int pageSize, string? search, string? sortBy, string? sortOrder, string? fgpo, string? status);
}
