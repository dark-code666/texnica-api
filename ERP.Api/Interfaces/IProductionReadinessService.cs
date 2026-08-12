using ERP.Api.Dtos;

namespace ERP.Api.Interfaces;

public interface IProductionReadinessService
{
    Task<IEnumerable<ProductionReadinessDto>> GetAllAsync();
    Task<ProductionReadinessDto?> GetByIdAsync(int id);
    Task<IEnumerable<ProductionReadinessDto>> GetByFgpoAsync(int fgpoId);
    Task<ProductionReadinessDto> CreateAsync(CreateProductionReadinessDto dto);
    Task<bool> UpdateAsync(int id, UpdateProductionReadinessDto dto);
    Task<bool> DeleteAsync(int id);
    Task<PagedResultDto<ProductionReadinessDto>> GetPagedAsync(int page, int pageSize, string? search, string? sortBy, string? sortOrder, string? fgpo, string? result);
}
