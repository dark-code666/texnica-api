using Microsoft.EntityFrameworkCore;
using ERP.Api.Data;
using ERP.Api.Dtos;
using ERP.Api.Domain;
using ERP.Api.Interfaces;

namespace ERP.Api.Services;

public class FabricShipmentService : IFabricShipmentService
{
    private readonly ErpDbContext _context;

    private static readonly string[] ValidShipmentStatuses =
    {
        "Planned", "Booking Confirmed", "Exported", "In Transit", "Delivered", "Cancelled"
    };

    private static readonly string[] ValidUoms =
    {
        "Yards", "Meters", "Kilograms", "Pounds", "Rolls", "Pieces"
    };

    public FabricShipmentService(ErpDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<FabricShipmentDto>> GetAllAsync()
    {
        var items = await _context.FabricShipments
            .Include(s => s.FabricPO)
            .Include(s => s.FGPO)
                .ThenInclude(f => f!.Customer)
            .Where(s => s.Active)
            .OrderByDescending(s => s.CreatedAt)
            .ToListAsync();

        return items.Select(ToDto);
    }

    public async Task<FabricShipmentDto?> GetByIdAsync(int id)
    {
        var item = await _context.FabricShipments
            .Include(s => s.FabricPO)
            .Include(s => s.FGPO)
                .ThenInclude(f => f!.Customer)
            .FirstOrDefaultAsync(s => s.ID == id && s.Active);

        return item is null ? null : ToDto(item);
    }

    public async Task<IEnumerable<FabricShipmentDto>> GetByFabricPOAsync(int fabricPOId)
    {
        var items = await _context.FabricShipments
            .Include(s => s.FabricPO)
            .Include(s => s.FGPO)
                .ThenInclude(f => f!.Customer)
            .Where(s => s.Active && s.FabricPOId == fabricPOId)
            .OrderByDescending(s => s.CreatedAt)
            .ToListAsync();

        return items.Select(ToDto);
    }

    public async Task<IEnumerable<FabricShipmentDto>> GetByFgpoAsync(int fgpoId)
    {
        var items = await _context.FabricShipments
            .Include(s => s.FabricPO)
            .Include(s => s.FGPO)
                .ThenInclude(f => f!.Customer)
            .Where(s => s.Active && s.FGPOId == fgpoId)
            .OrderByDescending(s => s.CreatedAt)
            .ToListAsync();

        return items.Select(ToDto);
    }

    public async Task<IEnumerable<FabricShipmentDto>> GetByLotAsync(string lotNumber)
    {
        var items = await _context.FabricShipments
            .Include(s => s.FabricPO)
            .Include(s => s.FGPO)
                .ThenInclude(f => f!.Customer)
            .Where(s => s.Active && s.LotNumber != null && s.LotNumber.Contains(lotNumber))
            .OrderByDescending(s => s.CreatedAt)
            .ToListAsync();

        return items.Select(ToDto);
    }

    public async Task<FabricShipmentDto> CreateAsync(CreateFabricShipmentDto dto)
    {
        Validate(dto);

        if (string.IsNullOrWhiteSpace(dto.ShipmentNumber))
            throw new Exception("El Shipment Number es obligatorio.");

        var exists = await _context.FabricShipments
            .AnyAsync(s => s.ShipmentNumber == dto.ShipmentNumber && s.Active);
        if (exists)
            throw new Exception("El Shipment Number ya existe.");

        var fabricPO = await _context.FabricPOs
            .FirstOrDefaultAsync(p => p.ID == dto.FabricPOId && p.Active);
        if (fabricPO is null)
            throw new Exception("El Fabric PO seleccionado no es válido.");

        var fgpo = await _context.Fgpos
            .FirstOrDefaultAsync(f => f.ID == dto.FGPOId && f.Active);
        if (fgpo is null)
            throw new Exception("El FGPO seleccionado no es válido.");

        var lot = await GetOrCreateLotAsync(dto.FabricPOId, dto.FGPOId, dto.LotNumber);

        var entity = new FabricShipment
        {
            ShipmentNumber = dto.ShipmentNumber,
            FabricPOId = dto.FabricPOId,
            FGPOId = dto.FGPOId,
            Supplier = dto.Supplier,
            LotNumber = dto.LotNumber,
            LotId = lot?.ID,
            RollQty = dto.RollQty,
            ShippedQuantity = dto.ShippedQuantity,
            UOM = dto.UOM,
            ShippedWeight = dto.ShippedWeight,
            PackingList = dto.PackingList,
            InvoiceNumber = dto.InvoiceNumber,
            ContainerAWB = dto.ContainerAWB,
            ShippingMethod = dto.ShippingMethod,
            ETD = dto.ETD,
            ETA = dto.ETA,
            ShipmentStatus = dto.ShipmentStatus,
            DeliveredToTexnicaDate = dto.DeliveredToTexnicaDate,
            DataOwner = dto.DataOwner,
            Remarks = dto.Remarks,
            Active = true,
            CreatedAt = DateTime.UtcNow,
        };

        _context.FabricShipments.Add(entity);
        await _context.SaveChangesAsync();

        await _context.Entry(entity).Reference(s => s.FabricPO).LoadAsync();
        await _context.Entry(entity).Reference(s => s.FGPO).Query()
            .Include(f => f!.Customer).LoadAsync();

        return ToDto(entity);
    }

    public async Task<bool> UpdateAsync(int id, UpdateFabricShipmentDto dto)
    {
        var entity = await _context.FabricShipments
            .FirstOrDefaultAsync(s => s.ID == id && s.Active);

        if (entity is null)
            return false;

        ValidateUpdate(dto);

        var exists = await _context.FabricShipments
            .AnyAsync(s => s.ShipmentNumber == dto.ShipmentNumber && s.Active && s.ID != id);
        if (exists)
            throw new Exception("El Shipment Number ya existe.");

        var fabricPO = await _context.FabricPOs
            .FirstOrDefaultAsync(p => p.ID == dto.FabricPOId && p.Active);
        if (fabricPO is null)
            throw new Exception("El Fabric PO seleccionado no es válido.");

        var fgpo = await _context.Fgpos
            .FirstOrDefaultAsync(f => f.ID == dto.FGPOId && f.Active);
        if (fgpo is null)
            throw new Exception("El FGPO seleccionado no es válido.");

        var lot = await GetOrCreateLotAsync(dto.FabricPOId, dto.FGPOId, dto.LotNumber);

        entity.ShipmentNumber = dto.ShipmentNumber;
        entity.FabricPOId = dto.FabricPOId;
        entity.FGPOId = dto.FGPOId;
        entity.Supplier = dto.Supplier;
        entity.LotNumber = dto.LotNumber;
        entity.LotId = lot?.ID;
        entity.RollQty = dto.RollQty;
        entity.ShippedQuantity = dto.ShippedQuantity;
        entity.UOM = dto.UOM;
        entity.ShippedWeight = dto.ShippedWeight;
        entity.PackingList = dto.PackingList;
        entity.InvoiceNumber = dto.InvoiceNumber;
        entity.ContainerAWB = dto.ContainerAWB;
        entity.ShippingMethod = dto.ShippingMethod;
        entity.ETD = dto.ETD;
        entity.ETA = dto.ETA;
        entity.ShipmentStatus = dto.ShipmentStatus;
        entity.DeliveredToTexnicaDate = dto.DeliveredToTexnicaDate;
        entity.DataOwner = dto.DataOwner;
        entity.Remarks = dto.Remarks;
        entity.UpdatedAt = DateTime.UtcNow;

        _context.FabricShipments.Update(entity);
        await _context.SaveChangesAsync();

        return true;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var entity = await _context.FabricShipments
            .FirstOrDefaultAsync(s => s.ID == id && s.Active);

        if (entity is null)
            return false;

        entity.Active = false;
        entity.UpdatedAt = DateTime.UtcNow;

        _context.FabricShipments.Update(entity);
        await _context.SaveChangesAsync();

        return true;
    }

    private async Task<Lot?> GetOrCreateLotAsync(int fabricPOId, int fgpoId, string? lotNumber)
    {
        if (string.IsNullOrWhiteSpace(lotNumber))
            return null;

        var lot = await _context.Lots
            .FirstOrDefaultAsync(l => l.LotNumber == lotNumber && l.Active);
        if (lot is null)
        {
            lot = new Lot
            {
                LotNumber = lotNumber,
                FabricPOId = fabricPOId,
                FGPOId = fgpoId,
                Active = true,
                CreatedAt = DateTime.UtcNow,
            };
            _context.Lots.Add(lot);
        }

        return lot;
    }

    public async Task<PagedResultDto<FabricShipmentDto>> GetPagedAsync(int page, int pageSize, string? search, string? sortBy, string? sortOrder, string? fabricPO, string? fgpo, string? lotNumber, string? status)
    {
        if (page < 1) page = 1;
        if (pageSize < 1) pageSize = 10;
        if (pageSize > 100) pageSize = 100;

        var query = _context.FabricShipments
            .Include(s => s.FabricPO)
            .Include(s => s.FGPO)
                .ThenInclude(f => f!.Customer)
            .Where(s => s.Active);

        if (!string.IsNullOrWhiteSpace(search))
        {
            var searchTerm = search.Trim();
            query = query.Where(s =>
                s.ShipmentNumber.Contains(searchTerm) ||
                (s.LotNumber != null && s.LotNumber.Contains(searchTerm)) ||
                (s.Supplier != null && s.Supplier.Contains(searchTerm)) ||
                (s.InvoiceNumber != null && s.InvoiceNumber.Contains(searchTerm)) ||
                (s.ContainerAWB != null && s.ContainerAWB.Contains(searchTerm)) ||
                (s.FabricPO != null && s.FabricPO.FabricPONumber.Contains(searchTerm)) ||
                (s.FGPO != null && s.FGPO.FGPONumber.Contains(searchTerm)));
        }

        if (!string.IsNullOrWhiteSpace(fabricPO))
            query = query.Where(s => s.FabricPO != null && s.FabricPO.FabricPONumber.Contains(fabricPO.Trim()));

        if (!string.IsNullOrWhiteSpace(fgpo))
            query = query.Where(s => s.FGPO != null && s.FGPO.FGPONumber.Contains(fgpo.Trim()));

        if (!string.IsNullOrWhiteSpace(lotNumber))
            query = query.Where(s => s.LotNumber != null && s.LotNumber.Contains(lotNumber.Trim()));

        if (!string.IsNullOrWhiteSpace(status))
            query = query.Where(s => s.ShipmentStatus == status);

        var totalCount = await query.CountAsync();
        var totalPages = (int)Math.Ceiling(totalCount / (double)pageSize);

        var sortByLower = sortBy?.ToLowerInvariant();
        var sortOrderLower = sortOrder?.ToLowerInvariant();
        var descending = sortOrderLower == "desc";

        IQueryable<FabricShipment> orderedQuery = (sortByLower, descending) switch
        {
            ("shipmentnumber", false) => query.OrderBy(s => s.ShipmentNumber),
            ("shipmentnumber", true) => query.OrderByDescending(s => s.ShipmentNumber),
            ("lotnumber", false) => query.OrderBy(s => s.LotNumber),
            ("lotnumber", true) => query.OrderByDescending(s => s.LotNumber),
            ("supplier", false) => query.OrderBy(s => s.Supplier),
            ("supplier", true) => query.OrderByDescending(s => s.Supplier),
            ("shippedquantity", false) => query.OrderBy(s => s.ShippedQuantity),
            ("shippedquantity", true) => query.OrderByDescending(s => s.ShippedQuantity),
            ("etd", false) => query.OrderBy(s => s.ETD),
            ("etd", true) => query.OrderByDescending(s => s.ETD),
            ("eta", false) => query.OrderBy(s => s.ETA),
            ("eta", true) => query.OrderByDescending(s => s.ETA),
            ("shipmentstatus", false) => query.OrderBy(s => s.ShipmentStatus),
            ("shipmentstatus", true) => query.OrderByDescending(s => s.ShipmentStatus),
            ("createdat", false) => query.OrderBy(s => s.CreatedAt),
            ("createdat", true) => query.OrderByDescending(s => s.CreatedAt),
            _ => query.OrderByDescending(s => s.CreatedAt),
        };

        var items = await orderedQuery
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return new PagedResultDto<FabricShipmentDto>
        {
            Items = items.Select(ToDto),
            TotalCount = totalCount,
            Page = page,
            PageSize = pageSize,
            TotalPages = totalPages,
        };
    }

    private static void Validate(CreateFabricShipmentDto dto)
    {
        if (dto.ShippedQuantity <= 0)
            throw new Exception("La Shipped Quantity debe ser mayor que 0.");

        if (dto.ETD == default)
            throw new Exception("El ETD (Estimated Time of Departure) es obligatorio.");

        if (dto.ETA == default)
            throw new Exception("El ETA (Estimated Time of Arrival) es obligatorio.");

        if (!string.IsNullOrWhiteSpace(dto.UOM) && !ValidUoms.Contains(dto.UOM, StringComparer.OrdinalIgnoreCase))
            throw new Exception($"El UOM '{dto.UOM}' no es válido.");

        if (!string.IsNullOrWhiteSpace(dto.ShipmentStatus) && !ValidShipmentStatuses.Contains(dto.ShipmentStatus, StringComparer.OrdinalIgnoreCase))
            throw new Exception($"El Shipment Status '{dto.ShipmentStatus}' no es válido.");
    }

    private static void ValidateUpdate(UpdateFabricShipmentDto dto)
    {
        if (dto.ShippedQuantity <= 0)
            throw new Exception("La Shipped Quantity debe ser mayor que 0.");

        if (dto.ETD == default)
            throw new Exception("El ETD (Estimated Time of Departure) es obligatorio.");

        if (dto.ETA == default)
            throw new Exception("El ETA (Estimated Time of Arrival) es obligatorio.");

        if (!string.IsNullOrWhiteSpace(dto.UOM) && !ValidUoms.Contains(dto.UOM, StringComparer.OrdinalIgnoreCase))
            throw new Exception($"El UOM '{dto.UOM}' no es válido.");

        if (!string.IsNullOrWhiteSpace(dto.ShipmentStatus) && !ValidShipmentStatuses.Contains(dto.ShipmentStatus, StringComparer.OrdinalIgnoreCase))
            throw new Exception($"El Shipment Status '{dto.ShipmentStatus}' no es válido.");
    }

    private static FabricShipmentDto ToDto(FabricShipment item)
    {
        return new FabricShipmentDto
        {
            ID = item.ID,
            ShipmentNumber = item.ShipmentNumber,
            FabricPOId = item.FabricPOId,
            FabricPONumber = item.FabricPO?.FabricPONumber ?? string.Empty,
            FGPOId = item.FGPOId,
            FGPONumber = item.FGPO?.FGPONumber ?? string.Empty,
            CustomerName = item.FGPO?.Customer?.Name ?? string.Empty,
            Supplier = item.Supplier,
            LotNumber = item.LotNumber,
            LotId = item.LotId,
            RollQty = item.RollQty,
            ShippedQuantity = item.ShippedQuantity,
            UOM = item.UOM,
            ShippedWeight = item.ShippedWeight,
            PackingList = item.PackingList,
            InvoiceNumber = item.InvoiceNumber,
            ContainerAWB = item.ContainerAWB,
            ShippingMethod = item.ShippingMethod,
            ETD = item.ETD,
            ETA = item.ETA,
            ShipmentStatus = item.ShipmentStatus,
            DeliveredToTexnicaDate = item.DeliveredToTexnicaDate,
            InTransitQuantity = item.InTransitQuantity,
            RemainingToDeliver = item.RemainingToDeliver,
            DataOwner = item.DataOwner,
            Remarks = item.Remarks,
            Active = item.Active,
            CreatedAt = item.CreatedAt,
            UpdatedAt = item.UpdatedAt,
        };
    }
}
