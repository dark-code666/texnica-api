using Microsoft.EntityFrameworkCore;
using ERP.Api.Data;
using ERP.Api.Dtos;
using ERP.Api.Domain;
using ERP.Api.Interfaces;

namespace ERP.Api.Services;

public class FinishedGoodsService : IFinishedGoodsService
{
    private readonly ErpDbContext _context;

    public FinishedGoodsService(ErpDbContext context)
    {
        _context = context;
    }

    private IQueryable<FinishedGood> Query()
        => _context.FinishedGoods
            .Include(f => f.FGPO).ThenInclude(g => g!.Customer)
            .Include(f => f.FGPO).ThenInclude(g => g!.FgpoLines).ThenInclude(l => l.Style)
            .Include(f => f.FGPO).ThenInclude(g => g!.FgpoLines).ThenInclude(l => l.Color)
            .Include(f => f.FGPO).ThenInclude(g => g!.FgpoLines).ThenInclude(l => l.Size)
            .Include(f => f.DataOwner);

    public async Task<IEnumerable<FinishedGoodDto>> GetAllAsync()
    {
        var items = await Query()
            .Where(f => f.Active)
            .OrderByDescending(f => f.ReceiptDate)
            .ToListAsync();
        return items.Select(ToDto);
    }

    public async Task<FinishedGoodDto?> GetByIdAsync(int id)
    {
        var item = await Query().FirstOrDefaultAsync(f => f.ID == id && f.Active);
        return item is null ? null : ToDto(item);
    }

    public async Task<IEnumerable<FinishedGoodDto>> GetByFgpoAsync(int fgpoId)
    {
        var items = await Query()
            .Where(f => f.Active && f.FGPOId == fgpoId)
            .OrderByDescending(f => f.ReceiptDate)
            .ToListAsync();
        return items.Select(ToDto);
    }

    public async Task<FinishedGoodDto> CreateAsync(CreateFinishedGoodDto dto)
    {
        Validate(dto);
        var fgpo = await _context.Fgpos
            .Include(f => f.FgpoLines).ThenInclude(l => l.Style)
            .Include(f => f.FgpoLines).ThenInclude(l => l.Color)
            .Include(f => f.FgpoLines).ThenInclude(l => l.Size)
            .FirstOrDefaultAsync(f => f.ID == dto.FGPOId && f.Active)
            ?? throw new Exception("El FGPO seleccionado no es válido.");

        var entity = new FinishedGood
        {
            ReceiptDate = dto.ReceiptDate,
            FGPOId = dto.FGPOId,
            PackedQty = dto.PackedQty,
            WarehouseReceived = dto.WarehouseReceived,
            ReservedForShipment = dto.ReservedForShipment,
            LoadedQty = dto.LoadedQty,
            ShippedQty = dto.ShippedQty,
            WarehouseLocation = dto.WarehouseLocation,
            Status = dto.Status,
            DataOwnerId = dto.DataOwnerId,
            LastUpdated = dto.LastUpdated ?? DateTime.UtcNow,
            Remarks = dto.Remarks,
            Active = true,
            CreatedAt = DateTime.UtcNow,
        };

        _context.FinishedGoods.Add(entity);
        await _context.SaveChangesAsync();
        await _context.Entry(entity).Reference(f => f.FGPO).Query()
            .Include(g => g!.Customer)
            .Include(g => g!.FgpoLines).ThenInclude(l => l.Style)
            .Include(g => g!.FgpoLines).ThenInclude(l => l.Color)
            .Include(g => g!.FgpoLines).ThenInclude(l => l.Size)
            .LoadAsync();
        await _context.Entry(entity).Reference(f => f.DataOwner).LoadAsync();
        return ToDto(entity);
    }

