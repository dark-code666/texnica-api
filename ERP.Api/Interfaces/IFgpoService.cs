using ERP.Api.Dtos;

namespace ERP.Api.Interfaces;

public interface IFgpoService
{
    Task<IEnumerable<FgpoDto>> GetAllAsync();
    Task<FgpoDto?> GetByIdAsync(int id);
    Task<FgpoDto> CreateAsync(CreateFgpoDto dto);
    Task<bool> UpdateAsync(int id, UpdateFgpoDto dto);
    Task<bool> DeleteAsync(int id);
    Task<IEnumerable<FgpoDto>> SearchAsync(string? term);
    Task<PagedResultDto<FgpoDto>> GetPagedAsync(int page, int pageSize, string? search, string? sortBy, string? sortOrder, string? status, string? customer);
}
