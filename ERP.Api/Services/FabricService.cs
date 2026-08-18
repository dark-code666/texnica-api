using Microsoft.EntityFrameworkCore;
using ERP.Api.Data;
using ERP.Api.Dtos;
using ERP.Api.Domain;
using ERP.Api.Interfaces;

namespace ERP.Api.Services;

public class FabricService : IFabricService
{
    private readonly ErpDbContext _context;

    public FabricService(ErpDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<FabricDto>> GetAllAsync()
    {
        var items = await _context.Fabrics
            .Where(f => f.Active)
            .OrderBy(f => f.FabricName)
            .ToListAsync();
        return items.Select(ToDto);
    }

    public async Task<FabricDto?> GetByIdAsync(int id)
    {
        var item = await _context.Fabrics.FirstOrDefaultAsync(f => f.ID == id && f.Active);
        return item is null ? null : ToDto(item);
    }

    public async Task<FabricDto> CreateAsync(CreateFabricDto dto)
    {
        if (string.IsNullOrWhiteSpace(dto.FabricName))
            throw new Exception("El Fabric Name es obligatorio.");

        var exists = await _context.Fabrics.AnyAsync(f => f.FabricName == dto.FabricName && f.Color == dto.Color);
        if (exists)
            throw new Exception("El Fabric ya existe.");

        var entity = new Fabric
        {
            FabricReference = dto.FabricReference,
            FabricName = dto.FabricName.Trim(),
            Color = dto.Color,
            Content = dto.Content,
            Construction = dto.Construction,
            Gsm = dto.Gsm,
            WeightOz = dto.WeightOz,
            Comments = dto.Comments,
            Active = true,
            CreatedAt = DateTime.UtcNow,
        };

        _context.Fabrics.Add(entity);
        await _context.SaveChangesAsync();
        return ToDto(entity);
    }

    public async Task<bool> UpdateAsync(int id, UpdateFabricDto dto)
    {
        var entity = await _context.Fabrics.FirstOrDefaultAsync(f => f.ID == id && f.Active);
        if (entity is null)
            return false;

        if (string.IsNullOrWhiteSpace(dto.FabricName))
            throw new Exception("El Fabric Name es obligatorio.");

        var exists = await _context.Fabrics.AnyAsync(f => f.FabricName == dto.FabricName && f.Color == dto.Color && f.ID != id);
        if (exists)
            throw new Exception("El Fabric ya existe.");

        entity.FabricReference = dto.FabricReference;
        entity.FabricName = dto.FabricName.Trim();
        entity.Color = dto.Color;
        entity.Content = dto.Content;
        entity.Construction = dto.Construction;
        entity.Gsm = dto.Gsm;
        entity.WeightOz = dto.WeightOz;
        entity.Comments = dto.Comments;
        entity.UpdatedAt = DateTime.UtcNow;

        _context.Fabrics.Update(entity);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var entity = await _context.Fabrics.FirstOrDefaultAsync(f => f.ID == id && f.Active);
        if (entity is null)
            return false;

        entity.Active = false;
        entity.UpdatedAt = DateTime.UtcNow;
        _context.Fabrics.Update(entity);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<IEnumerable<FabricDto>> SearchAsync(string? term)
    {
        var query = _context.Fabrics.Where(f => f.Active);
        if (!string.IsNullOrWhiteSpace(term))
        {
            var t = term.Trim();
            query = query.Where(f => f.FabricName.Contains(t) || (f.Color != null && f.Color.Contains(t)) || (f.FabricReference != null && f.FabricReference.Contains(t)));
        }
        var items = await query.OrderBy(f => f.FabricName).ToListAsync();
        return items.Select(ToDto);
    }

    public async Task<PagedResultDto<FabricDto>> GetPagedAsync(int page, int pageSize, string? search, string? sortBy, string? sortOrder)
    {
        if (page < 1) page = 1;
        if (pageSize < 1) pageSize = 10;
        if (pageSize > 100) pageSize = 100;

        var query = _context.Fabrics.Where(f => f.Active);

        if (!string.IsNullOrWhiteSpace(search))
        {
            var term = search.Trim();
            query = query.Where(f => f.FabricName.Contains(term)
                || (f.FabricReference != null && f.FabricReference.Contains(term))
                || (f.Color != null && f.Color.Contains(term))
                || (f.Content != null && f.Content.Contains(term)));
        }

        var totalCount = await query.CountAsync();
        var totalPages = (int)Math.Ceiling(totalCount / (double)pageSize);

        var sortByLower = sortBy?.ToLowerInvariant();
        var descending = sortOrder?.ToLowerInvariant() == "desc";

        IQueryable<Fabric> orderedQuery = (sortByLower, descending) switch
        {
            ("fabricname", false) => query.OrderBy(f => f.FabricName),
            ("fabricname", true) => query.OrderByDescending(f => f.FabricName),
            ("createdat", false) => query.OrderBy(f => f.CreatedAt),
            ("createdat", true) => query.OrderByDescending(f => f.CreatedAt),
            _ => query.OrderBy(f => f.FabricName),
        };

        var items = await orderedQuery.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();

        return new PagedResultDto<FabricDto>
        {
            Items = items.Select(ToDto),
            TotalCount = totalCount,
            Page = page,
            PageSize = pageSize,
            TotalPages = totalPages,
        };
    }

    private static FabricDto ToDto(Fabric item) => new()
    {
        ID = item.ID,
        FabricReference = item.FabricReference,
        FabricName = item.FabricName,
        Color = item.Color,
        Content = item.Content,
        Construction = item.Construction,
        Gsm = item.Gsm,
        WeightOz = item.WeightOz,
        Comments = item.Comments,
        Active = item.Active,
        CreatedAt = item.CreatedAt,
        UpdatedAt = item.UpdatedAt,
    };
}
