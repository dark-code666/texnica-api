using Microsoft.EntityFrameworkCore;
using ERP.Api.Data;
using ERP.Api.Dtos;
using ERP.Api.Domain;
using ERP.Api.Interfaces;

namespace ERP.Api.Services;

public class PackingControlService : IPackingControlService
{
    private readonly ErpDbContext _context;

    public PackingControlService(ErpDbContext context)
    {
        _context = context;
    }

    private IQueryable<PackingControl> Query()
        => _context.PackingControls
            .Include(p => p.FGPO).ThenInclude(f => f!.Customer)
            .Include(p => p.FGPO).ThenInclude(f => f!.FgpoLines).ThenInclude(l => l.Style)
            .Include(p => p.FGPO).ThenInclude(f => f!.FgpoLines).ThenInclude(l => l.Color)
            .Include(p => p.FGPO).ThenInclude(f => f!.FgpoLines).ThenInclude(l => l.Size)
            .Include(p => p.ResponsiblePerson);

    public async Task<IEnumerable<PackingControlDto>> GetAllAsync()
    {
        var items = await Query()
            .Where(p => p.Active)
            .OrderByDescending(p => p.PackingDate)
            .ToListAsync();
        return items.Select(ToDto);
    }

    public async Task<PackingControlDto?> GetByIdAsync(int id)
    {
        var item = await Query().FirstOrDefaultAsync(p => p.ID == id && p.Active);
        return item is null ? null : ToDto(item);
    }

    public async Task<IEnumerable<PackingControlDto>> GetByFgpoAsync(int fgpoId)
    {
        var items = await Query()
            .Where(p => p.Active && p.FGPOId == fgpoId)
            .OrderByDescending(p => p.PackingDate)
            .ToListAsync();
        return items.Select(ToDto);
    }

    public async Task<PackingControlDto> CreateAsync(CreatePackingControlDto dto)
    {
        Validate(dto);
        var fgpo = await _context.Fgpos
            .Include(f => f.FgpoLines).ThenInclude(l => l.Style)
            .Include(f => f.FgpoLines).ThenInclude(l => l.Color)
            .Include(f => f.FgpoLines).ThenInclude(l => l.Size)
            .FirstOrDefaultAsync(f => f.ID == dto.FGPOId && f.Active)
            ?? throw new Exception("El FGPO seleccionado no es válido.");

        var entity = new PackingControl
        {
            PackingDate = dto.PackingDate,
            FGPOId = dto.FGPOId,
            QcPassedQty = dto.QcPassedQty,
            ReceivedByPackingQty = dto.ReceivedByPackingQty,
            FoldedQty = dto.FoldedQty,
            PolybaggedQty = dto.PolybaggedQty,
            PackedQty = dto.PackedQty,
            FullCartons = dto.FullCartons,
            PartialCartons = dto.PartialCartons,
            PcsPerCarton = dto.PcsPerCarton,
            ResponsiblePersonId = dto.ResponsiblePersonId,
            LastUpdated = dto.LastUpdated ?? DateTime.UtcNow,
            Remarks = dto.Remarks,
            Active = true,
            CreatedAt = DateTime.UtcNow,
        };

        _context.PackingControls.Add(entity);
        await _context.SaveChangesAsync();
        await _context.Entry(entity).Reference(p => p.FGPO).Query()
            .Include(f => f!.Customer)
            .Include(f => f!.FgpoLines).ThenInclude(l => l.Style)
            .Include(f => f!.FgpoLines).ThenInclude(l => l.Color)
            .Include(f => f!.FgpoLines).ThenInclude(l => l.Size)
            .LoadAsync();
        await _context.Entry(entity).Reference(p => p.ResponsiblePerson).LoadAsync();
        return ToDto(entity);
    }

