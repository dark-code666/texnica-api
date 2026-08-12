using Microsoft.EntityFrameworkCore;
using ERP.Api.Data;
using ERP.Api.Dtos;
using ERP.Api.Domain;
using ERP.Api.Interfaces;

namespace ERP.Api.Services;

public class CuttingReleaseService : ICuttingReleaseService
{
    private readonly ErpDbContext _context;

    public CuttingReleaseService(ErpDbContext context)
    {
        _context = context;
    }

    private async Task<string> GenerateReleaseNumberAsync()
    {
        var year = DateTime.UtcNow.Year;
        var prefix = $"REL-{year}-";
        var count = await _context.CuttingReleases.CountAsync(r => r.ReleaseNumber != null && r.ReleaseNumber.StartsWith(prefix));
        return $"{prefix}{(count + 1):D4}";
    }

    public async Task<IEnumerable<CuttingReleaseDto>> GetAllAsync()
    {
        var items = await _context.CuttingReleases
            .Include(r => r.FGPO)
                .ThenInclude(f => f!.Customer)
            .Where(r => r.Active)
            .OrderByDescending(r => r.ReleaseDate)
            .ToListAsync();

        return items.Select(ToDto);
    }

    public async Task<CuttingReleaseDto?> GetByIdAsync(int id)
    {
        var item = await _context.CuttingReleases
            .Include(r => r.FGPO)
                .ThenInclude(f => f!.Customer)
            .FirstOrDefaultAsync(r => r.ID == id && r.Active);

        return item is null ? null : ToDto(item);
    }

    public async Task<IEnumerable<CuttingReleaseDto>> GetByFgpoAsync(int fgpoId)
    {
        var items = await _context.CuttingReleases
            .Include(r => r.FGPO)
                .ThenInclude(f => f!.Customer)
            .Where(r => r.Active && r.FGPOId == fgpoId)
            .OrderByDescending(r => r.ReleaseDate)
            .ToListAsync();

        return items.Select(ToDto);
    }

    public async Task<CuttingReleaseDto> CreateAsync(CreateCuttingReleaseDto dto)
    {
        Validate(dto);

        var fgpo = await _context.Fgpos
            .FirstOrDefaultAsync(f => f.ID == dto.FGPOId && f.Active)
            ?? throw new Exception("El FGPO seleccionado no es válido.");

        var entity = new CuttingRelease
        {
            ReleaseNumber = await GenerateReleaseNumberAsync(),
            ReleaseDate = dto.ReleaseDate,
            FGPOId = dto.FGPOId,
            FabricLot = dto.FabricLot,
            ApprovedCutQty = dto.ApprovedCutQty,
            ApprovedWidth = dto.ApprovedWidth,
            MarkerNumber = dto.MarkerNumber,
            ApprovedYield = dto.ApprovedYield,
            PrrResult = dto.PrrResult,
            ReleasedByUserId = dto.ReleasedByUserId,
            ReviewedByUserId = dto.ReviewedByUserId,
            Exception = dto.Exception,
            Conditions = dto.Conditions,
            ReleaseStatus = dto.ReleaseStatus,
            Comments = dto.Comments,
            Active = true,
            CreatedAt = DateTime.UtcNow,
        };

        _context.CuttingReleases.Add(entity);
        await _context.SaveChangesAsync();

        await _context.Entry(entity).Reference(r => r.FGPO).Query()
            .Include(f => f!.Customer).LoadAsync();

        return ToDto(entity);
    }

    public async Task<bool> UpdateAsync(int id, UpdateCuttingReleaseDto dto)
    {
        var entity = await _context.CuttingReleases
            .FirstOrDefaultAsync(r => r.ID == id && r.Active);
        if (entity is null)
            return false;

        Validate(dto);

        var fgpo = await _context.Fgpos
            .FirstOrDefaultAsync(f => f.ID == dto.FGPOId && f.Active)
            ?? throw new Exception("El FGPO seleccionado no es válido.");

        entity.ReleaseDate = dto.ReleaseDate;
        entity.FGPOId = dto.FGPOId;
        entity.FabricLot = dto.FabricLot;
        entity.ApprovedCutQty = dto.ApprovedCutQty;
        entity.ApprovedWidth = dto.ApprovedWidth;
        entity.MarkerNumber = dto.MarkerNumber;
        entity.ApprovedYield = dto.ApprovedYield;
        entity.PrrResult = dto.PrrResult;
        entity.ReleasedByUserId = dto.ReleasedByUserId;
        entity.ReviewedByUserId = dto.ReviewedByUserId;
        entity.Exception = dto.Exception;
        entity.Conditions = dto.Conditions;
        entity.ReleaseStatus = dto.ReleaseStatus;
        entity.Comments = dto.Comments;
        entity.UpdatedAt = DateTime.UtcNow;

        _context.CuttingReleases.Update(entity);
        await _context.SaveChangesAsync();

        return true;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var entity = await _context.CuttingReleases
            .FirstOrDefaultAsync(r => r.ID == id && r.Active);
        if (entity is null)
            return false;

        entity.Active = false;
        entity.UpdatedAt = DateTime.UtcNow;
        _context.CuttingReleases.Update(entity);
        await _context.SaveChangesAsync();

        return true;
    }

