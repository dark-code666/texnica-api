using Microsoft.EntityFrameworkCore;
using ERP.Api.Data;
using ERP.Api.Dtos;
using ERP.Api.Domain;
using ERP.Api.Interfaces;

namespace ERP.Api.Services;

public class FgpoLineService : IFgpoLineService
{
    private readonly ErpDbContext _context;

    public FgpoLineService(ErpDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<FgpoLineDto>> GetAllAsync()
    {
        var items = await _context.FgpoLines
            .Include(l => l.Fgpo).ThenInclude(f => f!.Customer)
            .Include(l => l.Style)
            .Include(l => l.Color)
            .Include(l => l.Size)
            .Where(l => l.Active)
            .OrderBy(l => l.Fgpo!.FGPONumber).ThenBy(l => l.Style!.StyleCode).ThenBy(l => l.Size!.SortOrder)
            .ToListAsync();
        return items.Select(ToDto);
    }

    public async Task<FgpoLineDto?> GetByIdAsync(int id)
    {
        var item = await _context.FgpoLines
            .Include(l => l.Fgpo).ThenInclude(f => f!.Customer)
            .Include(l => l.Style)
            .Include(l => l.Color)
            .Include(l => l.Size)
            .FirstOrDefaultAsync(l => l.ID == id && l.Active);
        return item is null ? null : ToDto(item);
    }

    public async Task<IEnumerable<FgpoLineDto>> GetByFgpoAsync(int fgpoId)
    {
        var items = await _context.FgpoLines
            .Include(l => l.Fgpo).ThenInclude(f => f!.Customer)
            .Include(l => l.Style)
            .Include(l => l.Color)
            .Include(l => l.Size)
            .Where(l => l.Active && l.FgpoId == fgpoId)
            .OrderBy(l => l.Style!.StyleCode).ThenBy(l => l.Size!.SortOrder)
            .ToListAsync();
        return items.Select(ToDto);
    }

    public async Task<FgpoLineDto> CreateAsync(CreateFgpoLineDto dto)
    {
        await ValidateAsync(dto.FgpoId, dto.StyleId, dto.ColorId, dto.SizeId);
        if (dto.Quantity < 0)
            throw new Exception("La cantidad no puede ser negativa.");

        var exists = await _context.FgpoLines.AnyAsync(l => l.FgpoId == dto.FgpoId && l.StyleId == dto.StyleId && l.ColorId == dto.ColorId && l.SizeId == dto.SizeId);
        if (exists)
            throw new Exception("Esa combinación Style/Color/Size ya existe en el FGPO.");

        var entity = new FgpoLine
        {
            FgpoId = dto.FgpoId,
            StyleId = dto.StyleId,
            ColorId = dto.ColorId,
            SizeId = dto.SizeId,
            Quantity = dto.Quantity,
            UnitPrice = dto.UnitPrice,
            Active = true,
            CreatedAt = DateTime.UtcNow,
        };

        _context.FgpoLines.Add(entity);
        await _context.SaveChangesAsync();

        await _context.Entry(entity).Reference(l => l.Fgpo).Query().Include(f => f!.Customer).LoadAsync();
        await _context.Entry(entity).Reference(l => l.Style).LoadAsync();
        await _context.Entry(entity).Reference(l => l.Color).LoadAsync();
        await _context.Entry(entity).Reference(l => l.Size).LoadAsync();
        return ToDto(entity);
    }

    public async Task<bool> UpdateAsync(int id, UpdateFgpoLineDto dto)
    {
        var entity = await _context.FgpoLines.FirstOrDefaultAsync(l => l.ID == id && l.Active);
        if (entity is null)
            return false;

        await ValidateAsync(dto.FgpoId, dto.StyleId, dto.ColorId, dto.SizeId);
        if (dto.Quantity < 0)
            throw new Exception("La cantidad no puede ser negativa.");

        var exists = await _context.FgpoLines.AnyAsync(l => l.FgpoId == dto.FgpoId && l.StyleId == dto.StyleId && l.ColorId == dto.ColorId && l.SizeId == dto.SizeId && l.ID != id);
        if (exists)
            throw new Exception("Esa combinación Style/Color/Size ya existe en el FGPO.");

        entity.FgpoId = dto.FgpoId;
        entity.StyleId = dto.StyleId;
        entity.ColorId = dto.ColorId;
        entity.SizeId = dto.SizeId;
        entity.Quantity = dto.Quantity;
        entity.UnitPrice = dto.UnitPrice;
        entity.UpdatedAt = DateTime.UtcNow;

        _context.FgpoLines.Update(entity);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var entity = await _context.FgpoLines.FirstOrDefaultAsync(l => l.ID == id && l.Active);
        if (entity is null)
            return false;

        entity.Active = false;
        entity.UpdatedAt = DateTime.UtcNow;
        _context.FgpoLines.Update(entity);
        await _context.SaveChangesAsync();
        return true;
    }

    private async Task ValidateAsync(int fgpoId, int styleId, int colorId, int sizeId)
    {
        if (await _context.Fgpos.FirstOrDefaultAsync(f => f.ID == fgpoId && f.Active) is null)
            throw new Exception("El FGPO seleccionado no es válido.");
        if (await _context.Styles.FirstOrDefaultAsync(s => s.ID == styleId && s.Active) is null)
            throw new Exception("El Style seleccionado no es válido.");
        if (await _context.Colors.FirstOrDefaultAsync(c => c.ID == colorId && c.Active) is null)
            throw new Exception("El Color seleccionado no es válido.");
        if (await _context.Sizes.FirstOrDefaultAsync(s => s.ID == sizeId && s.Active) is null)
            throw new Exception("El Size seleccionado no es válido.");
    }

    private static FgpoLineDto ToDto(FgpoLine item) => new()
    {
        ID = item.ID,
        FgpoId = item.FgpoId,
        FgpoNumber = item.Fgpo?.FGPONumber ?? string.Empty,
        CustomerName = item.Fgpo?.Customer?.Name ?? string.Empty,
        StyleId = item.StyleId,
        StyleCode = item.Style?.StyleCode ?? string.Empty,
        ColorId = item.ColorId,
        ColorName = item.Color?.ColorName ?? string.Empty,
        SizeId = item.SizeId,
        SizeCode = item.Size?.SizeCode ?? string.Empty,
        Quantity = item.Quantity,
        UnitPrice = item.UnitPrice,
        TotalValue = (item.UnitPrice ?? 0) * item.Quantity,
        Active = item.Active,
        CreatedAt = item.CreatedAt,
        UpdatedAt = item.UpdatedAt,
    };
}
