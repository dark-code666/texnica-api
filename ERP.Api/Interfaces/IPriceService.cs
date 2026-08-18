using ERP.Api.Dtos;

namespace ERP.Api.Interfaces;

public interface IPriceService
{
    Task<IEnumerable<PriceDto>> GetAllAsync();
    Task<PriceDto?> GetByIdAsync(int id);
    Task<PriceDto> CreateAsync(CreatePriceDto dto);
    Task<bool> UpdateAsync(int id, UpdatePriceDto dto);
    Task<bool> DeleteAsync(int id);
    Task<IEnumerable<PriceDto>> GetByStyleAsync(int styleId);
    Task<PagedResultDto<PriceDto>> GetPagedAsync(int page, int pageSize, string? search, string? sortBy, string? sortOrder);
}
