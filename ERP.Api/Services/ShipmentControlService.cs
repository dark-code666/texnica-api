using Microsoft.EntityFrameworkCore;
using ERP.Api.Data;
using ERP.Api.Dtos;
using ERP.Api.Domain;
using ERP.Api.Interfaces;

namespace ERP.Api.Services;

public class ShipmentControlService : IShipmentControlService
{
    private readonly ErpDbContext _context;

    public ShipmentControlService(ErpDbContext context)
    {
        _context = context;
    }

    private IQueryable<ShipmentControl> Query()
        => _context.ShipmentControls
            .Include(s => s.FGPO).ThenInclude(f => f!.Customer)
            .Include(s => s.FGPO).ThenInclude(f => f!.FgpoLines).ThenInclude(l => l.Style)
            .Include(s => s.FGPO).ThenInclude(f => f!.FgpoLines).ThenInclude(l => l.Color)
            .Include(s => s.FGPO).ThenInclude(f => f!.FgpoLines).ThenInclude(l => l.Size)
            .Include(s => s.DataOwner);

    public async Task<IEnumerable<ShipmentControlDto>> GetAllAsync()
    {
        var items = await Query()
            .Where(s => s.Active)
            .OrderByDescending(s => s.ETD)
            .ToListAsync();
        return items.Select(ToDto);
    }

    public async Task<ShipmentControlDto?> GetByIdAsync(int id)
    {
        var item = await Query().FirstOrDefaultAsync(s => s.ID == id && s.Active);
        return item is null ? null : ToDto(item);
    }

    public async Task<IEnumerable<ShipmentControlDto>> GetByFgpoAsync(int fgpoId)
    {
        var items = await Query()
            .Where(s => s.Active && s.FGPOId == fgpoId)
            .OrderByDescending(s => s.ETD)
            .ToListAsync();
        return items.Select(ToDto);
    }

    public async Task<ShipmentControlDto> CreateAsync(CreateShipmentControlDto dto)
    {
        Validate(dto);
        var fgpo = await _context.Fgpos
            .Include(f => f.FgpoLines).ThenInclude(l => l.Style)
            .Include(f => f.FgpoLines).ThenInclude(l => l.Color)
            .Include(f => f.FgpoLines).ThenInclude(l => l.Size)
            .FirstOrDefaultAsync(f => f.ID == dto.FGPOId && f.Active)
            ?? throw new Exception("El FGPO seleccionado no es válido.");

        var entity = new ShipmentControl
        {
            ShipmentNumber = dto.ShipmentNumber,
            PlannedLoadingDate = dto.PlannedLoadingDate,
            ActualLoadingDate = dto.ActualLoadingDate,
            ETD = dto.ETD,
            ETA = dto.ETA,
            FGPOId = dto.FGPOId,
            PlannedQty = dto.PlannedQty,
            ActualLoadedQty = dto.ActualLoadedQty,
            InTransitQty = dto.InTransitQty,
            CustomerReceivedQty = dto.CustomerReceivedQty,
            TotalShippedQty = dto.TotalShippedQty,
            ContainerType = dto.ContainerType,
            ContainerNumber = dto.ContainerNumber,
            BookingNumber = dto.BookingNumber,
            Destination = dto.Destination,
            ShipmentStatus = dto.ShipmentStatus,
            PackingList = dto.PackingList,
            InvoiceNumber = dto.InvoiceNumber,
            LoadPlan = dto.LoadPlan,
            DataOwnerId = dto.DataOwnerId,
            LastUpdated = dto.LastUpdated ?? DateTime.UtcNow,
            Remarks = dto.Remarks,
            Active = true,
            CreatedAt = DateTime.UtcNow,
        };

        _context.ShipmentControls.Add(entity);
        await _context.SaveChangesAsync();
        await _context.Entry(entity).Reference(s => s.FGPO).Query()
            .Include(f => f!.Customer)
            .Include(f => f!.FgpoLines).ThenInclude(l => l.Style)
            .Include(f => f!.FgpoLines).ThenInclude(l => l.Color)
            .Include(f => f!.FgpoLines).ThenInclude(l => l.Size)
            .LoadAsync();
        await _context.Entry(entity).Reference(s => s.DataOwner).LoadAsync();
        return ToDto(entity);
    }

