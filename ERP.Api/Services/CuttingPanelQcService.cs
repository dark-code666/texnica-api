using Microsoft.EntityFrameworkCore;
using ERP.Api.Data;
using ERP.Api.Dtos;
using ERP.Api.Domain;
using ERP.Api.Interfaces;

namespace ERP.Api.Services;

public class CuttingPanelQcService : ICuttingPanelQcService
{
    private readonly ErpDbContext _context;

    public CuttingPanelQcService(ErpDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<CuttingPanelQcDto>> GetAllAsync()
    {
        var items = await _context.CuttingPanelQcs
            .Include(c => c.FGPO)
                .ThenInclude(f => f!.Customer)
            .Include(c => c.Size)
            .Include(c => c.Inspector)
            .Where(c => c.Active)
            .OrderByDescending(c => c.InspectionDate)
            .ToListAsync();

        return items.Select(ToDto);
    }

    public async Task<CuttingPanelQcDto?> GetByIdAsync(int id)
    {
        var item = await _context.CuttingPanelQcs
            .Include(c => c.FGPO)
                .ThenInclude(f => f!.Customer)
            .Include(c => c.Size)
            .Include(c => c.Inspector)
            .FirstOrDefaultAsync(c => c.ID == id && c.Active);

        return item is null ? null : ToDto(item);
    }

    public async Task<IEnumerable<CuttingPanelQcDto>> GetByFgpoAsync(int fgpoId)
    {
        var items = await _context.CuttingPanelQcs
            .Include(c => c.FGPO)
                .ThenInclude(f => f!.Customer)
            .Include(c => c.Size)
            .Include(c => c.Inspector)
            .Where(c => c.Active && c.FGPOId == fgpoId)
            .OrderByDescending(c => c.InspectionDate)
            .ToListAsync();

        return items.Select(ToDto);
    }

    public async Task<CuttingPanelQcDto> CreateAsync(CreateCuttingPanelQcDto dto)
    {
        Validate(dto);

        var fgpo = await _context.Fgpos
            .FirstOrDefaultAsync(f => f.ID == dto.FGPOId && f.Active)
            ?? throw new Exception("El FGPO seleccionado no es válido.");

        var entity = new CuttingPanelQc
        {
            InspectionDate = dto.InspectionDate,
            FGPOId = dto.FGPOId,
            SizeId = dto.SizeId,
            FabricLot = dto.FabricLot,
            CutLotLay = dto.CutLotLay,
            BundleNo = dto.BundleNo,
            SampleQty = dto.SampleQty,
            PanelDefects = dto.PanelDefects,
            NotchesDefects = dto.NotchesDefects,
            DrillMarkDefects = dto.DrillMarkDefects,
            ShadeDefects = dto.ShadeDefects,
            MeasurementDefects = dto.MeasurementDefects,
            MaxAllowed = 0.02m,
            InspectorId = dto.InspectorId,
            CorrectiveAction = dto.CorrectiveAction,
            Comments = dto.Comments,
            Active = true,
            CreatedAt = DateTime.UtcNow,
        };

        _context.CuttingPanelQcs.Add(entity);
        await _context.SaveChangesAsync();

        await _context.Entry(entity).Reference(c => c.FGPO).Query()
            .Include(f => f!.Customer).LoadAsync();
        await _context.Entry(entity).Reference(c => c.Size).LoadAsync();
        await _context.Entry(entity).Reference(c => c.Inspector).LoadAsync();

        return ToDto(entity);
    }

    public async Task<bool> UpdateAsync(int id, UpdateCuttingPanelQcDto dto)
    {
        var entity = await _context.CuttingPanelQcs
            .FirstOrDefaultAsync(c => c.ID == id && c.Active);
        if (entity is null)
            return false;

        Validate(dto);

        var fgpo = await _context.Fgpos
            .FirstOrDefaultAsync(f => f.ID == dto.FGPOId && f.Active)
            ?? throw new Exception("El FGPO seleccionado no es válido.");

        entity.InspectionDate = dto.InspectionDate;
        entity.FGPOId = dto.FGPOId;
        entity.SizeId = dto.SizeId;
        entity.FabricLot = dto.FabricLot;
        entity.CutLotLay = dto.CutLotLay;
        entity.BundleNo = dto.BundleNo;
        entity.SampleQty = dto.SampleQty;
        entity.PanelDefects = dto.PanelDefects;
        entity.NotchesDefects = dto.NotchesDefects;
        entity.DrillMarkDefects = dto.DrillMarkDefects;
        entity.ShadeDefects = dto.ShadeDefects;
        entity.MeasurementDefects = dto.MeasurementDefects;
        entity.InspectorId = dto.InspectorId;
        entity.CorrectiveAction = dto.CorrectiveAction;
        entity.Comments = dto.Comments;
        entity.UpdatedAt = DateTime.UtcNow;

        _context.CuttingPanelQcs.Update(entity);
        await _context.SaveChangesAsync();

        return true;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var entity = await _context.CuttingPanelQcs
            .FirstOrDefaultAsync(c => c.ID == id && c.Active);
        if (entity is null)
            return false;

        entity.Active = false;
        entity.UpdatedAt = DateTime.UtcNow;
        _context.CuttingPanelQcs.Update(entity);
        await _context.SaveChangesAsync();

        return true;
    }

    public async Task<PagedResultDto<CuttingPanelQcDto>> GetPagedAsync(int page, int pageSize, string? search, string? sortBy, string? sortOrder, string? fgpo, string? result)
    {
        if (page < 1) page = 1;
        if (pageSize < 1) pageSize = 10;
        if (pageSize > 100) pageSize = 100;

        var query = _context.CuttingPanelQcs
            .Include(c => c.FGPO)
                .ThenInclude(f => f!.Customer)
            .Include(c => c.Size)
            .Include(c => c.Inspector)
            .Where(c => c.Active);

        if (!string.IsNullOrWhiteSpace(search))
        {
            var term = search.Trim();
            query = query.Where(c =>
                (c.FabricLot != null && c.FabricLot.Contains(term)) ||
                (c.CutLotLay != null && c.CutLotLay.Contains(term)) ||
                (c.BundleNo != null && c.BundleNo.Contains(term)) ||
                (c.Result != null && c.Result.Contains(term)) ||
                (c.FGPO != null && c.FGPO.FGPONumber.Contains(term)));
        }

        if (!string.IsNullOrWhiteSpace(fgpo))
            query = query.Where(c => c.FGPO != null && c.FGPO.FGPONumber.Contains(fgpo.Trim()));

        if (!string.IsNullOrWhiteSpace(result))
            query = query.Where(c => c.Result != null && c.Result.Contains(result.Trim()));

        var totalCount = await query.CountAsync();
        var totalPages = (int)Math.Ceiling(totalCount / (double)pageSize);

        var sortByLower = sortBy?.ToLowerInvariant();
        var descending = sortOrder?.ToLowerInvariant() == "desc";

        IQueryable<CuttingPanelQc> orderedQuery = (sortByLower, descending) switch
        {
            ("inspectiondate", false) => query.OrderBy(c => c.InspectionDate),
            ("inspectiondate", true) => query.OrderByDescending(c => c.InspectionDate),
            ("totaldefects", false) => query.OrderBy(c => c.TotalDefects),
            ("totaldefects", true) => query.OrderByDescending(c => c.TotalDefects),
            ("result", false) => query.OrderBy(c => c.Result),
            ("result", true) => query.OrderByDescending(c => c.Result),
            ("createdat", false) => query.OrderBy(c => c.CreatedAt),
            ("createdat", true) => query.OrderByDescending(c => c.CreatedAt),
            _ => query.OrderByDescending(c => c.InspectionDate),
        };

        var items = await orderedQuery
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return new PagedResultDto<CuttingPanelQcDto>
        {
            Items = items.Select(ToDto),
            TotalCount = totalCount,
            Page = page,
            PageSize = pageSize,
            TotalPages = totalPages,
        };
    }

    private static void Validate(CreateCuttingPanelQcDto dto)
    {
        if (dto.InspectionDate == default)
            throw new Exception("La Date es obligatoria.");

        if (dto.SampleQty < 0)
            throw new Exception("El Sample Qty no puede ser negativo.");

        if (dto.PanelDefects < 0 || dto.NotchesDefects < 0 || dto.DrillMarkDefects < 0 || dto.ShadeDefects < 0 || dto.MeasurementDefects < 0)
            throw new Exception("Los defectos no pueden ser negativos.");
    }

    private static void Validate(UpdateCuttingPanelQcDto dto)
    {
        if (dto.InspectionDate == default)
            throw new Exception("La Date es obligatoria.");

        if (dto.SampleQty < 0)
            throw new Exception("El Sample Qty no puede ser negativo.");

        if (dto.PanelDefects < 0 || dto.NotchesDefects < 0 || dto.DrillMarkDefects < 0 || dto.ShadeDefects < 0 || dto.MeasurementDefects < 0)
            throw new Exception("Los defectos no pueden ser negativos.");
    }

    private static CuttingPanelQcDto ToDto(CuttingPanelQc item)
    {
        return new CuttingPanelQcDto
        {
            ID = item.ID,
            InspectionDate = item.InspectionDate,
            FGPOId = item.FGPOId,
            FGPONumber = item.FGPO?.FGPONumber ?? string.Empty,
            CustomerName = item.FGPO?.Customer?.Name ?? string.Empty,
            SizeId = item.SizeId,
            SizeName = item.Size?.SizeCode,
            FabricLot = item.FabricLot,
            CutLotLay = item.CutLotLay,
            BundleNo = item.BundleNo,
            SampleQty = item.SampleQty,
            PanelDefects = item.PanelDefects,
            NotchesDefects = item.NotchesDefects,
            DrillMarkDefects = item.DrillMarkDefects,
            ShadeDefects = item.ShadeDefects,
            MeasurementDefects = item.MeasurementDefects,
            TotalDefects = item.TotalDefects,
            DefectRatePct = item.DefectRatePct,
            MaxAllowed = item.MaxAllowed,
            Result = item.Result,
            InspectorId = item.InspectorId,
            InspectorName = item.Inspector?.UserName,
            CorrectiveAction = item.CorrectiveAction,
            Comments = item.Comments,
            Active = item.Active,
            CreatedAt = item.CreatedAt,
            UpdatedAt = item.UpdatedAt,
        };
    }
}
