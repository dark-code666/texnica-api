using ERP.Api.Dtos;

namespace ERP.Api.Interfaces;

public interface IBoxTypeService
{
    Task<IEnumerable<BoxTypeDto>> GetAllAsync();
    Task<BoxTypeDto?> GetByIdAsync(int id);
    Task<BoxTypeDto> CreateAsync(CreateBoxTypeDto dto);
    Task<bool> UpdateAsync(int id, UpdateBoxTypeDto dto);
    Task<bool> DeleteAsync(int id);
    Task<IEnumerable<BoxTypeDto>> SearchAsync(string? term);
}
