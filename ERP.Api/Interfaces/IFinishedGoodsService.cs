using ERP.Api.Dtos;

namespace ERP.Api.Interfaces;

public interface IFinishedGoodsService
{
    Task<IEnumerable<FinishedGoodDto>> GetAllAsync();
    Task<FinishedGoodDto?> GetByIdAsync(int id);
    Task<IEnumerable<FinishedGoodDto>> GetByFgpoAsync(int fgpoId);
    Task<FinishedGoodDto> CreateAsync(CreateFinishedGoodDto dto);
    Task<bool> UpdateAsync(int id, UpdateFinishedGoodDto dto);
    Task<bool> DeleteAsync(int id);
    Task<PagedResultDto<FinishedGoodDto>> GetPagedAsync(int page, int pageSize, string? search, string? sortBy, string? sortOrder, string? fgpo, string? status);
}
