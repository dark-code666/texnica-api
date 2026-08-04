using ERP.Api.Dtos;

namespace ERP.Api.Interfaces;

public interface IFabricRequirementService
{
    Task<IEnumerable<FabricRequirementDto>> GetAllAsync();
    Task<FabricRequirementDto?> GetByIdAsync(int id);
    Task<IEnumerable<FabricRequirementDto>> GetByFgpoAsync(int fgpoId);
    Task<FabricRequirementDto> CreateAsync(CreateFabricRequirementDto dto);
    Task<bool> UpdateAsync(int id, UpdateFabricRequirementDto dto);
    Task<bool> DeleteAsync(int id);
    Task<PagedResultDto<FabricRequirementDto>> GetPagedAsync(int page, int pageSize, string? search, string? sortBy, string? sortOrder, string? fgpo, string? customer, string? style, string? fabricComponent, string? status);
}
