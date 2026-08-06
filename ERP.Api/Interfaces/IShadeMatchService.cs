using ERP.Api.Dtos;

namespace ERP.Api.Interfaces;

public interface IShadeMatchService
{
    Task<IEnumerable<ShadeMatchDto>> GetAllAsync();
    Task<ShadeMatchDto?> GetByIdAsync(int id);
    Task<IEnumerable<ShadeMatchDto>> GetByFgpoAsync(int fgpoId);
    Task<ShadeMatchDto> CreateAsync(CreateShadeMatchDto dto);
    Task<bool> UpdateAsync(int id, UpdateShadeMatchDto dto);
    Task<bool> DeleteAsync(int id);
    Task<PagedResultDto<ShadeMatchDto>> GetPagedAsync(int page, int pageSize, string? search, string? sortBy, string? sortOrder, string? fgpo, string? result);
}
