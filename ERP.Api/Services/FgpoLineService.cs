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

        var exists = await _context.FgpoLines.AnyAsync(l =>
            l.Active &&
            l.FgpoId == dto.FgpoId &&
            l.StyleId == dto.StyleId &&
            l.ColorId == dto.ColorId &&
            l.SizeId == dto.SizeId);
        if (exists)
            throw new Exception("Esa combinación Style/Color/Size ya existe en el FGPO.");

        // Si no viene precio, se toma el del catálogo Prices (Style+Color+Size)
        var unitPrice = dto.UnitPrice ?? await ResolvePriceAsync(dto.StyleId, dto.ColorId, dto.SizeId);

        var entity = new FgpoLine
        {
            FgpoId = dto.FgpoId,
            StyleId = dto.StyleId,
            ColorId = dto.ColorId,
            SizeId = dto.SizeId,
            Quantity = dto.Quantity,
            UnitPrice = unitPrice,
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

        var exists = await _context.FgpoLines.AnyAsync(l =>
            l.Active &&
            l.FgpoId == dto.FgpoId &&
            l.StyleId == dto.StyleId &&
            l.ColorId == dto.ColorId &&
            l.SizeId == dto.SizeId &&
            l.ID != id);
        if (exists)
            throw new Exception("Esa combinación Style/Color/Size ya existe en el FGPO.");

        entity.FgpoId = dto.FgpoId;
        entity.StyleId = dto.StyleId;
        entity.ColorId = dto.ColorId;
        entity.SizeId = dto.SizeId;
        entity.Quantity = dto.Quantity;
        // Si no viene precio, se toma el del catálogo Prices
        entity.UnitPrice = dto.UnitPrice ?? await ResolvePriceAsync(dto.StyleId, dto.ColorId, dto.SizeId);
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

    // Toma el UnitPrice del catálogo Prices (Style+Color+Size) si existe
    private async Task<decimal?> ResolvePriceAsync(int styleId, int colorId, int sizeId)
    {
        var price = await _context.Prices
            .FirstOrDefaultAsync(p => p.Active && p.StyleId == styleId && p.ColorId == colorId && p.SizeId == sizeId);
        return price?.UnitPrice;
    }

    public async Task<PagedResultDto<FgpoLineDto>> GetPagedAsync(int page, int pageSize, string? search, string? sortBy, string? sortOrder)
    {
        if (page < 1) page = 1;
        if (pageSize < 1) pageSize = 10;
        if (pageSize > 100) pageSize = 100;

        var query = _context.FgpoLines
            .Include(l => l.Fgpo).ThenInclude(f => f!.Customer)
            .Include(l => l.Style)
            .Include(l => l.Color)
            .Include(l => l.Size)
            .Where(l => l.Active);

        if (!string.IsNullOrWhiteSpace(search))
        {
            var term = search.Trim();
            query = query.Where(l =>
                (l.Fgpo != null && l.Fgpo.FGPONumber.Contains(term)) ||
                (l.Style != null && l.Style.StyleCode.Contains(term)) ||
                (l.Color != null && l.Color.ColorName.Contains(term)) ||
                (l.Size != null && l.Size.SizeCode.Contains(term)));
        }

        var totalCount = await query.CountAsync();
        var totalPages = (int)Math.Ceiling(totalCount / (double)pageSize);

        var sortByLower = sortBy?.ToLowerInvariant();
        var descending = sortOrder?.ToLowerInvariant() == "desc";

        IQueryable<FgpoLine> orderedQuery = (sortByLower, descending) switch
        {
            ("stylecode", false) => query.OrderBy(l => l.Style!.StyleCode),
            ("stylecode", true) => query.OrderByDescending(l => l.Style!.StyleCode),
            ("quantity", false) => query.OrderBy(l => l.Quantity),
            ("quantity", true) => query.OrderByDescending(l => l.Quantity),
            ("createdat", false) => query.OrderBy(l => l.CreatedAt),
            ("createdat", true) => query.OrderByDescending(l => l.CreatedAt),
            _ => query.OrderBy(l => l.Style!.StyleCode),
        };

        var items = await orderedQuery.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();

        return new PagedResultDto<FgpoLineDto>
        {
            Items = items.Select(ToDto),
            TotalCount = totalCount,
            Page = page,
            PageSize = pageSize,
            TotalPages = totalPages,
        };
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
