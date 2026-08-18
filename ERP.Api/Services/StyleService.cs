using Microsoft.EntityFrameworkCore;
using ERP.Api.Data;
using ERP.Api.Dtos;
using ERP.Api.Domain;
using ERP.Api.Interfaces;

namespace ERP.Api.Services;

public class StyleService : IStyleService
{
    private readonly ErpDbContext _context;

    public StyleService(ErpDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<StyleDto>> GetAllAsync()
    {
        var items = await _context.Styles
            .Where(s => s.Active)
            .OrderBy(s => s.StyleCode)
            .ToListAsync();
        return items.Select(ToDto);
    }

    public async Task<StyleDto?> GetByIdAsync(int id)
    {
        var item = await _context.Styles
            .FirstOrDefaultAsync(s => s.ID == id && s.Active);
        return item is null ? null : ToDto(item);
    }

    public async Task<StyleDto> CreateAsync(CreateStyleDto dto)
    {
        if (string.IsNullOrWhiteSpace(dto.StyleCode))
            throw new Exception("El Style Code es obligatorio.");

        var exists = await _context.Styles.AnyAsync(s => s.StyleCode == dto.StyleCode);
        if (exists)
            throw new Exception("El Style ya existe.");

        var entity = new Style
        {
            StyleCode = dto.StyleCode.Trim(),
            Description = dto.Description,
            FabricDescription = dto.FabricDescription,
            FabricContent = dto.FabricContent,
            Construction = dto.Construction,
            Gsm = dto.Gsm,
            WeightOz = dto.WeightOz,
            Comments = dto.Comments,
            Active = true,
            CreatedAt = DateTime.UtcNow,
        };

        _context.Styles.Add(entity);
        await _context.SaveChangesAsync();
        return ToDto(entity);
    }

    public async Task<bool> UpdateAsync(int id, UpdateStyleDto dto)
    {
        var entity = await _context.Styles.FirstOrDefaultAsync(s => s.ID == id && s.Active);
        if (entity is null)
            return false;

        if (string.IsNullOrWhiteSpace(dto.StyleCode))
            throw new Exception("El Style Code es obligatorio.");

        var exists = await _context.Styles.AnyAsync(s => s.StyleCode == dto.StyleCode && s.ID != id);
        if (exists)
            throw new Exception("El Style ya existe.");

        entity.StyleCode = dto.StyleCode.Trim();
        entity.Description = dto.Description;
        entity.FabricDescription = dto.FabricDescription;
        entity.FabricContent = dto.FabricContent;
        entity.Construction = dto.Construction;
        entity.Gsm = dto.Gsm;
        entity.WeightOz = dto.WeightOz;
        entity.Comments = dto.Comments;
        entity.UpdatedAt = DateTime.UtcNow;

        _context.Styles.Update(entity);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var entity = await _context.Styles.FirstOrDefaultAsync(s => s.ID == id && s.Active);
        if (entity is null)
            return false;

        entity.Active = false;
        entity.UpdatedAt = DateTime.UtcNow;
        _context.Styles.Update(entity);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<IEnumerable<StyleDto>> SearchAsync(string? term)
    {
        var query = _context.Styles.Where(s => s.Active);
        if (!string.IsNullOrWhiteSpace(term))
        {
            var t = term.Trim();
            query = query.Where(s => s.StyleCode.Contains(t) || (s.Description != null && s.Description.Contains(t)));
        }
        var items = await query.OrderBy(s => s.StyleCode).ToListAsync();
        return items.Select(ToDto);
    }

    public async Task<PagedResultDto<StyleDto>> GetPagedAsync(int page, int pageSize, string? search, string? sortBy, string? sortOrder)
    {
        if (page < 1) page = 1;
        if (pageSize < 1) pageSize = 10;
        if (pageSize > 100) pageSize = 100;

        var query = _context.Styles.Where(s => s.Active);

        if (!string.IsNullOrWhiteSpace(search))
        {
            var term = search.Trim();
            query = query.Where(s => s.StyleCode.Contains(term)
                || (s.Description != null && s.Description.Contains(term))
                || (s.FabricDescription != null && s.FabricDescription.Contains(term)));
        }

        var totalCount = await query.CountAsync();
        var totalPages = (int)Math.Ceiling(totalCount / (double)pageSize);

        var sortByLower = sortBy?.ToLowerInvariant();
        var descending = sortOrder?.ToLowerInvariant() == "desc";

        IQueryable<Style> orderedQuery = (sortByLower, descending) switch
        {
            ("stylecode", false) => query.OrderBy(s => s.StyleCode),
            ("stylecode", true) => query.OrderByDescending(s => s.StyleCode),
            ("gsm", false) => query.OrderBy(s => s.Gsm),
            ("gsm", true) => query.OrderByDescending(s => s.Gsm),
            ("createdat", false) => query.OrderBy(s => s.CreatedAt),
            ("createdat", true) => query.OrderByDescending(s => s.CreatedAt),
            _ => query.OrderBy(s => s.StyleCode),
        };

        var items = await orderedQuery.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();

        return new PagedResultDto<StyleDto>
        {
            Items = items.Select(ToDto),
            TotalCount = totalCount,
            Page = page,
            PageSize = pageSize,
            TotalPages = totalPages,
        };
    }

    private static StyleDto ToDto(Style item) => new()
    {
        ID = item.ID,
        StyleCode = item.StyleCode,
        Description = item.Description,
        FabricDescription = item.FabricDescription,
        FabricContent = item.FabricContent,
        Construction = item.Construction,
        Gsm = item.Gsm,
        WeightOz = item.WeightOz,
        Comments = item.Comments,
        Active = item.Active,
        CreatedAt = item.CreatedAt,
        UpdatedAt = item.UpdatedAt,
    };
}