    public async Task<bool> UpdateAsync(int id, UpdatePackingControlDto dto)
    {
        var entity = await _context.PackingControls.FirstOrDefaultAsync(p => p.ID == id && p.Active);
        if (entity is null)
            return false;

        Validate(dto);
        var fgpo = await _context.Fgpos.FirstOrDefaultAsync(f => f.ID == dto.FGPOId && f.Active)
            ?? throw new Exception("El FGPO seleccionado no es válido.");

        entity.PackingDate = dto.PackingDate;
        entity.FGPOId = dto.FGPOId;
        entity.QcPassedQty = dto.QcPassedQty;
        entity.ReceivedByPackingQty = dto.ReceivedByPackingQty;
        entity.FoldedQty = dto.FoldedQty;
        entity.PolybaggedQty = dto.PolybaggedQty;
        entity.PackedQty = dto.PackedQty;
        entity.FullCartons = dto.FullCartons;
        entity.PartialCartons = dto.PartialCartons;
        entity.PcsPerCarton = dto.PcsPerCarton;
        entity.ResponsiblePersonId = dto.ResponsiblePersonId;
        entity.LastUpdated = dto.LastUpdated ?? DateTime.UtcNow;
        entity.Remarks = dto.Remarks;
        entity.UpdatedAt = DateTime.UtcNow;

        _context.PackingControls.Update(entity);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var entity = await _context.PackingControls.FirstOrDefaultAsync(p => p.ID == id && p.Active);
        if (entity is null)
            return false;

        entity.Active = false;
        entity.UpdatedAt = DateTime.UtcNow;
        _context.PackingControls.Update(entity);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<PagedResultDto<PackingControlDto>> GetPagedAsync(int page, int pageSize, string? search, string? sortBy, string? sortOrder, string? fgpo)
    {
        if (page < 1) page = 1;
        if (pageSize < 1) pageSize = 10;
        if (pageSize > 100) pageSize = 100;

        var query = Query().Where(p => p.Active);

        if (!string.IsNullOrWhiteSpace(search))
        {
            var term = search.Trim();
            query = query.Where(p =>
                (p.FGPO != null && p.FGPO.FGPONumber.Contains(term)) ||
                (p.FGPO != null && p.FGPO.Customer != null && p.FGPO.Customer.Name.Contains(term)) ||
                (p.FGPO != null && p.FGPO.FgpoLines.Any(l => l.Active && l.Style != null && l.Style.StyleCode.Contains(term))) ||
                (p.FGPO != null && p.FGPO.FgpoLines.Any(l => l.Active && l.Color != null && l.Color.ColorName.Contains(term))) ||
                (p.Remarks != null && p.Remarks.Contains(term)));
        }

        if (!string.IsNullOrWhiteSpace(fgpo))
            query = query.Where(p => p.FGPO != null && p.FGPO.FGPONumber.Contains(fgpo.Trim()));

        var totalCount = await query.CountAsync();
        var totalPages = (int)Math.Ceiling(totalCount / (double)pageSize);

        var sortByLower = sortBy?.ToLowerInvariant();
        var descending = sortOrder?.ToLowerInvariant() == "desc";

        IQueryable<PackingControl> orderedQuery = (sortByLower, descending) switch
        {
            ("packingdate", false) => query.OrderBy(p => p.PackingDate),
            ("packingdate", true) => query.OrderByDescending(p => p.PackingDate),
            ("packedqty", false) => query.OrderBy(p => p.PackedQty),
            ("packedqty", true) => query.OrderByDescending(p => p.PackedQty),
            ("readytoshipqty", false) => query.OrderBy(p => p.ReadyToShipQty),
            ("readytoshipqty", true) => query.OrderByDescending(p => p.ReadyToShipQty),
            ("packingvariance", false) => query.OrderBy(p => p.PackingVariance),
            ("packingvariance", true) => query.OrderByDescending(p => p.PackingVariance),
            ("createdat", false) => query.OrderBy(p => p.CreatedAt),
            ("createdat", true) => query.OrderByDescending(p => p.CreatedAt),
            _ => query.OrderByDescending(p => p.PackingDate),
        };

        var items = await orderedQuery
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return new PagedResultDto<PackingControlDto>
        {
            Items = items.Select(ToDto),
            TotalCount = totalCount,
            Page = page,
            PageSize = pageSize,
            TotalPages = totalPages,
        };
    }

    private static void Validate(CreatePackingControlDto dto)
    {
        if (dto.PackingDate == default)
            throw new Exception("La Packing Date es obligatoria.");
        if (dto.QcPassedQty < 0 || dto.ReceivedByPackingQty < 0 || dto.FoldedQty < 0
            || dto.PolybaggedQty < 0 || dto.PackedQty < 0)
            throw new Exception("Las cantidades no pueden ser negativas.");
        if (dto.FullCartons < 0 || dto.PartialCartons < 0 || dto.PcsPerCarton < 0)
            throw new Exception("El conteo de cartones no puede ser negativo.");
    }

    private static void Validate(UpdatePackingControlDto dto)
    {
        if (dto.PackingDate == default)
            throw new Exception("La Packing Date es obligatoria.");
        if (dto.QcPassedQty < 0 || dto.ReceivedByPackingQty < 0 || dto.FoldedQty < 0
            || dto.PolybaggedQty < 0 || dto.PackedQty < 0)
            throw new Exception("Las cantidades no pueden ser negativas.");
        if (dto.FullCartons < 0 || dto.PartialCartons < 0 || dto.PcsPerCarton < 0)
            throw new Exception("El conteo de cartones no puede ser negativo.");
    }

    private static PackingControlDto ToDto(PackingControl item)
    {
        var primaryLine = item.FGPO?.FgpoLines?.FirstOrDefault(l => l.Active);
        return new PackingControlDto
        {
            ID = item.ID,
            PackingDate = item.PackingDate,
            FGPOId = item.FGPOId,
            FGPONumber = item.FGPO?.FGPONumber ?? string.Empty,
            CustomerName = item.FGPO?.Customer?.Name ?? string.Empty,
            Style = primaryLine?.Style?.StyleCode,
            Color = primaryLine?.Color?.ColorName,
            Size = primaryLine?.Size?.SizeCode,
            QcPassedQty = item.QcPassedQty,
            ReceivedByPackingQty = item.ReceivedByPackingQty,
            FoldedQty = item.FoldedQty,
            PolybaggedQty = item.PolybaggedQty,
            PackedQty = item.PackedQty,
            FullCartons = item.FullCartons,
            PartialCartons = item.PartialCartons,
            PcsPerCarton = item.PcsPerCarton,
            ReadyToShipQty = item.ReadyToShipQty,
            PackingVariance = item.PackingVariance,
            PendingPacking = item.PendingPacking,
            OverpackedQty = item.OverpackedQty,
            ResponsiblePersonId = item.ResponsiblePersonId,
            ResponsiblePersonName = item.ResponsiblePerson?.UserName,
            LastUpdated = item.LastUpdated,
            Remarks = item.Remarks,
            Active = item.Active,
            CreatedAt = item.CreatedAt,
            UpdatedAt = item.UpdatedAt,
        };
    }
}
