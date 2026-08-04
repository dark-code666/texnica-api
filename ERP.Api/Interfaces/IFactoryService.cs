using ERP.Api.Dtos;

namespace ERP.Api.Interfaces;

public interface IFactoryService
{
    Task<IEnumerable<FactoryDto>> GetAllAsync();
    Task<FactoryDto?> GetByIdAsync(int id);
    Task<FactoryDto> CreateAsync(CreateFactoryDto dto);
    Task<bool> UpdateAsync(int id, UpdateFactoryDto dto);
    Task<bool> DeleteAsync(int id);
    Task<IEnumerable<FactoryDto>> SearchAsync(string? term);
    Task<PagedResultDto<FactoryDto>> GetPagedAsync(int page, int pageSize, string? search, string? sortBy, string? sortOrder);
}
