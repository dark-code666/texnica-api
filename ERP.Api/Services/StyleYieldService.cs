using Microsoft.EntityFrameworkCore;
using ERP.Api.Data;
using ERP.Api.Dtos;
using ERP.Api.Domain;
using ERP.Api.Interfaces;

namespace ERP.Api.Services;

public class StyleYieldService : IStyleYieldService
{
    private readonly ErpDbContext _context;

    public StyleYieldService(ErpDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<StyleYieldDto>> GetAllAsync()
    {
        var items = await _context.StyleYields
            .Include(y => y.Style)
            .Include(y => y.Component)
            .Where(y => y.Active)
            .OrderBy(y => y.Style!.StyleCode).ThenBy(y => y.Component!.ComponentCode)
            .ToListAsync();
        return items.Select(ToDto);
    }

    public async Task<StyleYieldDto?> GetByIdAsync(int id)
    {
        var item = await _context.StyleYields
            .Include(y => y.Style)
            .Include(y => y.Component)
            .FirstOrDefaultAsync(y => y.ID == id && y.Active);
        return item is null ? null : ToDto(item);
    }

    public async Task<IEnumerable<StyleYieldDto>> GetByStyleAsync(int styleId)
    {
        var items = await _context.StyleYields
            .Include(y => y.Style)
            .Include(y => y.Component)
            .Where(y => y.Active && y.StyleId == styleId)
            .OrderBy(y => y.Component!.ComponentCode)
            .ToListAsync();
        return items.Select(ToDto);
    }

    public async Task<StyleYieldDto> CreateAsync(CreateStyleYieldDto dto)
    {
        await ValidateAsync(dto.StyleId, dto.ComponentId);

        var exists = await _context.StyleYields.AnyAsync(y => y.StyleId == dto.StyleId && y.ComponentId == dto.ComponentId);
        if (exists)
            throw new Exception("El Yield para ese Style y Component ya existe.");

        var entity = new StyleYield
        {
            StyleId = dto.StyleId,
            ComponentId = dto.ComponentId,
            YieldQuoted = dto.YieldQuoted,
            YieldReal = dto.YieldReal,
            Notes = dto.Notes,
            Active = true,
            CreatedAt = DateTime.UtcNow,
        };

        _context.StyleYields.Add(entity);
        await _context.SaveChangesAsync();

        await _context.Entry(entity).Reference(y => y.Style).LoadAsync();
        await _context.Entry(entity).Reference(y => y.Component).LoadAsync();
        return ToDto(entity);
    }

    public async Task<bool> UpdateAsync(int id, UpdateStyleYieldDto dto)
    {
        var entity = await _context.StyleYields.FirstOrDefaultAsync(y => y.ID == id && y.Active);
        if (entity is null)
            return false;

        await ValidateAsync(dto.StyleId, dto.ComponentId);

        var exists = await _context.StyleYields.AnyAsync(y => y.StyleId == dto.StyleId && y.ComponentId == dto.ComponentId && y.ID != id);
        if (exists)
            throw new Exception("El Yield para ese Style y Component ya existe.");

        entity.StyleId = dto.StyleId;
        entity.ComponentId = dto.ComponentId;
        entity.YieldQuoted = dto.YieldQuoted;
        entity.YieldReal = dto.YieldReal;
        entity.Notes = dto.Notes;
        entity.UpdatedAt = DateTime.UtcNow;

        _context.StyleYields.Update(entity);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var entity = await _context.StyleYields.FirstOrDefaultAsync(y => y.ID == id && y.Active);
        if (entity is null)
            return false;

        entity.Active = false;
        entity.UpdatedAt = DateTime.UtcNow;
        _context.StyleYields.Update(entity);
        await _context.SaveChangesAsync();
        return true;
    }

    private async Task ValidateAsync(int styleId, int componentId)
    {
        if (await _context.Styles.FirstOrDefaultAsync(s => s.ID == styleId && s.Active) is null)
            throw new Exception("El Style seleccionado no es válido.");
        if (await _context.Components.FirstOrDefaultAsync(c => c.ID == componentId && c.Active) is null)
            throw new Exception("El Component seleccionado no es válido.");
    }

    public async Task<PagedResultDto<StyleYieldDto>> GetPagedAsync(int page, int pageSize, string? search, string? sortBy, string? sortOrder)
    {
        if (page < 1) page = 1;
        if (pageSize < 1) pageSize = 10;
        if (pageSize > 100) pageSize = 100;

        var query = _context.StyleYields
            .Include(y => y.Style)
            .Include(y => y.Component)
            .Where(y => y.Active);

        if (!string.IsNullOrWhiteSpace(search))
        {
            var term = search.Trim();
            query = query.Where(y =>
                (y.Style != null && y.Style.StyleCode.Contains(term)) ||
                (y.Component != null && y.Component.ComponentCode.Contains(term)) ||
                (y.Notes != null && y.Notes.Contains(term)));
        }

        var totalCount = await query.CountAsync();
        var totalPages = (int)Math.Ceiling(totalCount / (double)pageSize);

        var sortByLower = sortBy?.ToLowerInvariant();
        var descending = sortOrder?.ToLowerInvariant() == "desc";

        IQueryable<StyleYield> orderedQuery = (sortByLower, descending) switch
        {
            ("stylecode", false) => query.OrderBy(y => y.Style!.StyleCode),
            ("stylecode", true) => query.OrderByDescending(y => y.Style!.StyleCode),
            ("componentcode", false) => query.OrderBy(y => y.Component!.ComponentCode),
            ("componentcode", true) => query.OrderByDescending(y => y.Component!.ComponentCode),
            ("yieldquoted", false) => query.OrderBy(y => y.YieldQuoted),
            ("yieldquoted", true) => query.OrderByDescending(y => y.YieldQuoted),
            ("createdat", false) => query.OrderBy(y => y.CreatedAt),
            ("createdat", true) => query.OrderByDescending(y => y.CreatedAt),
            _ => query.OrderBy(y => y.Style!.StyleCode),
        };

        var items = await orderedQuery.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();

        return new PagedResultDto<StyleYieldDto>
        {
            Items = items.Select(ToDto),
            TotalCount = totalCount,
            Page = page,
            PageSize = pageSize,
            TotalPages = totalPages,
        };
    }

    private static StyleYieldDto ToDto(StyleYield item) => new()
    {
        ID = item.ID,
        StyleId = item.StyleId,
        StyleCode = item.Style?.StyleCode ?? string.Empty,
        ComponentId = item.ComponentId,
        ComponentCode = item.Component?.ComponentCode ?? string.Empty,
        YieldQuoted = item.YieldQuoted,
        YieldReal = item.YieldReal,
        Notes = item.Notes,
        Active = item.Active,
        CreatedAt = item.CreatedAt,
        UpdatedAt = item.UpdatedAt,
    };
}
