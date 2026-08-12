using ERP.Api.Dtos;

namespace ERP.Api.Interfaces;

public interface ISupplierService
{
    Task<IEnumerable<SupplierDto>> GetAllAsync();
    Task<SupplierDto?> GetByIdAsync(int id);
    Task<SupplierDto> CreateAsync(CreateSupplierDto dto);
    Task<bool> UpdateAsync(int id, UpdateSupplierDto dto);
    Task<bool> DeleteAsync(int id);
    Task<IEnumerable<SupplierDto>> SearchAsync(string? term);
    Task<PagedResultDto<SupplierDto>> GetPagedAsync(int page, int pageSize, string? search, string? sortBy, string? sortOrder);
}
