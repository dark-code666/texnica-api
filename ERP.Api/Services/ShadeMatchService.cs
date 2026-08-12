using Microsoft.EntityFrameworkCore;
using ERP.Api.Data;
using ERP.Api.Dtos;
using ERP.Api.Domain;
using ERP.Api.Interfaces;

namespace ERP.Api.Services;

public class ShadeMatchService : IShadeMatchService
{
    private readonly ErpDbContext _context;

    public ShadeMatchService(ErpDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<ShadeMatchDto>> GetAllAsync()
    {
        var items = await _context.ShadeMatches
            .Include(s => s.FGPO)
                .ThenInclude(f => f!.Customer)
            .Where(s => s.Active)
            .OrderByDescending(s => s.ReviewDate)
            .ToListAsync();

        return items.Select(ToDto);
    }

    public async Task<ShadeMatchDto?> GetByIdAsync(int id)
    {
        var item = await _context.ShadeMatches
            .Include(s => s.FGPO)
                .ThenInclude(f => f!.Customer)
            .FirstOrDefaultAsync(s => s.ID == id && s.Active);

        return item is null ? null : ToDto(item);
    }

    public async Task<IEnumerable<ShadeMatchDto>> GetByFgpoAsync(int fgpoId)
    {
        var items = await _context.ShadeMatches
            .Include(s => s.FGPO)
                .ThenInclude(f => f!.Customer)
            .Where(s => s.Active && s.FGPOId == fgpoId)
            .OrderByDescending(s => s.ReviewDate)
            .ToListAsync();

        return items.Select(ToDto);
    }

    public async Task<ShadeMatchDto> CreateAsync(CreateShadeMatchDto dto)
    {
        Validate(dto);

        var fgpo = await _context.Fgpos
            .FirstOrDefaultAsync(f => f.ID == dto.FGPOId && f.Active)
            ?? throw new Exception("El FGPO seleccionado no es válido.");

        var entity = new ShadeMatch
        {
            ReviewDate = dto.ReviewDate,
            FGPOId = dto.FGPOId,
            BodyFabricLot = dto.BodyFabricLot,
            RibLot = dto.RibLot,
            ShoulderTapeLot = dto.ShoulderTapeLot,
            BodyShadeGroup = dto.BodyShadeGroup,
            RibShadeGroup = dto.RibShadeGroup,
            TapeShadeGroup = dto.TapeShadeGroup,
            BodyVsRib = dto.BodyVsRib,
            BodyVsTape = dto.BodyVsTape,
            LightSource = dto.LightSource,
            BeforeWashResult = dto.BeforeWashResult,
            AfterWashResult = dto.AfterWashResult,
            OverallResult = dto.OverallResult,
            ApprovedBy = dto.ApprovedBy,
            ReportLink = dto.ReportLink,
            Comments = dto.Comments,
            Active = true,
            CreatedAt = DateTime.UtcNow,
        };

        _context.ShadeMatches.Add(entity);
        await _context.SaveChangesAsync();

        await _context.Entry(entity).Reference(s => s.FGPO).Query()
            .Include(f => f!.Customer).LoadAsync();

        return ToDto(entity);
    }

    public async Task<bool> UpdateAsync(int id, UpdateShadeMatchDto dto)
    {
        var entity = await _context.ShadeMatches
            .FirstOrDefaultAsync(s => s.ID == id && s.Active);
        if (entity is null)
            return false;

        Validate(dto);

        var fgpo = await _context.Fgpos
            .FirstOrDefaultAsync(f => f.ID == dto.FGPOId && f.Active)
            ?? throw new Exception("El FGPO seleccionado no es válido.");

        entity.ReviewDate = dto.ReviewDate;
        entity.FGPOId = dto.FGPOId;
        entity.BodyFabricLot = dto.BodyFabricLot;
        entity.RibLot = dto.RibLot;
        entity.ShoulderTapeLot = dto.ShoulderTapeLot;
        entity.BodyShadeGroup = dto.BodyShadeGroup;
        entity.RibShadeGroup = dto.RibShadeGroup;
        entity.TapeShadeGroup = dto.TapeShadeGroup;
        entity.BodyVsRib = dto.BodyVsRib;
        entity.BodyVsTape = dto.BodyVsTape;
        entity.LightSource = dto.LightSource;
        entity.BeforeWashResult = dto.BeforeWashResult;
        entity.AfterWashResult = dto.AfterWashResult;
        entity.OverallResult = dto.OverallResult;
        entity.ApprovedBy = dto.ApprovedBy;
        entity.ReportLink = dto.ReportLink;
        entity.Comments = dto.Comments;
        entity.UpdatedAt = DateTime.UtcNow;

        _context.ShadeMatches.Update(entity);
        await _context.SaveChangesAsync();

        return true;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var entity = await _context.ShadeMatches
            .FirstOrDefaultAsync(s => s.ID == id && s.Active);
        if (entity is null)
            return false;

        entity.Active = false;
        entity.UpdatedAt = DateTime.UtcNow;
        _context.ShadeMatches.Update(entity);
        await _context.SaveChangesAsync();

        return true;
    }

    public async Task<PagedResultDto<ShadeMatchDto>> GetPagedAsync(int page, int pageSize, string? search, string? sortBy, string? sortOrder, string? fgpo, string? result)
    {
        if (page < 1) page = 1;
        if (pageSize < 1) pageSize = 10;
        if (pageSize > 100) pageSize = 100;

        var query = _context.ShadeMatches
            .Include(s => s.FGPO)
                .ThenInclude(f => f!.Customer)
            .Where(s => s.Active);

        if (!string.IsNullOrWhiteSpace(search))
        {
            var term = search.Trim();
            query = query.Where(s =>
                (s.BodyFabricLot != null && s.BodyFabricLot.Contains(term)) ||
                (s.RibLot != null && s.RibLot.Contains(term)) ||
                (s.BodyShadeGroup != null && s.BodyShadeGroup.Contains(term)) ||
                (s.OverallResult != null && s.OverallResult.Contains(term)) ||
                (s.FGPO != null && s.FGPO.FGPONumber.Contains(term)) ||
                (s.FGPO != null && s.FGPO.FgpoLines != null && s.FGPO.FgpoLines.Any(l => l.Style != null && l.Style.StyleCode.Contains(term))));
        }

        if (!string.IsNullOrWhiteSpace(fgpo))
            query = query.Where(s => s.FGPO != null && s.FGPO.FGPONumber.Contains(fgpo.Trim()));

        if (!string.IsNullOrWhiteSpace(result))
            query = query.Where(s => s.OverallResult != null && s.OverallResult.Contains(result.Trim()));

        var totalCount = await query.CountAsync();
        var totalPages = (int)Math.Ceiling(totalCount / (double)pageSize);

        var sortByLower = sortBy?.ToLowerInvariant();
        var descending = sortOrder?.ToLowerInvariant() == "desc";

        IQueryable<ShadeMatch> orderedQuery = (sortByLower, descending) switch
        {
            ("reviewdate", false) => query.OrderBy(s => s.ReviewDate),
            ("reviewdate", true) => query.OrderByDescending(s => s.ReviewDate),
            ("bodyfabriclot", false) => query.OrderBy(s => s.BodyFabricLot),
            ("bodyfabriclot", true) => query.OrderByDescending(s => s.BodyFabricLot),
            ("overallresult", false) => query.OrderBy(s => s.OverallResult),
            ("overallresult", true) => query.OrderByDescending(s => s.OverallResult),
            ("createdat", false) => query.OrderBy(s => s.CreatedAt),
            ("createdat", true) => query.OrderByDescending(s => s.CreatedAt),
            _ => query.OrderByDescending(s => s.ReviewDate),
        };

        var items = await orderedQuery
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return new PagedResultDto<ShadeMatchDto>
        {
            Items = items.Select(ToDto),
            TotalCount = totalCount,
            Page = page,
            PageSize = pageSize,
            TotalPages = totalPages,
        };
    }

    private static void Validate(CreateShadeMatchDto dto)
    {
        if (dto.ReviewDate == default)
            throw new Exception("La Review Date es obligatoria.");
    }

    private static void Validate(UpdateShadeMatchDto dto)
    {
        if (dto.ReviewDate == default)
            throw new Exception("La Review Date es obligatoria.");
    }

    private static ShadeMatchDto ToDto(ShadeMatch item)
    {
        return new ShadeMatchDto
        {
            ID = item.ID,
            ReviewDate = item.ReviewDate,
            FGPOId = item.FGPOId,
            FGPONumber = item.FGPO?.FGPONumber ?? string.Empty,
            CustomerName = item.FGPO?.Customer?.Name ?? string.Empty,
            Style = item.FGPO?.FgpoLines?.FirstOrDefault(l => l.Active)?.Style?.StyleCode,
            Color = item.FGPO?.FgpoLines?.FirstOrDefault(l => l.Active)?.Color?.ColorName,
            BodyFabricLot = item.BodyFabricLot,
            RibLot = item.RibLot,
            ShoulderTapeLot = item.ShoulderTapeLot,
            BodyShadeGroup = item.BodyShadeGroup,
            RibShadeGroup = item.RibShadeGroup,
            TapeShadeGroup = item.TapeShadeGroup,
            BodyVsRib = item.BodyVsRib,
            BodyVsTape = item.BodyVsTape,
            LightSource = item.LightSource,
            BeforeWashResult = item.BeforeWashResult,
            AfterWashResult = item.AfterWashResult,
            OverallResult = item.OverallResult,
            ApprovedBy = item.ApprovedBy,
            ReportLink = item.ReportLink,
            Comments = item.Comments,
            Active = item.Active,
            CreatedAt = item.CreatedAt,
            UpdatedAt = item.UpdatedAt,
        };
    }
}
