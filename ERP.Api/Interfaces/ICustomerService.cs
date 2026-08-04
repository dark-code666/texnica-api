using ERP.Api.Dtos;

namespace ERP.Api.Interfaces;

public interface ICustomerService
{
    Task<IEnumerable<CustomerDto>> GetAllAsync();
    Task<CustomerDto?> GetByIdAsync(int id);
    Task<CustomerDto> CreateAsync(CreateCustomerDto dto);
    Task<bool> UpdateAsync(int id, UpdateCustomerDto dto);
    Task<bool> DeleteAsync(int id);
    Task<IEnumerable<CustomerDto>> SearchAsync(string? term);
    Task<PagedResultDto<CustomerDto>> GetPagedAsync(int page, int pageSize, string? search, string? sortBy, string? sortOrder);
}