    public async Task<bool> UpdateAsync(int id, UpdateFinishedGoodDto dto)
    {
        var entity = await _context.FinishedGoods.FirstOrDefaultAsync(f => f.ID == id && f.Active);
        if (entity is null)
            return false;

        Validate(dto);
        var fgpo = await _context.Fgpos.FirstOrDefaultAsync(f => f.ID == dto.FGPOId && f.Active)
            ?? throw new Exception("El FGPO seleccionado no es válido.");

        entity.ReceiptDate = dto.ReceiptDate;
        entity.FGPOId = dto.FGPOId;
        entity.PackedQty = dto.PackedQty;
        entity.WarehouseReceived = dto.WarehouseReceived;
        entity.ReservedForShipment = dto.ReservedForShipment;
        entity.LoadedQty = dto.LoadedQty;
        entity.ShippedQty = dto.ShippedQty;
        entity.WarehouseLocation = dto.WarehouseLocation;
        entity.Status = dto.Status;
        entity.DataOwnerId = dto.DataOwnerId;
        entity.LastUpdated = dto.LastUpdated ?? DateTime.UtcNow;
        entity.Remarks = dto.Remarks;
        entity.UpdatedAt = DateTime.UtcNow;

        _context.FinishedGoods.Update(entity);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var entity = await _context.FinishedGoods.FirstOrDefaultAsync(f => f.ID == id && f.Active);
        if (entity is null)
            return false;

        entity.Active = false;
        entity.UpdatedAt = DateTime.UtcNow;
        _context.FinishedGoods.Update(entity);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<PagedResultDto<FinishedGoodDto>> GetPagedAsync(int page, int pageSize, string? search, string? sortBy, string? sortOrder, string? fgpo, string? status)
    {
        if (page < 1) page = 1;
        if (pageSize < 1) pageSize = 10;
        if (pageSize > 100) pageSize = 100;

        var query = Query().Where(f => f.Active);

        if (!string.IsNullOrWhiteSpace(search))
        {
            var term = search.Trim();
            query = query.Where(f =>
                (f.FGPO != null && f.FGPO.FGPONumber.Contains(term)) ||
                (f.FGPO != null && f.FGPO.Customer != null && f.FGPO.Customer.Name.Contains(term)) ||
                (f.FGPO != null && f.FGPO.FgpoLines.Any(l => l.Active && l.Style != null && l.Style.StyleCode.Contains(term))) ||
                (f.FGPO != null && f.FGPO.FgpoLines.Any(l => l.Active && l.Color != null && l.Color.ColorName.Contains(term))) ||
                (f.WarehouseLocation != null && f.WarehouseLocation.Contains(term)) ||
                (f.Status != null && f.Status.Contains(term)) ||
                (f.Remarks != null && f.Remarks.Contains(term)));
        }

        if (!string.IsNullOrWhiteSpace(fgpo))
            query = query.Where(f => f.FGPO != null && f.FGPO.FGPONumber.Contains(fgpo.Trim()));

        if (!string.IsNullOrWhiteSpace(status))
            query = query.Where(f => f.Status != null && f.Status.Contains(status.Trim()));

        var totalCount = await query.CountAsync();
        var totalPages = (int)Math.Ceiling(totalCount / (double)pageSize);

        var sortByLower = sortBy?.ToLowerInvariant();
        var descending = sortOrder?.ToLowerInvariant() == "desc";

        IQueryable<FinishedGood> orderedQuery = (sortByLower, descending) switch
        {
            ("receiptdate", false) => query.OrderBy(f => f.ReceiptDate),
            ("receiptdate", true) => query.OrderByDescending(f => f.ReceiptDate),
            ("warehousereceived", false) => query.OrderBy(f => f.WarehouseReceived),
            ("warehousereceived", true) => query.OrderByDescending(f => f.WarehouseReceived),
            ("warehousebalance", false) => query.OrderBy(f => f.WarehouseBalance),
            ("warehousebalance", true) => query.OrderByDescending(f => f.WarehouseBalance),
            ("readytoshipqty", false) => query.OrderBy(f => f.ReadyToShipQty),
            ("readytoshipqty", true) => query.OrderByDescending(f => f.ReadyToShipQty),
            ("status", false) => query.OrderBy(f => f.Status),
            ("status", true) => query.OrderByDescending(f => f.Status),
            ("createdat", false) => query.OrderBy(f => f.CreatedAt),
            ("createdat", true) => query.OrderByDescending(f => f.CreatedAt),
            _ => query.OrderByDescending(f => f.ReceiptDate),
        };

        var items = await orderedQuery
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return new PagedResultDto<FinishedGoodDto>
        {
            Items = items.Select(ToDto),
            TotalCount = totalCount,
            Page = page,
            PageSize = pageSize,
            TotalPages = totalPages,
        };
    }

    private static void Validate(CreateFinishedGoodDto dto)
    {
        if (dto.ReceiptDate == default)
            throw new Exception("La Receipt Date es obligatoria.");
        if (dto.PackedQty < 0 || dto.WarehouseReceived < 0 || dto.ReservedForShipment < 0
            || dto.LoadedQty < 0 || dto.ShippedQty < 0)
            throw new Exception("Las cantidades no pueden ser negativas.");
    }

    private static void Validate(UpdateFinishedGoodDto dto)
    {
        if (dto.ReceiptDate == default)
            throw new Exception("La Receipt Date es obligatoria.");
        if (dto.PackedQty < 0 || dto.WarehouseReceived < 0 || dto.ReservedForShipment < 0
            || dto.LoadedQty < 0 || dto.ShippedQty < 0)
            throw new Exception("Las cantidades no pueden ser negativas.");
    }

    private static FinishedGoodDto ToDto(FinishedGood item)
    {
        var primaryLine = item.FGPO?.FgpoLines?.FirstOrDefault(l => l.Active);
        return new FinishedGoodDto
        {
            ID = item.ID,
            ReceiptDate = item.ReceiptDate,
            FGPOId = item.FGPOId,
            FGPONumber = item.FGPO?.FGPONumber ?? string.Empty,
            CustomerName = item.FGPO?.Customer?.Name ?? string.Empty,
            Style = primaryLine?.Style?.StyleCode,
            Color = primaryLine?.Color?.ColorName,
            Size = primaryLine?.Size?.SizeCode,
            PackedQty = item.PackedQty,
            WarehouseReceived = item.WarehouseReceived,
            ReservedForShipment = item.ReservedForShipment,
            LoadedQty = item.LoadedQty,
            ShippedQty = item.ShippedQty,
            ReadyToShipQty = item.ReadyToShipQty,
            WarehouseBalance = item.WarehouseBalance,
            WarehouseLocation = item.WarehouseLocation,
            Status = item.Status,
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
