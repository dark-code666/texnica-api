using ERP.Api.Dtos;

namespace ERP.Api.Interfaces;

public interface IAqlInspectionService
{
    Task<PagedResultDto<AqlInspectionDto>> GetAllAsync(int page, int pageSize, string? type, string? search);
    Task<AqlInspectionDto?> GetByIdAsync(int id);
    Task<AqlInspectionDto> CreateAsync(CreateAqlInspectionDto dto);
    Task<AqlInspectionDto?> UpdateAsync(int id, UpdateAqlInspectionDto dto);
    Task<bool> DeleteAsync(int id);
}
