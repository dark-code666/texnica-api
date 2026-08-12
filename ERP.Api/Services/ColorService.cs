using Microsoft.EntityFrameworkCore;
using ERP.Api.Data;
using ERP.Api.Dtos;
using ERP.Api.Domain;
using ERP.Api.Interfaces;

namespace ERP.Api.Services;

public class ColorService : IColorService
{
    private readonly ErpDbContext _context;

    public ColorService(ErpDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<ColorDto>> GetAllAsync()
    {
        var items = await _context.Colors
            .Where(c => c.Active)
            .OrderBy(c => c.ColorName)
            .ToListAsync();
        return items.Select(ToDto);
    }

    public async Task<ColorDto?> GetByIdAsync(int id)
    {
        var item = await _context.Colors.FirstOrDefaultAsync(c => c.ID == id && c.Active);
        return item is null ? null : ToDto(item);
    }

    public async Task<ColorDto> CreateAsync(CreateColorDto dto)
    {
        if (string.IsNullOrWhiteSpace(dto.ColorName))
            throw new Exception("El Color Name es obligatorio.");

        var exists = await _context.Colors.AnyAsync(c => c.ColorName == dto.ColorName);
        if (exists)
            throw new Exception("El Color ya existe.");

        var entity = new Color
        {
            ColorName = dto.ColorName.Trim(),
            DyeMethod = dto.DyeMethod,
            Active = true,
            CreatedAt = DateTime.UtcNow,
        };

        _context.Colors.Add(entity);
        await _context.SaveChangesAsync();
        return ToDto(entity);
    }

    public async Task<bool> UpdateAsync(int id, UpdateColorDto dto)
    {
        var entity = await _context.Colors.FirstOrDefaultAsync(c => c.ID == id && c.Active);
        if (entity is null)
            return false;

        if (string.IsNullOrWhiteSpace(dto.ColorName))
            throw new Exception("El Color Name es obligatorio.");

        var exists = await _context.Colors.AnyAsync(c => c.ColorName == dto.ColorName && c.ID != id);
        if (exists)
            throw new Exception("El Color ya existe.");

        entity.ColorName = dto.ColorName.Trim();
        entity.DyeMethod = dto.DyeMethod;
        entity.UpdatedAt = DateTime.UtcNow;

        _context.Colors.Update(entity);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var entity = await _context.Colors.FirstOrDefaultAsync(c => c.ID == id && c.Active);
        if (entity is null)
            return false;

        entity.Active = false;
        entity.UpdatedAt = DateTime.UtcNow;
        _context.Colors.Update(entity);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<IEnumerable<ColorDto>> SearchAsync(string? term)
    {
        var query = _context.Colors.Where(c => c.Active);
        if (!string.IsNullOrWhiteSpace(term))
        {
            var t = term.Trim();
            query = query.Where(c => c.ColorName.Contains(t) || (c.DyeMethod != null && c.DyeMethod.Contains(t)));
        }
        var items = await query.OrderBy(c => c.ColorName).ToListAsync();
        return items.Select(ToDto);
    }

    private static ColorDto ToDto(Color item) => new()
    {
        ID = item.ID,
        ColorName = item.ColorName,
        DyeMethod = item.DyeMethod,
        Active = item.Active,
        CreatedAt = item.CreatedAt,
        UpdatedAt = item.UpdatedAt,
    };
}
