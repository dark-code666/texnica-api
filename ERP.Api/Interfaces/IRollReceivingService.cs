using ERP.Api.Dtos;

namespace ERP.Api.Interfaces;

public interface IRollReceivingService
{
    Task<IEnumerable<RollReceivingDto>> GetAllAsync();
    Task<RollReceivingDto?> GetByIdAsync(int id);
    Task<IEnumerable<RollReceivingDto>> GetByReceivingAsync(int receivingId);
    Task<RollReceivingDto> CreateAsync(CreateRollReceivingDto dto);
    Task<bool> UpdateAsync(int id, UpdateRollReceivingDto dto);
    Task<bool> DeleteAsync(int id);
    Task<PagedResultDto<RollReceivingDto>> GetPagedAsync(int page, int pageSize, string? search, string? sortBy, string? sortOrder, string? receiving, string? fabricPO, string? lotNumber);
}
