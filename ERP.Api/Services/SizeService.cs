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
            Description = dto.Description?.Trim(),
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
        entity.Description = dto.Description?.Trim();
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
            query = query.Where(s => s.SizeCode.Contains(t) || (s.Description != null && s.Description.Contains(t)));
        }
        var items = await query.OrderBy(s => s.SortOrder).ToListAsync();
        return items.Select(ToDto);
    }

    public async Task<PagedResultDto<SizeDto>> GetPagedAsync(int page, int pageSize, string? search, string? sortBy, string? sortOrder)
    {
        if (page < 1) page = 1;
        if (pageSize < 1) pageSize = 10;
        if (pageSize > 100) pageSize = 100;

        var query = _context.Sizes.Where(s => s.Active);

        if (!string.IsNullOrWhiteSpace(search))
        {
            var term = search.Trim();
            query = query.Where(s => s.SizeCode.Contains(term) || (s.Description != null && s.Description.Contains(term)));
        }

        var totalCount = await query.CountAsync();
        var totalPages = (int)Math.Ceiling(totalCount / (double)pageSize);

        var sortByLower = sortBy?.ToLowerInvariant();
        var descending = sortOrder?.ToLowerInvariant() == "desc";

        IQueryable<Size> orderedQuery = (sortByLower, descending) switch
        {
            ("sizecode", false) => query.OrderBy(s => s.SizeCode),
            ("sizecode", true) => query.OrderByDescending(s => s.SizeCode),
            ("sortorder", false) => query.OrderBy(s => s.SortOrder),
            ("sortorder", true) => query.OrderByDescending(s => s.SortOrder),
            ("createdat", false) => query.OrderBy(s => s.CreatedAt),
            ("createdat", true) => query.OrderByDescending(s => s.CreatedAt),
            _ => query.OrderBy(s => s.SortOrder),
        };

        var items = await orderedQuery.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();

        return new PagedResultDto<SizeDto>
        {
            Items = items.Select(ToDto),
            TotalCount = totalCount,
            Page = page,
            PageSize = pageSize,
            TotalPages = totalPages,
        };
    }

    private static SizeDto ToDto(Size item) => new()
    {
        ID = item.ID,
        SizeCode = item.SizeCode,
        Description = item.Description,
        SortOrder = item.SortOrder,
        Active = item.Active,
        CreatedAt = item.CreatedAt,
        UpdatedAt = item.UpdatedAt,
    };
}
