using Microsoft.EntityFrameworkCore;
using ERP.Api.Data;
using ERP.Api.Dtos;
using ERP.Api.Domain;
using ERP.Api.Interfaces;

namespace ERP.Api.Services;

public class InlineQualityService : IInlineQualityService
{
    private readonly ErpDbContext _context;

    public InlineQualityService(ErpDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<InlineQualityDto>> GetAllAsync()
    {
        var items = await _context.InlineQualities
            .Include(i => i.FGPO)
                .ThenInclude(f => f!.Customer)
            .Where(i => i.Active)
            .OrderByDescending(i => i.InspectionDate)
            .ToListAsync();

        return items.Select(ToDto);
    }

    public async Task<InlineQualityDto?> GetByIdAsync(int id)
    {
        var item = await _context.InlineQualities
            .Include(i => i.FGPO)
                .ThenInclude(f => f!.Customer)
            .FirstOrDefaultAsync(i => i.ID == id && i.Active);

        return item is null ? null : ToDto(item);
    }

    public async Task<IEnumerable<InlineQualityDto>> GetByFgpoAsync(int fgpoId)
    {
        var items = await _context.InlineQualities
            .Include(i => i.FGPO)
                .ThenInclude(f => f!.Customer)
            .Where(i => i.Active && i.FGPOId == fgpoId)
            .OrderByDescending(i => i.InspectionDate)
            .ToListAsync();

        return items.Select(ToDto);
    }

    public async Task<IEnumerable<InlineQualityDto>> GetByLineAsync(string line)
    {
        var items = await _context.InlineQualities
            .Include(i => i.FGPO)
                .ThenInclude(f => f!.Customer)
            .Where(i => i.Active && i.Line != null && i.Line.Contains(line))
            .OrderByDescending(i => i.InspectionDate)
            .ToListAsync();

        return items.Select(ToDto);
    }

    public async Task<InlineQualityDto> CreateAsync(CreateInlineQualityDto dto)
    {
        Validate(dto);

        var fgpo = await _context.Fgpos
            .FirstOrDefaultAsync(f => f.ID == dto.FGPOId && f.Active)
            ?? throw new Exception("El FGPO seleccionado no es válido.");

        var entity = new InlineQuality
        {
            InspectionDate = dto.InspectionDate,
            Time = dto.Time,
            Line = dto.Line,
            FGPOId = dto.FGPOId,
            Operation = dto.Operation,
            Operator = dto.Operator,
            CheckedQty = dto.CheckedQty,
            CriticalDefects = dto.CriticalDefects,
            MajorDefects = dto.MajorDefects,
            MinorDefects = dto.MinorDefects,
            DefectivePieces = dto.DefectivePieces,
            MaxAllowed = dto.MaxAllowed,
            Inspector = dto.Inspector,
            ImmediateCorrection = dto.ImmediateCorrection,
            RootCause = dto.RootCause,
            Active = true,
            CreatedAt = DateTime.UtcNow,
        };

        _context.InlineQualities.Add(entity);
        await _context.SaveChangesAsync();

        await _context.Entry(entity).Reference(i => i.FGPO).Query()
            .Include(f => f!.Customer).LoadAsync();

        return ToDto(entity);
    }

    public async Task<bool> UpdateAsync(int id, UpdateInlineQualityDto dto)
    {
        var entity = await _context.InlineQualities
            .FirstOrDefaultAsync(i => i.ID == id && i.Active);
        if (entity is null)
            return false;

        Validate(dto);

        var fgpo = await _context.Fgpos
            .FirstOrDefaultAsync(f => f.ID == dto.FGPOId && f.Active)
            ?? throw new Exception("El FGPO seleccionado no es válido.");

        entity.InspectionDate = dto.InspectionDate;
        entity.Time = dto.Time;
        entity.Line = dto.Line;
        entity.FGPOId = dto.FGPOId;
        entity.Operation = dto.Operation;
        entity.Operator = dto.Operator;
        entity.CheckedQty = dto.CheckedQty;
        entity.CriticalDefects = dto.CriticalDefects;
        entity.MajorDefects = dto.MajorDefects;
        entity.MinorDefects = dto.MinorDefects;
        entity.DefectivePieces = dto.DefectivePieces;
        entity.MaxAllowed = dto.MaxAllowed;
        entity.Inspector = dto.Inspector;
        entity.ImmediateCorrection = dto.ImmediateCorrection;
        entity.RootCause = dto.RootCause;
        entity.UpdatedAt = DateTime.UtcNow;

        _context.InlineQualities.Update(entity);
        await _context.SaveChangesAsync();

        return true;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var entity = await _context.InlineQualities
            .FirstOrDefaultAsync(i => i.ID == id && i.Active);
        if (entity is null)
            return false;

        entity.Active = false;
        entity.UpdatedAt = DateTime.UtcNow;
        _context.InlineQualities.Update(entity);
        await _context.SaveChangesAsync();

        return true;
    }

    public async Task<PagedResultDto<InlineQualityDto>> GetPagedAsync(int page, int pageSize, string? search, string? sortBy, string? sortOrder, string? fgpo, string? line, string? result)
    {
        if (page < 1) page = 1;
        if (pageSize < 1) pageSize = 10;
        if (pageSize > 100) pageSize = 100;

        var query = _context.InlineQualities
            .Include(i => i.FGPO)
                .ThenInclude(f => f!.Customer)
            .Where(i => i.Active);

        if (!string.IsNullOrWhiteSpace(search))
        {
            var term = search.Trim();
            query = query.Where(i =>
                (i.Line != null && i.Line.Contains(term)) ||
                (i.Operation != null && i.Operation.Contains(term)) ||
                (i.Operator != null && i.Operator.Contains(term)) ||
                (i.Result != null && i.Result.Contains(term)) ||
                (i.FGPO != null && i.FGPO.FGPONumber.Contains(term)));
        }

        if (!string.IsNullOrWhiteSpace(fgpo))
            query = query.Where(i => i.FGPO != null && i.FGPO.FGPONumber.Contains(fgpo.Trim()));

        if (!string.IsNullOrWhiteSpace(line))
            query = query.Where(i => i.Line != null && i.Line.Contains(line.Trim()));

        if (!string.IsNullOrWhiteSpace(result))
            query = query.Where(i => i.Result != null && i.Result.Contains(result.Trim()));

        var totalCount = await query.CountAsync();
        var totalPages = (int)Math.Ceiling(totalCount / (double)pageSize);

        var sortByLower = sortBy?.ToLowerInvariant();
        var descending = sortOrder?.ToLowerInvariant() == "desc";

        IQueryable<InlineQuality> orderedQuery = (sortByLower, descending) switch
        {
            ("inspectiondate", false) => query.OrderBy(i => i.InspectionDate),
            ("inspectiondate", true) => query.OrderByDescending(i => i.InspectionDate),
            ("line", false) => query.OrderBy(i => i.Line),
            ("line", true) => query.OrderByDescending(i => i.Line),
            ("dhu", false) => query.OrderBy(i => i.DhuPct),
            ("dhu", true) => query.OrderByDescending(i => i.DhuPct),
            ("defectiveratepct", false) => query.OrderBy(i => i.DefectiveRatePct),
            ("defectiveratepct", true) => query.OrderByDescending(i => i.DefectiveRatePct),
            ("result", false) => query.OrderBy(i => i.Result),
            ("result", true) => query.OrderByDescending(i => i.Result),
            ("createdat", false) => query.OrderBy(i => i.CreatedAt),
            ("createdat", true) => query.OrderByDescending(i => i.CreatedAt),
            _ => query.OrderByDescending(i => i.InspectionDate),
        };

        var items = await orderedQuery
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return new PagedResultDto<InlineQualityDto>
        {
            Items = items.Select(ToDto),
            TotalCount = totalCount,
            Page = page,
            PageSize = pageSize,
            TotalPages = totalPages,
        };
    }

    private static void Validate(CreateInlineQualityDto dto)
    {
        if (dto.InspectionDate == default)
            throw new Exception("La Inspection Date es obligatoria.");

        if (dto.CheckedQty <= 0)
            throw new Exception("El Checked Qty debe ser mayor a 0.");

        if (dto.CriticalDefects < 0 || dto.MajorDefects < 0 || dto.MinorDefects < 0)
            throw new Exception("Los defectos no pueden ser negativos.");

        if (dto.MaxAllowed < 0)
            throw new Exception("El Max Allowed no puede ser negativo.");
    }

    private static void Validate(UpdateInlineQualityDto dto)
    {
        if (dto.InspectionDate == default)
            throw new Exception("La Inspection Date es obligatoria.");

        if (dto.CheckedQty <= 0)
            throw new Exception("El Checked Qty debe ser mayor a 0.");

        if (dto.CriticalDefects < 0 || dto.MajorDefects < 0 || dto.MinorDefects < 0)
            throw new Exception("Los defectos no pueden ser negativos.");

        if (dto.MaxAllowed < 0)
            throw new Exception("El Max Allowed no puede ser negativo.");
    }

    private static InlineQualityDto ToDto(InlineQuality item)
    {
        return new InlineQualityDto
        {
            ID = item.ID,
            InspectionDate = item.InspectionDate,
            Time = item.Time,
            Line = item.Line,
            FGPOId = item.FGPOId,
            FGPONumber = item.FGPO?.FGPONumber ?? string.Empty,
            CustomerName = item.FGPO?.Customer?.Name ?? string.Empty,
            Style = item.FGPO?.Style,
            Color = item.FGPO?.Color,
            Operation = item.Operation,
            Operator = item.Operator,
            CheckedQty = item.CheckedQty,
            CriticalDefects = item.CriticalDefects,
            MajorDefects = item.MajorDefects,
            MinorDefects = item.MinorDefects,
            TotalDefects = item.TotalDefects,
            DhuPct = item.DhuPct,
            DefectivePieces = item.DefectivePieces,
            DefectiveRatePct = item.DefectiveRatePct,
            MaxAllowed = item.MaxAllowed,
            Result = item.Result,
            Inspector = item.Inspector,
            ImmediateCorrection = item.ImmediateCorrection,
            RootCause = item.RootCause,
            Active = item.Active,
            CreatedAt = item.CreatedAt,
            UpdatedAt = item.UpdatedAt,
        };
    }
}
