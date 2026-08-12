using Microsoft.EntityFrameworkCore;
using ERP.Api.Data;
using ERP.Api.Dtos;
using ERP.Api.Domain;
using ERP.Api.Interfaces;

namespace ERP.Api.Services;

public class BoxTypeService : IBoxTypeService
{
    private readonly ErpDbContext _context;

    public BoxTypeService(ErpDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<BoxTypeDto>> GetAllAsync()
    {
        var items = await _context.BoxTypes
            .Where(b => b.Active)
            .OrderBy(b => b.BoxCode)
            .ToListAsync();
        return items.Select(ToDto);
    }

    public async Task<BoxTypeDto?> GetByIdAsync(int id)
    {
        var item = await _context.BoxTypes.FirstOrDefaultAsync(b => b.ID == id && b.Active);
        return item is null ? null : ToDto(item);
    }

    public async Task<BoxTypeDto> CreateAsync(CreateBoxTypeDto dto)
    {
        if (string.IsNullOrWhiteSpace(dto.BoxCode))
            throw new Exception("El Box Code es obligatorio.");

        var exists = await _context.BoxTypes.AnyAsync(b => b.BoxCode == dto.BoxCode);
        if (exists)
            throw new Exception("El Box Type ya existe.");

        var entity = new BoxType
        {
            BoxCode = dto.BoxCode.Trim(),
            Length = dto.Length,
            Width = dto.Width,
            Height = dto.Height,
            EmptyCartonWeight = dto.EmptyCartonWeight,
            Comments = dto.Comments,
            Active = true,
            CreatedAt = DateTime.UtcNow,
        };

        _context.BoxTypes.Add(entity);
        await _context.SaveChangesAsync();
        return ToDto(entity);
    }

    public async Task<bool> UpdateAsync(int id, UpdateBoxTypeDto dto)
    {
        var entity = await _context.BoxTypes.FirstOrDefaultAsync(b => b.ID == id && b.Active);
        if (entity is null)
            return false;

        if (string.IsNullOrWhiteSpace(dto.BoxCode))
            throw new Exception("El Box Code es obligatorio.");

        var exists = await _context.BoxTypes.AnyAsync(b => b.BoxCode == dto.BoxCode && b.ID != id);
        if (exists)
            throw new Exception("El Box Type ya existe.");

        entity.BoxCode = dto.BoxCode.Trim();
        entity.Length = dto.Length;
        entity.Width = dto.Width;
        entity.Height = dto.Height;
        entity.EmptyCartonWeight = dto.EmptyCartonWeight;
        entity.Comments = dto.Comments;
        entity.UpdatedAt = DateTime.UtcNow;

        _context.BoxTypes.Update(entity);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var entity = await _context.BoxTypes.FirstOrDefaultAsync(b => b.ID == id && b.Active);
        if (entity is null)
            return false;

        entity.Active = false;
        entity.UpdatedAt = DateTime.UtcNow;
        _context.BoxTypes.Update(entity);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<IEnumerable<BoxTypeDto>> SearchAsync(string? term)
    {
        var query = _context.BoxTypes.Where(b => b.Active);
        if (!string.IsNullOrWhiteSpace(term))
        {
            var t = term.Trim();
            query = query.Where(b => b.BoxCode.Contains(t));
        }
        var items = await query.OrderBy(b => b.BoxCode).ToListAsync();
        return items.Select(ToDto);
    }

    private static BoxTypeDto ToDto(BoxType item) => new()
    {
        ID = item.ID,
        BoxCode = item.BoxCode,
        Length = item.Length,
        Width = item.Width,
        Height = item.Height,
        EmptyCartonWeight = item.EmptyCartonWeight,
        Comments = item.Comments,
        Active = item.Active,
        CreatedAt = item.CreatedAt,
        UpdatedAt = item.UpdatedAt,
    };
}
