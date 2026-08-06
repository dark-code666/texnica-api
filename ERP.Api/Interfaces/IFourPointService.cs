using ERP.Api.Dtos;

namespace ERP.Api.Interfaces;

public interface IFourPointService
{
    Task<IEnumerable<FourPointDto>> GetAllAsync();
    Task<FourPointDto?> GetByIdAsync(int id);
    Task<IEnumerable<FourPointDto>> GetByFabricPOAsync(int fabricPOId);
    Task<IEnumerable<FourPointDto>> GetByFgpoAsync(int fgpoId);
    Task<IEnumerable<FourPointDto>> GetByReceivingAsync(int receivingId);
    Task<FourPointDto> CreateAsync(CreateFourPointDto dto);
    Task<bool> UpdateAsync(int id, UpdateFourPointDto dto);
    Task<bool> DeleteAsync(int id);
    Task<PagedResultDto<FourPointDto>> GetPagedAsync(int page, int pageSize, string? search, string? sortBy, string? sortOrder, string? fabricPO, string? fgpo, string? result);
}
