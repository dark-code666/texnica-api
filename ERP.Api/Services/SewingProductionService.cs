using Microsoft.EntityFrameworkCore;
using ERP.Api.Data;
using ERP.Api.Dtos;
using ERP.Api.Domain;
using ERP.Api.Interfaces;

namespace ERP.Api.Services;

public class SewingProductionService : ISewingProductionService
{
    private readonly ErpDbContext _context;

    public SewingProductionService(ErpDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<SewingProductionDto>> GetAllAsync()
    {
        var items = await _context.SewingProductions
            .Include(s => s.FGPO).ThenInclude(f => f!.Customer)
            .Include(s => s.Size)
            .Include(s => s.Supervisor)
            .Where(s => s.Active)
            .OrderByDescending(s => s.ProductionDate)
            .ToListAsync();
        return items.Select(ToDto);
    }

    public async Task<SewingProductionDto?> GetByIdAsync(int id)
    {
        var item = await _context.SewingProductions
            .Include(s => s.FGPO).ThenInclude(f => f!.Customer)
            .Include(s => s.Size)
            .Include(s => s.Supervisor)
            .FirstOrDefaultAsync(s => s.ID == id && s.Active);
        return item is null ? null : ToDto(item);
    }

    public async Task<IEnumerable<SewingProductionDto>> GetByFgpoAsync(int fgpoId)
    {
        var items = await _context.SewingProductions
            .Include(s => s.FGPO).ThenInclude(f => f!.Customer)
            .Include(s => s.Size)
            .Include(s => s.Supervisor)
            .Where(s => s.Active && s.FGPOId == fgpoId)
            .OrderByDescending(s => s.ProductionDate)
            .ToListAsync();
        return items.Select(ToDto);
    }

    public async Task<SewingProductionDto> CreateAsync(CreateSewingProductionDto dto)
    {
        Validate(dto);
        var fgpo = await _context.Fgpos.FirstOrDefaultAsync(f => f.ID == dto.FGPOId && f.Active)
            ?? throw new Exception("El FGPO seleccionado no es válido.");
        await ValidateSupervisorAsync(dto.SupervisorId);
        var sizeId = await ResolveSizeFromFgpoAsync(dto.FGPOId);

        var entity = new SewingProduction
        {
            ProductionDate = dto.ProductionDate,
            Shift = dto.Shift,
            Line = dto.Line,
            FGPOId = dto.FGPOId,
            SizeId = sizeId,
            SewingInput = dto.SewingInput,
            DailyTarget = dto.DailyTarget,
            DailyOutput = dto.DailyOutput,
            CumulativeOutput = dto.CumulativeOutput,
            Wip = dto.Wip,
            Rework = dto.Rework,
            Reject = dto.Reject,
            DowntimeMinutes = dto.DowntimeMinutes,
            TopStatus = dto.TopStatus,
            SupervisorId = dto.SupervisorId,
            Remarks = dto.Remarks,
            Active = true,
            CreatedAt = DateTime.UtcNow,
        };

        _context.SewingProductions.Add(entity);
        await _context.SaveChangesAsync();
        await _context.Entry(entity).Reference(s => s.FGPO).Query().Include(f => f!.Customer).LoadAsync();
        await _context.Entry(entity).Reference(s => s.Size).LoadAsync();
        return ToDto(entity);
    }

    public async Task<bool> UpdateAsync(int id, UpdateSewingProductionDto dto)
    {
        var entity = await _context.SewingProductions.FirstOrDefaultAsync(s => s.ID == id && s.Active);
        if (entity is null)
            return false;

        Validate(dto);
        var fgpo = await _context.Fgpos.FirstOrDefaultAsync(f => f.ID == dto.FGPOId && f.Active)
            ?? throw new Exception("El FGPO seleccionado no es válido.");
        await ValidateSupervisorAsync(dto.SupervisorId);
        var sizeId = await ResolveSizeFromFgpoAsync(dto.FGPOId);

        entity.ProductionDate = dto.ProductionDate;
        entity.Shift = dto.Shift;
        entity.Line = dto.Line;
        entity.FGPOId = dto.FGPOId;
        entity.SizeId = sizeId;
        entity.SewingInput = dto.SewingInput;
        entity.DailyTarget = dto.DailyTarget;
        entity.DailyOutput = dto.DailyOutput;
        entity.CumulativeOutput = dto.CumulativeOutput;
        entity.Wip = dto.Wip;
        entity.Rework = dto.Rework;
        entity.Reject = dto.Reject;
        entity.DowntimeMinutes = dto.DowntimeMinutes;
        entity.TopStatus = dto.TopStatus;
        entity.SupervisorId = dto.SupervisorId;
        entity.Remarks = dto.Remarks;
        entity.UpdatedAt = DateTime.UtcNow;

        _context.SewingProductions.Update(entity);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var entity = await _context.SewingProductions.FirstOrDefaultAsync(s => s.ID == id && s.Active);
        if (entity is null)
            return false;

        entity.Active = false;
        entity.UpdatedAt = DateTime.UtcNow;
        _context.SewingProductions.Update(entity);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<PagedResultDto<SewingProductionDto>> GetPagedAsync(int page, int pageSize, string? search, string? sortBy, string? sortOrder, string? fgpo, string? line)
    {
        if (page < 1) page = 1;
        if (pageSize < 1) pageSize = 10;
        if (pageSize > 100) pageSize = 100;

        var query = _context.SewingProductions
            .Include(s => s.FGPO).ThenInclude(f => f!.Customer)
            .Include(s => s.Size)
            .Include(s => s.Supervisor)
            .Where(s => s.Active);

        if (!string.IsNullOrWhiteSpace(search))
        {
            var term = search.Trim();
            query = query.Where(s =>
                (s.Line != null && s.Line.Contains(term)) ||
                (s.Shift != null && s.Shift.Contains(term)) ||
                (s.Size != null && s.Size.SizeCode.Contains(term)) ||
                (s.Supervisor != null && s.Supervisor.UserName.Contains(term)) ||
                (s.TopStatus != null && s.TopStatus.Contains(term)) ||
                (s.FGPO != null && s.FGPO.FGPONumber.Contains(term)));
        }

        if (!string.IsNullOrWhiteSpace(fgpo))
            query = query.Where(s => s.FGPO != null && s.FGPO.FGPONumber.Contains(fgpo.Trim()));

        if (!string.IsNullOrWhiteSpace(line))
            query = query.Where(s => s.Line != null && s.Line.Contains(line.Trim()));

        var totalCount = await query.CountAsync();
        var totalPages = (int)Math.Ceiling(totalCount / (double)pageSize);

        var sortByLower = sortBy?.ToLowerInvariant();
        var descending = sortOrder?.ToLowerInvariant() == "desc";

        IQueryable<SewingProduction> orderedQuery = (sortByLower, descending) switch
        {
            ("productiondate", false) => query.OrderBy(s => s.ProductionDate),
            ("productiondate", true) => query.OrderByDescending(s => s.ProductionDate),
            ("line", false) => query.OrderBy(s => s.Line),
            ("line", true) => query.OrderByDescending(s => s.Line),
            ("targetachievementpct", false) => query.OrderBy(s => s.TargetAchievementPct),
            ("targetachievementpct", true) => query.OrderByDescending(s => s.TargetAchievementPct),
            ("createdat", false) => query.OrderBy(s => s.CreatedAt),
            ("createdat", true) => query.OrderByDescending(s => s.CreatedAt),
            _ => query.OrderByDescending(s => s.ProductionDate),
        };

        var items = await orderedQuery
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return new PagedResultDto<SewingProductionDto>
        {
            Items = items.Select(ToDto),
            TotalCount = totalCount,
            Page = page,
            PageSize = pageSize,
            TotalPages = totalPages,
        };
    }

    private async Task<int?> ResolveSizeFromFgpoAsync(int fgpoId) =>
        await _context.FgpoLines
            .Where(l => l.FgpoId == fgpoId && l.Active)
            .OrderBy(l => l.ID)
            .Select(l => (int?)l.SizeId)
            .FirstOrDefaultAsync();

    private async Task ValidateSupervisorAsync(int? supervisorId)
    {
        if (supervisorId.HasValue && await _context.Users.FirstOrDefaultAsync(u => u.ID == supervisorId.Value && u.Active) is null)
            throw new Exception("El Supervisor seleccionado no es válido.");
    }

    private static void Validate(CreateSewingProductionDto dto)
    {
        if (dto.ProductionDate == default)
            throw new Exception("La Date es obligatoria.");

        if (dto.SewingInput < 0 || dto.DailyTarget < 0 || dto.DailyOutput < 0 || dto.CumulativeOutput < 0 || dto.Wip < 0 || dto.Rework < 0 || dto.Reject < 0 || dto.DowntimeMinutes < 0)
            throw new Exception("Las cantidades no pueden ser negativas.");
    }

    private static void Validate(UpdateSewingProductionDto dto)
    {
        if (dto.ProductionDate == default)
            throw new Exception("La Date es obligatoria.");

        if (dto.SewingInput < 0 || dto.DailyTarget < 0 || dto.DailyOutput < 0 || dto.CumulativeOutput < 0 || dto.Wip < 0 || dto.Rework < 0 || dto.Reject < 0 || dto.DowntimeMinutes < 0)
            throw new Exception("Las cantidades no pueden ser negativas.");
    }

    private static SewingProductionDto ToDto(SewingProduction item) => new()
    {
        ID = item.ID,
        ProductionDate = item.ProductionDate,
        Shift = item.Shift,
        Line = item.Line,
        FGPOId = item.FGPOId,
        FgpoNumber = item.FGPO?.FGPONumber ?? string.Empty,
        SizeId = item.SizeId,
        SizeCode = item.Size?.SizeCode ?? string.Empty,
        SewingInput = item.SewingInput,
        DailyTarget = item.DailyTarget,
        DailyOutput = item.DailyOutput,
        CumulativeOutput = item.CumulativeOutput,
        Wip = item.Wip,
        Rework = item.Rework,
        Reject = item.Reject,
        DowntimeMinutes = item.DowntimeMinutes,
        TargetAchievementPct = item.TargetAchievementPct,
        SewingVariance = item.SewingVariance,
        PendingSewing = item.PendingSewing,
        Overproduction = item.Overproduction,
        TopStatus = item.TopStatus,
        SupervisorId = item.SupervisorId,
        SupervisorName = item.Supervisor?.UserName ?? string.Empty,
        Remarks = item.Remarks,
        Active = item.Active,
        CreatedAt = item.CreatedAt,
        UpdatedAt = item.UpdatedAt,
    };
}
