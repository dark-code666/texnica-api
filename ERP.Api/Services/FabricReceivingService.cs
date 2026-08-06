using Microsoft.EntityFrameworkCore;
using ERP.Api.Data;
using ERP.Api.Dtos;
using ERP.Api.Domain;
using ERP.Api.Interfaces;

namespace ERP.Api.Services;

public class FabricReceivingService : IFabricReceivingService
{
    private readonly ErpDbContext _context;

    private static readonly string[] ValidStatuses =
    {
        "Pending", "Partially Received", "Fully Received", "Quantity Difference", "Rejected"
    };

    public FabricReceivingService(ErpDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<FabricReceivingDto>> GetAllAsync()
    {
        var items = await _context.FabricReceivings
            .Include(r => r.FabricPO)
            .Include(r => r.FGPO)
                .ThenInclude(f => f!.Customer)
            .Where(r => r.Active)
            .OrderByDescending(r => r.CreatedAt)
            .ToListAsync();

        return items.Select(ToDto);
    }

    public async Task<FabricReceivingDto?> GetByIdAsync(int id)
    {
        var item = await _context.FabricReceivings
            .Include(r => r.FabricPO)
            .Include(r => r.FGPO)
                .ThenInclude(f => f!.Customer)
            .FirstOrDefaultAsync(r => r.ID == id && r.Active);

        return item is null ? null : ToDto(item);
    }

    public async Task<IEnumerable<FabricReceivingDto>> GetByFabricPOAsync(int fabricPOId)
    {
        var items = await _context.FabricReceivings
            .Include(r => r.FabricPO)
            .Include(r => r.FGPO)
                .ThenInclude(f => f!.Customer)
            .Where(r => r.Active && r.FabricPOId == fabricPOId)
            .OrderByDescending(r => r.CreatedAt)
            .ToListAsync();

        return items.Select(ToDto);
    }

    public async Task<IEnumerable<FabricReceivingDto>> GetByFgpoAsync(int fgpoId)
    {
        var items = await _context.FabricReceivings
            .Include(r => r.FabricPO)
            .Include(r => r.FGPO)
                .ThenInclude(f => f!.Customer)
            .Where(r => r.Active && r.FGPOId == fgpoId)
            .OrderByDescending(r => r.CreatedAt)
            .ToListAsync();

        return items.Select(ToDto);
    }

    public async Task<FabricReceivingDto> CreateAsync(CreateFabricReceivingDto dto)
    {
        Validate(dto);

        var exists = await _context.FabricReceivings
            .AnyAsync(r => r.ReceivingNumber == dto.ReceivingNumber && r.Active);
        if (exists)
            throw new Exception("El Receiving Number ya existe.");

        var fabricPO = await _context.FabricPOs
            .FirstOrDefaultAsync(p => p.ID == dto.FabricPOId && p.Active);
        if (fabricPO is null)
            throw new Exception("El Fabric PO seleccionado no es válido.");

        var fgpo = await _context.Fgpos
            .FirstOrDefaultAsync(f => f.ID == dto.FGPOId && f.Active);
        if (fgpo is null)
            throw new Exception("El FGPO seleccionado no es válido.");

        var entity = new FabricReceiving
        {
            ReceivingNumber = dto.ReceivingNumber,
            ReceivingDate = dto.ReceivingDate,
            ShipmentNumber = dto.ShipmentNumber,
            FabricPOId = dto.FabricPOId,
            FGPOId = dto.FGPOId,
            Supplier = dto.Supplier,
            PackingListQty = dto.PackingListQty,
            ActualReceivedQty = dto.ActualReceivedQty,
            ExpectedRolls = dto.ExpectedRolls,
            ReceivedRolls = dto.ReceivedRolls,
            ReceivingStatus = dto.ReceivingStatus,
            WarehouseLocation = dto.WarehouseLocation,
            ReceivedBy = dto.ReceivedBy,
            DataOwner = dto.DataOwner,
            Remarks = dto.Remarks,
            Active = true,
            CreatedAt = DateTime.UtcNow,
        };

        _context.FabricReceivings.Add(entity);
        await _context.SaveChangesAsync();

        await _context.Entry(entity).Reference(r => r.FabricPO).LoadAsync();
        await _context.Entry(entity).Reference(r => r.FGPO).Query()
            .Include(f => f!.Customer).LoadAsync();

        return ToDto(entity);
    }

    public async Task<bool> UpdateAsync(int id, UpdateFabricReceivingDto dto)
    {
        var entity = await _context.FabricReceivings
            .FirstOrDefaultAsync(r => r.ID == id && r.Active);

        if (entity is null)
            return false;

        ValidateUpdate(dto);

        var exists = await _context.FabricReceivings
            .AnyAsync(r => r.ReceivingNumber == dto.ReceivingNumber && r.Active && r.ID != id);
        if (exists)
            throw new Exception("El Receiving Number ya existe.");

        var fabricPO = await _context.FabricPOs
            .FirstOrDefaultAsync(p => p.ID == dto.FabricPOId && p.Active);
        if (fabricPO is null)
            throw new Exception("El Fabric PO seleccionado no es válido.");

        var fgpo = await _context.Fgpos
            .FirstOrDefaultAsync(f => f.ID == dto.FGPOId && f.Active);
        if (fgpo is null)
            throw new Exception("El FGPO seleccionado no es válido.");

        entity.ReceivingNumber = dto.ReceivingNumber;
        entity.ReceivingDate = dto.ReceivingDate;
        entity.ShipmentNumber = dto.ShipmentNumber;
        entity.FabricPOId = dto.FabricPOId;
        entity.FGPOId = dto.FGPOId;
        entity.Supplier = dto.Supplier;
        entity.PackingListQty = dto.PackingListQty;
        entity.ActualReceivedQty = dto.ActualReceivedQty;
        entity.ExpectedRolls = dto.ExpectedRolls;
        entity.ReceivedRolls = dto.ReceivedRolls;
        entity.ReceivingStatus = dto.ReceivingStatus;
        entity.WarehouseLocation = dto.WarehouseLocation;
        entity.ReceivedBy = dto.ReceivedBy;
        entity.DataOwner = dto.DataOwner;
        entity.Remarks = dto.Remarks;
        entity.UpdatedAt = DateTime.UtcNow;

        _context.FabricReceivings.Update(entity);
        await _context.SaveChangesAsync();

        return true;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var entity = await _context.FabricReceivings
            .FirstOrDefaultAsync(r => r.ID == id && r.Active);

        if (entity is null)
            return false;

        entity.Active = false;
        entity.UpdatedAt = DateTime.UtcNow;

        _context.FabricReceivings.Update(entity);
        await _context.SaveChangesAsync();

        return true;
    }

    public async Task<PagedResultDto<FabricReceivingDto>> GetPagedAsync(int page, int pageSize, string? search, string? sortBy, string? sortOrder, string? fabricPO, string? fgpo, string? status)
    {
        if (page < 1) page = 1;
        if (pageSize < 1) pageSize = 10;
        if (pageSize > 100) pageSize = 100;

        var query = _context.FabricReceivings
            .Include(r => r.FabricPO)
            .Include(r => r.FGPO)
                .ThenInclude(f => f!.Customer)
            .Where(r => r.Active);

        if (!string.IsNullOrWhiteSpace(search))
        {
            var searchTerm = search.Trim();
            query = query.Where(r =>
                r.ReceivingNumber.Contains(searchTerm) ||
                (r.ShipmentNumber != null && r.ShipmentNumber.Contains(searchTerm)) ||
                (r.Supplier != null && r.Supplier.Contains(searchTerm)) ||
                (r.ReceivedBy != null && r.ReceivedBy.Contains(searchTerm)) ||
                (r.WarehouseLocation != null && r.WarehouseLocation.Contains(searchTerm)) ||
                (r.FabricPO != null && r.FabricPO.FabricPONumber.Contains(searchTerm)) ||
                (r.FGPO != null && r.FGPO.FGPONumber.Contains(searchTerm)));
        }

        if (!string.IsNullOrWhiteSpace(fabricPO))
            query = query.Where(r => r.FabricPO != null && r.FabricPO.FabricPONumber.Contains(fabricPO.Trim()));

        if (!string.IsNullOrWhiteSpace(fgpo))
            query = query.Where(r => r.FGPO != null && r.FGPO.FGPONumber.Contains(fgpo.Trim()));

        if (!string.IsNullOrWhiteSpace(status))
            query = query.Where(r => r.ReceivingStatus == status);

        var totalCount = await query.CountAsync();
        var totalPages = (int)Math.Ceiling(totalCount / (double)pageSize);

        var sortByLower = sortBy?.ToLowerInvariant();
        var sortOrderLower = sortOrder?.ToLowerInvariant();
        var descending = sortOrderLower == "desc";

        IQueryable<FabricReceiving> orderedQuery = (sortByLower, descending) switch
        {
            ("receivingnumber", false) => query.OrderBy(r => r.ReceivingNumber),
            ("receivingnumber", true) => query.OrderByDescending(r => r.ReceivingNumber),
            ("shipmentnumber", false) => query.OrderBy(r => r.ShipmentNumber),
            ("shipmentnumber", true) => query.OrderByDescending(r => r.ShipmentNumber),
            ("supplier", false) => query.OrderBy(r => r.Supplier),
            ("supplier", true) => query.OrderByDescending(r => r.Supplier),
            ("receivingdate", false) => query.OrderBy(r => r.ReceivingDate),
            ("receivingdate", true) => query.OrderByDescending(r => r.ReceivingDate),
            ("actualreceivedqty", false) => query.OrderBy(r => r.ActualReceivedQty),
            ("actualreceivedqty", true) => query.OrderByDescending(r => r.ActualReceivedQty),
            ("receivingstatus", false) => query.OrderBy(r => r.ReceivingStatus),
            ("receivingstatus", true) => query.OrderByDescending(r => r.ReceivingStatus),
            ("createdat", false) => query.OrderBy(r => r.CreatedAt),
            ("createdat", true) => query.OrderByDescending(r => r.CreatedAt),
            _ => query.OrderByDescending(r => r.CreatedAt),
        };

        var items = await orderedQuery
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return new PagedResultDto<FabricReceivingDto>
        {
            Items = items.Select(ToDto),
            TotalCount = totalCount,
            Page = page,
            PageSize = pageSize,
            TotalPages = totalPages,
        };
    }

    private static void Validate(CreateFabricReceivingDto dto)
    {
        if (dto.ReceivingDate == default)
            throw new Exception("La Receiving Date es obligatoria.");

        if (dto.PackingListQty < 0)
            throw new Exception("La Packing List Qty no puede ser negativa.");

        if (dto.ActualReceivedQty < 0)
            throw new Exception("La Actual Received Qty no puede ser negativa.");

        if (!string.IsNullOrWhiteSpace(dto.ReceivingStatus) && !ValidStatuses.Contains(dto.ReceivingStatus, StringComparer.OrdinalIgnoreCase))
            throw new Exception($"El Receiving Status '{dto.ReceivingStatus}' no es válido.");
    }

    private static void ValidateUpdate(UpdateFabricReceivingDto dto)
    {
        if (dto.ReceivingDate == default)
            throw new Exception("La Receiving Date es obligatoria.");

        if (dto.PackingListQty < 0)
            throw new Exception("La Packing List Qty no puede ser negativa.");

        if (dto.ActualReceivedQty < 0)
            throw new Exception("La Actual Received Qty no puede ser negativa.");

        if (!string.IsNullOrWhiteSpace(dto.ReceivingStatus) && !ValidStatuses.Contains(dto.ReceivingStatus, StringComparer.OrdinalIgnoreCase))
            throw new Exception($"El Receiving Status '{dto.ReceivingStatus}' no es válido.");
    }

    private static FabricReceivingDto ToDto(FabricReceiving item)
    {
        return new FabricReceivingDto
        {
            ID = item.ID,
            ReceivingNumber = item.ReceivingNumber,
            ReceivingDate = item.ReceivingDate,
            ShipmentNumber = item.ShipmentNumber,
            FabricPOId = item.FabricPOId,
            FabricPONumber = item.FabricPO?.FabricPONumber ?? string.Empty,
            FGPOId = item.FGPOId,
            FGPONumber = item.FGPO?.FGPONumber ?? string.Empty,
            CustomerName = item.FGPO?.Customer?.Name ?? string.Empty,
            Supplier = item.Supplier,
            PackingListQty = item.PackingListQty,
            ActualReceivedQty = item.ActualReceivedQty,
            ReceivingVariance = item.ReceivingVariance,
            ReceivingShortage = item.ReceivingShortage,
            ReceivingOverQty = item.ReceivingOverQty,
            ExpectedRolls = item.ExpectedRolls,
            ReceivedRolls = item.ReceivedRolls,
            MissingRolls = item.MissingRolls,
            ReceivingStatus = item.ReceivingStatus,
            WarehouseLocation = item.WarehouseLocation,
            ReceivedBy = item.ReceivedBy,
            DataOwner = item.DataOwner,
            Remarks = item.Remarks,
            Active = item.Active,
            CreatedAt = item.CreatedAt,
            UpdatedAt = item.UpdatedAt,
        };
    }
}
