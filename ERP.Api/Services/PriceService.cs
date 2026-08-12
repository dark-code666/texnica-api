using Microsoft.EntityFrameworkCore;
using ERP.Api.Data;
using ERP.Api.Dtos;
using ERP.Api.Domain;
using ERP.Api.Interfaces;

namespace ERP.Api.Services;

public class PriceService : IPriceService
{
    private readonly ErpDbContext _context;

    public PriceService(ErpDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<PriceDto>> GetAllAsync()
    {
        var items = await _context.Prices
            .Include(p => p.Style)
            .Include(p => p.Color)
            .Include(p => p.Size)
            .Where(p => p.Active)
            .OrderBy(p => p.Style!.StyleCode).ThenBy(p => p.Color!.ColorName).ThenBy(p => p.Size!.SortOrder)
            .ToListAsync();
        return items.Select(ToDto);
    }

    public async Task<PriceDto?> GetByIdAsync(int id)
    {
        var item = await _context.Prices
            .Include(p => p.Style)
            .Include(p => p.Color)
            .Include(p => p.Size)
            .FirstOrDefaultAsync(p => p.ID == id && p.Active);
        return item is null ? null : ToDto(item);
    }

    public async Task<IEnumerable<PriceDto>> GetByStyleAsync(int styleId)
    {
        var items = await _context.Prices
            .Include(p => p.Style)
            .Include(p => p.Color)
            .Include(p => p.Size)
            .Where(p => p.Active && p.StyleId == styleId)
            .OrderBy(p => p.Color!.ColorName).ThenBy(p => p.Size!.SortOrder)
            .ToListAsync();
        return items.Select(ToDto);
    }

    public async Task<PriceDto> CreateAsync(CreatePriceDto dto)
    {
        await ValidateAsync(dto.StyleId, dto.ColorId, dto.SizeId);

        var exists = await _context.Prices.AnyAsync(p => p.StyleId == dto.StyleId && p.ColorId == dto.ColorId && p.SizeId == dto.SizeId);
        if (exists)
            throw new Exception("El precio para ese Style/Color/Size ya existe.");

        var entity = new Price
        {
            StyleId = dto.StyleId,
            ColorId = dto.ColorId,
            SizeId = dto.SizeId,
            Sku = dto.Sku,
            UnitPrice = dto.UnitPrice,
            Comments = dto.Comments,
            Active = true,
            CreatedAt = DateTime.UtcNow,
        };

        _context.Prices.Add(entity);
        await _context.SaveChangesAsync();

        await _context.Entry(entity).Reference(p => p.Style).LoadAsync();
        await _context.Entry(entity).Reference(p => p.Color).LoadAsync();
        await _context.Entry(entity).Reference(p => p.Size).LoadAsync();
        return ToDto(entity);
    }

    public async Task<bool> UpdateAsync(int id, UpdatePriceDto dto)
    {
        var entity = await _context.Prices.FirstOrDefaultAsync(p => p.ID == id && p.Active);
        if (entity is null)
            return false;

        await ValidateAsync(dto.StyleId, dto.ColorId, dto.SizeId);

        var exists = await _context.Prices.AnyAsync(p => p.StyleId == dto.StyleId && p.ColorId == dto.ColorId && p.SizeId == dto.SizeId && p.ID != id);
        if (exists)
            throw new Exception("El precio para ese Style/Color/Size ya existe.");

        entity.StyleId = dto.StyleId;
        entity.ColorId = dto.ColorId;
        entity.SizeId = dto.SizeId;
        entity.Sku = dto.Sku;
        entity.UnitPrice = dto.UnitPrice;
        entity.Comments = dto.Comments;
        entity.UpdatedAt = DateTime.UtcNow;

        _context.Prices.Update(entity);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var entity = await _context.Prices.FirstOrDefaultAsync(p => p.ID == id && p.Active);
        if (entity is null)
            return false;

        entity.Active = false;
        entity.UpdatedAt = DateTime.UtcNow;
        _context.Prices.Update(entity);
        await _context.SaveChangesAsync();
        return true;
    }

    private async Task ValidateAsync(int styleId, int colorId, int sizeId)
    {
        if (await _context.Styles.FirstOrDefaultAsync(s => s.ID == styleId && s.Active) is null)
            throw new Exception("El Style seleccionado no es válido.");
        if (await _context.Colors.FirstOrDefaultAsync(c => c.ID == colorId && c.Active) is null)
            throw new Exception("El Color seleccionado no es válido.");
        if (await _context.Sizes.FirstOrDefaultAsync(s => s.ID == sizeId && s.Active) is null)
            throw new Exception("El Size seleccionado no es válido.");
    }

    private static PriceDto ToDto(Price item) => new()
    {
        ID = item.ID,
        StyleId = item.StyleId,
        StyleCode = item.Style?.StyleCode ?? string.Empty,
        ColorId = item.ColorId,
        ColorName = item.Color?.ColorName ?? string.Empty,
        SizeId = item.SizeId,
        SizeCode = item.Size?.SizeCode ?? string.Empty,
        Sku = item.Sku,
        UnitPrice = item.UnitPrice,
        Comments = item.Comments,
        Active = item.Active,
        CreatedAt = item.CreatedAt,
        UpdatedAt = item.UpdatedAt,
    };
}
