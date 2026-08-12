using ERP.Api.Dtos;

namespace ERP.Api.Interfaces;

public interface ISizeService
{
    Task<IEnumerable<SizeDto>> GetAllAsync();
    Task<SizeDto?> GetByIdAsync(int id);
    Task<SizeDto> CreateAsync(CreateSizeDto dto);
    Task<bool> UpdateAsync(int id, UpdateSizeDto dto);
    Task<bool> DeleteAsync(int id);
    Task<IEnumerable<SizeDto>> SearchAsync(string? term);
}