    public async Task<PagedResultDto<CuttingReleaseDto>> GetPagedAsync(int page, int pageSize, string? search, string? sortBy, string? sortOrder, string? fgpo, string? status)
    {
        if (page < 1) page = 1;
        if (pageSize < 1) pageSize = 10;
        if (pageSize > 100) pageSize = 100;

        var query = _context.CuttingReleases
            .Include(r => r.FGPO)
                .ThenInclude(f => f!.Customer)
            .Where(r => r.Active);

        if (!string.IsNullOrWhiteSpace(search))
        {
            var term = search.Trim();
            query = query.Where(r =>
                (r.ReleaseNumber != null && r.ReleaseNumber.Contains(term)) ||
                (r.FabricLot != null && r.FabricLot.Contains(term)) ||
                (r.MarkerNumber != null && r.MarkerNumber.Contains(term)) ||
                (r.ReleaseStatus != null && r.ReleaseStatus.Contains(term)) ||
                (r.FGPO != null && r.FGPO.FGPONumber.Contains(term)));
        }

        if (!string.IsNullOrWhiteSpace(fgpo))
            query = query.Where(r => r.FGPO != null && r.FGPO.FGPONumber.Contains(fgpo.Trim()));

        if (!string.IsNullOrWhiteSpace(status))
            query = query.Where(r => r.ReleaseStatus != null && r.ReleaseStatus.Contains(status.Trim()));

        var totalCount = await query.CountAsync();
        var totalPages = (int)Math.Ceiling(totalCount / (double)pageSize);

        var sortByLower = sortBy?.ToLowerInvariant();
        var descending = sortOrder?.ToLowerInvariant() == "desc";

        IQueryable<CuttingRelease> orderedQuery = (sortByLower, descending) switch
        {
            ("releasedate", false) => query.OrderBy(r => r.ReleaseDate),
            ("releasedate", true) => query.OrderByDescending(r => r.ReleaseDate),
            ("releasenumber", false) => query.OrderBy(r => r.ReleaseNumber),
            ("releasenumber", true) => query.OrderByDescending(r => r.ReleaseNumber),
            ("releasestatus", false) => query.OrderBy(r => r.ReleaseStatus),
            ("releasestatus", true) => query.OrderByDescending(r => r.ReleaseStatus),
            ("createdat", false) => query.OrderBy(r => r.CreatedAt),
            ("createdat", true) => query.OrderByDescending(r => r.CreatedAt),
            _ => query.OrderByDescending(r => r.ReleaseDate),
        };

        var items = await orderedQuery
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return new PagedResultDto<CuttingReleaseDto>
        {
            Items = items.Select(ToDto),
            TotalCount = totalCount,
            Page = page,
            PageSize = pageSize,
            TotalPages = totalPages,
        };
    }

    private static void Validate(CreateCuttingReleaseDto dto)
    {
        if (dto.ReleaseDate == default)
            throw new Exception("La Release Date es obligatoria.");

        if (dto.ApprovedCutQty < 0)
            throw new Exception("La Approved Cut Qty no puede ser negativa.");

        if (dto.ApprovedWidth < 0)
            throw new Exception("La Approved Width no puede ser negativa.");

        if (dto.ApprovedYield < 0)
            throw new Exception("El Approved Yield no puede ser negativo.");
    }

    private static void Validate(UpdateCuttingReleaseDto dto)
    {
        if (dto.ReleaseDate == default)
            throw new Exception("La Release Date es obligatoria.");

        if (dto.ApprovedCutQty < 0)
            throw new Exception("La Approved Cut Qty no puede ser negativa.");

        if (dto.ApprovedWidth < 0)
            throw new Exception("La Approved Width no puede ser negativa.");

        if (dto.ApprovedYield < 0)
            throw new Exception("El Approved Yield no puede ser negativo.");
    }

    private static CuttingReleaseDto ToDto(CuttingRelease item)
    {
        return new CuttingReleaseDto
        {
            ID = item.ID,
            ReleaseNumber = item.ReleaseNumber ?? string.Empty,
            ReleaseDate = item.ReleaseDate,
            FGPOId = item.FGPOId,
            FGPONumber = item.FGPO?.FGPONumber ?? string.Empty,
            CustomerName = item.FGPO?.Customer?.Name ?? string.Empty,
            Style = item.FGPO?.FgpoLines?.FirstOrDefault(l => l.Active)?.Style?.StyleCode,
            Color = item.FGPO?.FgpoLines?.FirstOrDefault(l => l.Active)?.Color?.ColorName,
            FabricLot = item.FabricLot,
            ApprovedCutQty = item.ApprovedCutQty,
            ApprovedWidth = item.ApprovedWidth,
            MarkerNumber = item.MarkerNumber,
            ApprovedYield = item.ApprovedYield,
            PrrResult = item.PrrResult,
            ReleasedBy = item.ReleasedBy?.UserName,
            ReviewedBy = item.ReviewedBy?.UserName,
            Exception = item.Exception,
            Conditions = item.Conditions,
            ReleaseStatus = item.ReleaseStatus,
            Comments = item.Comments,
            Active = item.Active,
            CreatedAt = item.CreatedAt,
            UpdatedAt = item.UpdatedAt,
        };
    }
}
