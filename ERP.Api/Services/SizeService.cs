using Microsoft.EntityFrameworkCore;
using ERP.Api.Data;
using ERP.Api.Dtos;
using ERP.Api.Domain;
using ERP.Api.Interfaces;

namespace ERP.Api.Services;

public class SizeService : ISizeService
{
    private readonly ErpDbContext _context;

    public SizeService(ErpDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<SizeDto>> GetAllAsync()
    {
        var items = await _context.Sizes
            .Where(s => s.Active)
            .OrderBy(s => s.SortOrder)
            .ToListAsync();
        return items.Select(ToDto);
    }

    public async Task<SizeDto?> GetByIdAsync(int id)
    {
        var item = await _context.Sizes.FirstOrDefaultAsync(s => s.ID == id && s.Active);
        return item is null ? null : ToDto(item);
    }

    public async Task<SizeDto> CreateAsync(CreateSizeDto dto)
    {
        if (string.IsNullOrWhiteSpace(dto.SizeCode))
            throw new Exception("El Size Code es obligatorio.");

        var exists = await _context.Sizes.AnyAsync(s => s.SizeCode == dto.SizeCode);
        if (exists)
            throw new Exception("El Size ya existe.");

        var entity = new Size
        {
            SizeCode = dto.SizeCode.Trim(),
            SortOrder = dto.SortOrder,
            Active = true,
            CreatedAt = DateTime.UtcNow,
        };

        _context.Sizes.Add(entity);
        await _context.SaveChangesAsync();
        return ToDto(entity);
    }

    public async Task<bool> UpdateAsync(int id, UpdateSizeDto dto)
    {
        var entity = await _context.Sizes.FirstOrDefaultAsync(s => s.ID == id && s.Active);
        if (entity is null)
            return false;

        if (string.IsNullOrWhiteSpace(dto.SizeCode))
            throw new Exception("El Size Code es obligatorio.");

        var exists = await _context.Sizes.AnyAsync(s => s.SizeCode == dto.SizeCode && s.ID != id);
        if (exists)
            throw new Exception("El Size ya existe.");

        entity.SizeCode = dto.SizeCode.Trim();
        entity.SortOrder = dto.SortOrder;
        entity.UpdatedAt = DateTime.UtcNow;

        _context.Sizes.Update(entity);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var entity = await _context.Sizes.FirstOrDefaultAsync(s => s.ID == id && s.Active);
        if (entity is null)
            return false;

        entity.Active = false;
        entity.UpdatedAt = DateTime.UtcNow;
        _context.Sizes.Update(entity);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<IEnumerable<SizeDto>> SearchAsync(string? term)
    {
        var query = _context.Sizes.Where(s => s.Active);
        if (!string.IsNullOrWhiteSpace(term))
        {
            var t = term.Trim();
            query = query.Where(s => s.SizeCode.Contains(t));
        }
        var items = await query.OrderBy(s => s.SortOrder).ToListAsync();
        return items.Select(ToDto);
    }

    private static SizeDto ToDto(Size item) => new()
    {
        ID = item.ID,
        SizeCode = item.SizeCode,
        SortOrder = item.SortOrder,
        Active = item.Active,
        CreatedAt = item.CreatedAt,
        UpdatedAt = item.UpdatedAt,
    };
}