    public async Task<bool> UpdateAsync(int id, UpdateShipmentControlDto dto)
    {
        var entity = await _context.ShipmentControls.FirstOrDefaultAsync(s => s.ID == id && s.Active);
        if (entity is null)
            return false;

        Validate(dto);
        var fgpo = await _context.Fgpos.FirstOrDefaultAsync(f => f.ID == dto.FGPOId && f.Active)
            ?? throw new Exception("El FGPO seleccionado no es válido.");

        entity.ShipmentNumber = dto.ShipmentNumber;
        entity.PlannedLoadingDate = dto.PlannedLoadingDate;
        entity.ActualLoadingDate = dto.ActualLoadingDate;
        entity.ETD = dto.ETD;
        entity.ETA = dto.ETA;
        entity.FGPOId = dto.FGPOId;
        entity.PlannedQty = dto.PlannedQty;
        entity.ActualLoadedQty = dto.ActualLoadedQty;
        entity.InTransitQty = dto.InTransitQty;
        entity.CustomerReceivedQty = dto.CustomerReceivedQty;
        entity.TotalShippedQty = dto.TotalShippedQty;
        entity.ContainerType = dto.ContainerType;
        entity.ContainerNumber = dto.ContainerNumber;
        entity.BookingNumber = dto.BookingNumber;
        entity.Destination = dto.Destination;
        entity.ShipmentStatus = dto.ShipmentStatus;
        entity.PackingList = dto.PackingList;
        entity.InvoiceNumber = dto.InvoiceNumber;
        entity.LoadPlan = dto.LoadPlan;
        entity.DataOwnerId = dto.DataOwnerId;
        entity.LastUpdated = dto.LastUpdated ?? DateTime.UtcNow;
        entity.Remarks = dto.Remarks;
        entity.UpdatedAt = DateTime.UtcNow;

        _context.ShipmentControls.Update(entity);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var entity = await _context.ShipmentControls.FirstOrDefaultAsync(s => s.ID == id && s.Active);
        if (entity is null)
            return false;

        entity.Active = false;
        entity.UpdatedAt = DateTime.UtcNow;
        _context.ShipmentControls.Update(entity);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<PagedResultDto<ShipmentControlDto>> GetPagedAsync(int page, int pageSize, string? search, string? sortBy, string? sortOrder, string? fgpo, string? status)
    {
        if (page < 1) page = 1;
        if (pageSize < 1) pageSize = 10;
        if (pageSize > 100) pageSize = 100;

        var query = Query().Where(s => s.Active);

        if (!string.IsNullOrWhiteSpace(search))
        {
            var term = search.Trim();
            query = query.Where(s =>
                s.ShipmentNumber.Contains(term) ||
                (s.FGPO != null && s.FGPO.FGPONumber.Contains(term)) ||
                (s.FGPO != null && s.FGPO.Customer != null && s.FGPO.Customer.Name.Contains(term)) ||
                (s.FGPO != null && s.FGPO.FgpoLines.Any(l => l.Active && l.Style != null && l.Style.StyleCode.Contains(term))) ||
                (s.FGPO != null && s.FGPO.FgpoLines.Any(l => l.Active && l.Color != null && l.Color.ColorName.Contains(term))) ||
                (s.ContainerNumber != null && s.ContainerNumber.Contains(term)) ||
                (s.BookingNumber != null && s.BookingNumber.Contains(term)) ||
                (s.Destination != null && s.Destination.Contains(term)) ||
                (s.ShipmentStatus != null && s.ShipmentStatus.Contains(term)) ||
                (s.Remarks != null && s.Remarks.Contains(term)));
        }

        if (!string.IsNullOrWhiteSpace(fgpo))
            query = query.Where(s => s.FGPO != null && s.FGPO.FGPONumber.Contains(fgpo.Trim()));

        if (!string.IsNullOrWhiteSpace(status))
            query = query.Where(s => s.ShipmentStatus != null && s.ShipmentStatus.Contains(status.Trim()));

        var totalCount = await query.CountAsync();
        var totalPages = (int)Math.Ceiling(totalCount / (double)pageSize);

        var sortByLower = sortBy?.ToLowerInvariant();
        var descending = sortOrder?.ToLowerInvariant() == "desc";

        IQueryable<ShipmentControl> orderedQuery = (sortByLower, descending) switch
        {
            ("shipmentnumber", false) => query.OrderBy(s => s.ShipmentNumber),
            ("shipmentnumber", true) => query.OrderByDescending(s => s.ShipmentNumber),
            ("etd", false) => query.OrderBy(s => s.ETD),
            ("etd", true) => query.OrderByDescending(s => s.ETD),
            ("plannedqty", false) => query.OrderBy(s => s.PlannedQty),
            ("plannedqty", true) => query.OrderByDescending(s => s.PlannedQty),
            ("totalshippedqty", false) => query.OrderBy(s => s.TotalShippedQty),
            ("totalshippedqty", true) => query.OrderByDescending(s => s.TotalShippedQty),
            ("shipmentstatus", false) => query.OrderBy(s => s.ShipmentStatus),
            ("shipmentstatus", true) => query.OrderByDescending(s => s.ShipmentStatus),
            ("createdat", false) => query.OrderBy(s => s.CreatedAt),
            ("createdat", true) => query.OrderByDescending(s => s.CreatedAt),
            _ => query.OrderByDescending(s => s.ETD),
        };

        var items = await orderedQuery
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return new PagedResultDto<ShipmentControlDto>
        {
            Items = items.Select(ToDto),
            TotalCount = totalCount,
            Page = page,
            PageSize = pageSize,
            TotalPages = totalPages,
        };
    }

    private static void Validate(CreateShipmentControlDto dto)
    {
        if (string.IsNullOrWhiteSpace(dto.ShipmentNumber))
            throw new Exception("El Shipment Number es obligatorio.");
        if (dto.PlannedQty < 0 || dto.ActualLoadedQty < 0 || dto.InTransitQty < 0
            || dto.CustomerReceivedQty < 0 || dto.TotalShippedQty < 0)
            throw new Exception("Las cantidades no pueden ser negativas.");
    }

    private static void Validate(UpdateShipmentControlDto dto)
    {
        if (string.IsNullOrWhiteSpace(dto.ShipmentNumber))
            throw new Exception("El Shipment Number es obligatorio.");
        if (dto.PlannedQty < 0 || dto.ActualLoadedQty < 0 || dto.InTransitQty < 0
            || dto.CustomerReceivedQty < 0 || dto.TotalShippedQty < 0)
            throw new Exception("Las cantidades no pueden ser negativas.");
    }

    private static ShipmentControlDto ToDto(ShipmentControl item)
    {
        var primaryLine = item.FGPO?.FgpoLines?.FirstOrDefault(l => l.Active);
        return new ShipmentControlDto
        {
            ID = item.ID,
            ShipmentNumber = item.ShipmentNumber,
            PlannedLoadingDate = item.PlannedLoadingDate,
            ActualLoadingDate = item.ActualLoadingDate,
            ETD = item.ETD,
            ETA = item.ETA,
            FGPOId = item.FGPOId,
            FGPONumber = item.FGPO?.FGPONumber ?? string.Empty,
            CustomerName = item.FGPO?.Customer?.Name ?? string.Empty,
            Style = primaryLine?.Style?.StyleCode,
            Color = primaryLine?.Color?.ColorName,
            Size = primaryLine?.Size?.SizeCode,
            PlannedQty = item.PlannedQty,
            ActualLoadedQty = item.ActualLoadedQty,
            InTransitQty = item.InTransitQty,
            CustomerReceivedQty = item.CustomerReceivedQty,
            TotalShippedQty = item.TotalShippedQty,
            ShipmentVariance = item.ShipmentVariance,
            PendingToShip = item.PendingToShip,
            OvershipmentQty = item.OvershipmentQty,
            ContainerType = item.ContainerType,
            ContainerNumber = item.ContainerNumber,
            BookingNumber = item.BookingNumber,
            Destination = item.Destination,
            ShipmentStatus = item.ShipmentStatus,
            PackingList = item.PackingList,
            InvoiceNumber = item.InvoiceNumber,
            LoadPlan = item.LoadPlan,
            DataOwnerId = item.DataOwnerId,
            DataOwnerName = item.DataOwner?.UserName,
            LastUpdated = item.LastUpdated,
            Remarks = item.Remarks,
            Active = item.Active,
            CreatedAt = item.CreatedAt,
            UpdatedAt = item.UpdatedAt,
        };
    }
}
