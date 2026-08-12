using Microsoft.EntityFrameworkCore;
using ERP.Api.Data;
using ERP.Api.Dtos;
using ERP.Api.Domain;
using ERP.Api.Interfaces;

namespace ERP.Api.Services;

public class FabricInventoryService : IFabricInventoryService
{
    private readonly ErpDbContext _context;

    public FabricInventoryService(ErpDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<FabricInventoryDto>> GetAllAsync()
    {
        var items = await _context.FabricInventories
            .Include(i => i.FabricPO)
                .ThenInclude(po => po!.Component)
            .Include(i => i.FGPO).ThenInclude(f => f!.Customer)
            .Include(i => i.Lot)
            .Include(i => i.DataOwner)
            .Where(i => i.Active)
            .OrderByDescending(i => i.CreatedAt)
            .ToListAsync();
        return items.Select(ToDto);
    }

    public async Task<FabricInventoryDto?> GetByIdAsync(int id)
    {
        var item = await _context.FabricInventories
            .Include(i => i.FabricPO)
                .ThenInclude(po => po!.Component)
            .Include(i => i.FGPO).ThenInclude(f => f!.Customer)
            .Include(i => i.Lot)
            .Include(i => i.DataOwner)
            .FirstOrDefaultAsync(i => i.ID == id && i.Active);
        return item is null ? null : ToDto(item);
    }

    public async Task<IEnumerable<FabricInventoryDto>> GetByFgpoAsync(int fgpoId)
    {
        var items = await _context.FabricInventories
            .Include(i => i.FabricPO)
                .ThenInclude(po => po!.Component)
            .Include(i => i.FGPO).ThenInclude(f => f!.Customer)
            .Include(i => i.Lot)
            .Include(i => i.DataOwner)
            .Where(i => i.Active && i.FGPOId == fgpoId)
            .OrderByDescending(i => i.CreatedAt)
            .ToListAsync();
        return items.Select(ToDto);
    }

    public async Task<FabricInventoryDto> CreateAsync(CreateFabricInventoryDto dto)
    {
        Validate(dto);
        var fgpo = await _context.Fgpos.FirstOrDefaultAsync(f => f.ID == dto.FGPOId && f.Active)
            ?? throw new Exception("El FGPO seleccionado no es válido.");
        var fabricPo = await _context.FabricPOs.FirstOrDefaultAsync(p => p.ID == dto.FabricPOId && p.Active)
            ?? throw new Exception("El Fabric PO seleccionado no es válido.");
        await ValidateLotAsync(dto.LotId);

        var entity = new FabricInventory
        {
            FabricPOId = dto.FabricPOId,
            FGPOId = dto.FGPOId,
            LotId = dto.LotId,
            ReceivedQuantity = dto.ReceivedQuantity,
            ApprovedQuantity = dto.ApprovedQuantity,
            RejectedQuantity = dto.RejectedQuantity,
            HoldQuantity = dto.HoldQuantity,
            ReservedQuantity = dto.ReservedQuantity,
            IssuedQuantity = dto.IssuedQuantity,
            ReturnedQuantity = dto.ReturnedQuantity,
            ShortageQuantity = dto.ShortageQuantity,
            WarehouseLocation = dto.WarehouseLocation,
            InventoryStatus = dto.InventoryStatus,
            DataOwnerId = dto.DataOwnerId,
            LastUpdated = dto.LastUpdated ?? DateTime.UtcNow,
            Remarks = dto.Remarks,
            Active = true,
            CreatedAt = DateTime.UtcNow,
        };

        _context.FabricInventories.Add(entity);
        await _context.SaveChangesAsync();
        await _context.Entry(entity).Reference(i => i.FabricPO).Query()
            .Include(po => po!.Component).LoadAsync();
        await _context.Entry(entity).Reference(i => i.FGPO).Query().Include(f => f!.Customer).LoadAsync();
        await _context.Entry(entity).Reference(i => i.Lot).LoadAsync();
        await _context.Entry(entity).Reference(i => i.DataOwner).LoadAsync();
        return ToDto(entity);
    }

    public async Task<bool> UpdateAsync(int id, UpdateFabricInventoryDto dto)
    {
        var entity = await _context.FabricInventories.FirstOrDefaultAsync(i => i.ID == id && i.Active);
        if (entity is null)
            return false;

        Validate(dto);
        var fgpo = await _context.Fgpos.FirstOrDefaultAsync(f => f.ID == dto.FGPOId && f.Active)
            ?? throw new Exception("El FGPO seleccionado no es válido.");
        var fabricPo = await _context.FabricPOs.FirstOrDefaultAsync(p => p.ID == dto.FabricPOId && p.Active)
            ?? throw new Exception("El Fabric PO seleccionado no es válido.");
        await ValidateLotAsync(dto.LotId);

        entity.FabricPOId = dto.FabricPOId;
        entity.FGPOId = dto.FGPOId;
        entity.LotId = dto.LotId;
        entity.ReceivedQuantity = dto.ReceivedQuantity;
        entity.ApprovedQuantity = dto.ApprovedQuantity;
        entity.RejectedQuantity = dto.RejectedQuantity;
        entity.HoldQuantity = dto.HoldQuantity;
        entity.ReservedQuantity = dto.ReservedQuantity;
        entity.IssuedQuantity = dto.IssuedQuantity;
        entity.ReturnedQuantity = dto.ReturnedQuantity;
        entity.ShortageQuantity = dto.ShortageQuantity;
        entity.WarehouseLocation = dto.WarehouseLocation;
        entity.InventoryStatus = dto.InventoryStatus;
        entity.DataOwnerId = dto.DataOwnerId;
        entity.LastUpdated = dto.LastUpdated ?? DateTime.UtcNow;
        entity.Remarks = dto.Remarks;
        entity.UpdatedAt = DateTime.UtcNow;

        _context.FabricInventories.Update(entity);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var entity = await _context.FabricInventories.FirstOrDefaultAsync(i => i.ID == id && i.Active);
        if (entity is null)
            return false;

        entity.Active = false;
        entity.UpdatedAt = DateTime.UtcNow;
        _context.FabricInventories.Update(entity);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<PagedResultDto<FabricInventoryDto>> GetPagedAsync(int page, int pageSize, string? search, string? sortBy, string? sortOrder, string? fgpo, string? status)
    {
        if (page < 1) page = 1;
        if (pageSize < 1) pageSize = 10;
        if (pageSize > 100) pageSize = 100;

        var query = _context.FabricInventories
            .Include(i => i.FabricPO)
                .ThenInclude(po => po!.Component)
            .Include(i => i.FGPO).ThenInclude(f => f!.Customer)
            .Include(i => i.Lot)
            .Include(i => i.DataOwner)
            .Where(i => i.Active);

        if (!string.IsNullOrWhiteSpace(search))
        {
            var term = search.Trim();
            query = query.Where(i =>
                (i.FabricPO != null && i.FabricPO.Component != null && i.FabricPO.Component.ComponentCode.Contains(term)) ||
                (i.Lot != null && i.Lot.LotNumber.Contains(term)) ||
                (i.InventoryStatus != null && i.InventoryStatus.Contains(term)) ||
                (i.WarehouseLocation != null && i.WarehouseLocation.Contains(term)) ||
                (i.FabricPO != null && i.FabricPO.FabricPONumber.Contains(term)) ||
                (i.FGPO != null && i.FGPO.FGPONumber.Contains(term)));
        }

        if (!string.IsNullOrWhiteSpace(fgpo))
            query = query.Where(i => i.FGPO != null && i.FGPO.FGPONumber.Contains(fgpo.Trim()));

        if (!string.IsNullOrWhiteSpace(status))
            query = query.Where(i => i.InventoryStatus != null && i.InventoryStatus.Contains(status.Trim()));

        var totalCount = await query.CountAsync();
        var totalPages = (int)Math.Ceiling(totalCount / (double)pageSize);

        var sortByLower = sortBy?.ToLowerInvariant();
        var descending = sortOrder?.ToLowerInvariant() == "desc";

        IQueryable<FabricInventory> orderedQuery = (sortByLower, descending) switch
        {
            ("availablequantity", false) => query.OrderBy(i => i.AvailableQuantity),
            ("availablequantity", true) => query.OrderByDescending(i => i.AvailableQuantity),
            ("shortagequantity", false) => query.OrderBy(i => i.ShortageQuantity),
            ("shortagequantity", true) => query.OrderByDescending(i => i.ShortageQuantity),
            ("inventorystatus", false) => query.OrderBy(i => i.InventoryStatus),
            ("inventorystatus", true) => query.OrderByDescending(i => i.InventoryStatus),
            ("createdat", false) => query.OrderBy(i => i.CreatedAt),
            ("createdat", true) => query.OrderByDescending(i => i.CreatedAt),
            _ => query.OrderByDescending(i => i.CreatedAt),
        };

        var items = await orderedQuery
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return new PagedResultDto<FabricInventoryDto>
        {
            Items = items.Select(ToDto),
            TotalCount = totalCount,
            Page = page,
            PageSize = pageSize,
            TotalPages = totalPages,
        };
    }

    private async Task ValidateLotAsync(int? lotId)
    {
        if (lotId.HasValue && await _context.Lots.FirstOrDefaultAsync(l => l.ID == lotId.Value && l.Active) is null)
            throw new Exception("El Lot seleccionado no es válido.");
    }

    private static void Validate(CreateFabricInventoryDto dto)
    {
        if (dto.ReceivedQuantity < 0 || dto.ApprovedQuantity < 0 || dto.RejectedQuantity < 0 || dto.HoldQuantity < 0 || dto.ReservedQuantity < 0 || dto.IssuedQuantity < 0 || dto.ReturnedQuantity < 0 || dto.ShortageQuantity < 0)
            throw new Exception("Las cantidades no pueden ser negativas.");
    }

    private static void Validate(UpdateFabricInventoryDto dto)
    {
        if (dto.ReceivedQuantity < 0 || dto.ApprovedQuantity < 0 || dto.RejectedQuantity < 0 || dto.HoldQuantity < 0 || dto.ReservedQuantity < 0 || dto.IssuedQuantity < 0 || dto.ReturnedQuantity < 0 || dto.ShortageQuantity < 0)
            throw new Exception("Las cantidades no pueden ser negativas.");
    }

    private static FabricInventoryDto ToDto(FabricInventory item) => new()
    {
        ID = item.ID,
        FabricPOId = item.FabricPOId,
        FabricPONumber = item.FabricPO?.FabricPONumber ?? string.Empty,
        FGPOId = item.FGPOId,
        FGPONumber = item.FGPO?.FGPONumber ?? string.Empty,
        CustomerName = item.FGPO?.Customer?.Name ?? string.Empty,
        ComponentId = item.FabricPO?.ComponentId,
        ComponentCode = item.FabricPO?.Component?.ComponentCode,
        LotId = item.LotId,
        LotNumber = item.Lot?.LotNumber ?? string.Empty,
        UOM = item.FabricPO?.UOM,
        ReceivedQuantity = item.ReceivedQuantity,
        ApprovedQuantity = item.ApprovedQuantity,
        RejectedQuantity = item.RejectedQuantity,
        HoldQuantity = item.HoldQuantity,
        ReservedQuantity = item.ReservedQuantity,
        IssuedQuantity = item.IssuedQuantity,
        ReturnedQuantity = item.ReturnedQuantity,
        AvailableQuantity = item.AvailableQuantity,
        ShortageQuantity = item.ShortageQuantity,
        WarehouseLocation = item.WarehouseLocation,
        InventoryStatus = item.InventoryStatus,
        DataOwnerId = item.DataOwnerId,
        DataOwnerName = item.DataOwner?.UserName,
        LastUpdated = item.LastUpdated,
        Remarks = item.Remarks,
        Active = item.Active,
        CreatedAt = item.CreatedAt,
        UpdatedAt = item.UpdatedAt,
    };
}
