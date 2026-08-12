using Microsoft.EntityFrameworkCore;
using ERP.Api.Data;
using ERP.Api.Dtos;
using ERP.Api.Domain;
using ERP.Api.Interfaces;

namespace ERP.Api.Services;

public class CuttingControlService : ICuttingControlService
{
    private readonly ErpDbContext _context;

    public CuttingControlService(ErpDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<CuttingControlDto>> GetAllAsync()
    {
        var items = await _context.CuttingControls
            .Include(c => c.FGPO)
                .ThenInclude(f => f!.Customer)
            .Include(c => c.Size)
            .Include(c => c.ResponsiblePerson)
            .Where(c => c.Active)
            .OrderByDescending(c => c.CutDate)
            .ToListAsync();

        return items.Select(ToDto);
    }

    public async Task<CuttingControlDto?> GetByIdAsync(int id)
    {
        var item = await _context.CuttingControls
            .Include(c => c.FGPO)
                .ThenInclude(f => f!.Customer)
            .Include(c => c.Size)
            .Include(c => c.ResponsiblePerson)
            .FirstOrDefaultAsync(c => c.ID == id && c.Active);

        return item is null ? null : ToDto(item);
    }

    public async Task<IEnumerable<CuttingControlDto>> GetByFgpoAsync(int fgpoId)
    {
        var items = await _context.CuttingControls
            .Include(c => c.FGPO)
                .ThenInclude(f => f!.Customer)
            .Include(c => c.Size)
            .Include(c => c.ResponsiblePerson)
            .Where(c => c.Active && c.FGPOId == fgpoId)
            .OrderByDescending(c => c.CutDate)
            .ToListAsync();

        return items.Select(ToDto);
    }

    public async Task<CuttingControlDto> CreateAsync(CreateCuttingControlDto dto)
    {
        Validate(dto);

        var fgpo = await _context.Fgpos
            .FirstOrDefaultAsync(f => f.ID == dto.FGPOId && f.Active)
            ?? throw new Exception("El FGPO seleccionado no es válido.");

        var entity = new CuttingControl
        {
            CutDate = dto.CutDate,
            FGPOId = dto.FGPOId,
            SizeId = dto.SizeId,
            FabricLot = dto.FabricLot,
            MarkerNumber = dto.MarkerNumber,
            PlannedCut = dto.PlannedCut,
            ActualCut = dto.ActualCut,
            GoodCut = dto.GoodCut,
            DamagedQty = dto.DamagedQty,
            ReplacementCut = dto.ReplacementCut,
            SentToSewing = dto.SentToSewing,
            ReleaseStatus = dto.ReleaseStatus,
            ResponsiblePersonId = dto.ResponsiblePersonId,
            Comments = dto.Comments,
            Active = true,
            CreatedAt = DateTime.UtcNow,
        };

        _context.CuttingControls.Add(entity);
        await _context.SaveChangesAsync();

        await _context.Entry(entity).Reference(c => c.FGPO).Query()
            .Include(f => f!.Customer).LoadAsync();
        await _context.Entry(entity).Reference(c => c.Size).LoadAsync();
        await _context.Entry(entity).Reference(c => c.ResponsiblePerson).LoadAsync();

        return ToDto(entity);
    }

    public async Task<bool> UpdateAsync(int id, UpdateCuttingControlDto dto)
    {
        var entity = await _context.CuttingControls
            .FirstOrDefaultAsync(c => c.ID == id && c.Active);
        if (entity is null)
            return false;

        Validate(dto);

        var fgpo = await _context.Fgpos
            .FirstOrDefaultAsync(f => f.ID == dto.FGPOId && f.Active)
            ?? throw new Exception("El FGPO seleccionado no es válido.");

        entity.CutDate = dto.CutDate;
        entity.FGPOId = dto.FGPOId;
        entity.SizeId = dto.SizeId;
        entity.FabricLot = dto.FabricLot;
        entity.MarkerNumber = dto.MarkerNumber;
        entity.PlannedCut = dto.PlannedCut;
        entity.ActualCut = dto.ActualCut;
        entity.GoodCut = dto.GoodCut;
        entity.DamagedQty = dto.DamagedQty;
        entity.ReplacementCut = dto.ReplacementCut;
        entity.SentToSewing = dto.SentToSewing;
        entity.ReleaseStatus = dto.ReleaseStatus;
        entity.ResponsiblePersonId = dto.ResponsiblePersonId;
        entity.Comments = dto.Comments;
        entity.UpdatedAt = DateTime.UtcNow;

        _context.CuttingControls.Update(entity);
        await _context.SaveChangesAsync();

        return true;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var entity = await _context.CuttingControls
            .FirstOrDefaultAsync(c => c.ID == id && c.Active);
        if (entity is null)
            return false;

        entity.Active = false;
        entity.UpdatedAt = DateTime.UtcNow;
        _context.CuttingControls.Update(entity);
        await _context.SaveChangesAsync();

        return true;
    }

    public async Task<PagedResultDto<CuttingControlDto>> GetPagedAsync(int page, int pageSize, string? search, string? sortBy, string? sortOrder, string? fgpo, string? status)
    {
        if (page < 1) page = 1;
        if (pageSize < 1) pageSize = 10;
        if (pageSize > 100) pageSize = 100;

        var query = _context.CuttingControls
            .Include(c => c.FGPO)
                .ThenInclude(f => f!.Customer)
            .Include(c => c.Size)
            .Include(c => c.ResponsiblePerson)
            .Where(c => c.Active);

        if (!string.IsNullOrWhiteSpace(search))
        {
            var term = search.Trim();
            query = query.Where(c =>
                (c.FabricLot != null && c.FabricLot.Contains(term)) ||
                (c.MarkerNumber != null && c.MarkerNumber.Contains(term)) ||
                (c.Size != null && c.Size.SizeCode.Contains(term)) ||
                (c.ReleaseStatus != null && c.ReleaseStatus.Contains(term)) ||
                (c.ResponsiblePerson != null && c.ResponsiblePerson.UserName.Contains(term)) ||
                (c.FGPO != null && c.FGPO.FGPONumber.Contains(term)));
        }

        if (!string.IsNullOrWhiteSpace(fgpo))
            query = query.Where(c => c.FGPO != null && c.FGPO.FGPONumber.Contains(fgpo.Trim()));

        if (!string.IsNullOrWhiteSpace(status))
            query = query.Where(c => c.ReleaseStatus != null && c.ReleaseStatus.Contains(status.Trim()));

        var totalCount = await query.CountAsync();
        var totalPages = (int)Math.Ceiling(totalCount / (double)pageSize);

        var sortByLower = sortBy?.ToLowerInvariant();
        var descending = sortOrder?.ToLowerInvariant() == "desc";

        IQueryable<CuttingControl> orderedQuery = (sortByLower, descending) switch
        {
            ("cutdate", false) => query.OrderBy(c => c.CutDate),
            ("cutdate", true) => query.OrderByDescending(c => c.CutDate),
            ("goodcut", false) => query.OrderBy(c => c.GoodCut),
            ("goodcut", true) => query.OrderByDescending(c => c.GoodCut),
            ("releasestatus", false) => query.OrderBy(c => c.ReleaseStatus),
            ("releasestatus", true) => query.OrderByDescending(c => c.ReleaseStatus),
            ("createdat", false) => query.OrderBy(c => c.CreatedAt),
            ("createdat", true) => query.OrderByDescending(c => c.CreatedAt),
            _ => query.OrderByDescending(c => c.CutDate),
        };

        var items = await orderedQuery
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return new PagedResultDto<CuttingControlDto>
        {
            Items = items.Select(ToDto),
            TotalCount = totalCount,
            Page = page,
            PageSize = pageSize,
            TotalPages = totalPages,
        };
    }

    private static void Validate(CreateCuttingControlDto dto)
    {
        if (dto.CutDate == default)
            throw new Exception("La Date es obligatoria.");

        if (dto.PlannedCut < 0 || dto.ActualCut < 0 || dto.GoodCut < 0 || dto.DamagedQty < 0 || dto.ReplacementCut < 0 || dto.SentToSewing < 0)
            throw new Exception("Las cantidades no pueden ser negativas.");
    }

    private static void Validate(UpdateCuttingControlDto dto)
    {
        if (dto.CutDate == default)
            throw new Exception("La Date es obligatoria.");

        if (dto.PlannedCut < 0 || dto.ActualCut < 0 || dto.GoodCut < 0 || dto.DamagedQty < 0 || dto.ReplacementCut < 0 || dto.SentToSewing < 0)
            throw new Exception("Las cantidades no pueden ser negativas.");
    }

    private static CuttingControlDto ToDto(CuttingControl item)
    {
        return new CuttingControlDto
        {
            ID = item.ID,
            CutDate = item.CutDate,
            FGPOId = item.FGPOId,
            FGPONumber = item.FGPO?.FGPONumber ?? string.Empty,
            CustomerName = item.FGPO?.Customer?.Name ?? string.Empty,
            SizeId = item.SizeId,
            SizeName = item.Size?.SizeCode,
            FabricLot = item.FabricLot,
            MarkerNumber = item.MarkerNumber,
            PlannedCut = item.PlannedCut,
            ActualCut = item.ActualCut,
            GoodCut = item.GoodCut,
            DamagedQty = item.DamagedQty,
            ReplacementCut = item.ReplacementCut,
            SentToSewing = item.SentToSewing,
            CuttingVariance = item.CuttingVariance,
            PendingCut = item.PendingCut,
            OvercutQty = item.OvercutQty,
            CutToSewDifference = item.CutToSewDifference,
            ReleaseStatus = item.ReleaseStatus,
            ResponsiblePersonId = item.ResponsiblePersonId,
            ResponsiblePersonName = item.ResponsiblePerson?.UserName,
            Comments = item.Comments,
            Active = item.Active,
            CreatedAt = item.CreatedAt,
            UpdatedAt = item.UpdatedAt,
        };
    }
}
