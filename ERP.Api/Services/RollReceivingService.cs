using Microsoft.EntityFrameworkCore;
using ERP.Api.Data;
using ERP.Api.Dtos;
using ERP.Api.Domain;
using ERP.Api.Interfaces;

namespace ERP.Api.Services;

public class RollReceivingService : IRollReceivingService
{
    private readonly ErpDbContext _context;

    public RollReceivingService(ErpDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<RollReceivingDto>> GetAllAsync()
    {
        var items = await _context.RollReceivings
            .Include(r => r.Receiving)
            .Include(r => r.FabricPO).ThenInclude(p => p.Supplier)
            .Include(r => r.FGPO)
                .ThenInclude(f => f!.Customer)
            .Where(r => r.Active)
            .OrderByDescending(r => r.CreatedAt)
            .ToListAsync();

        return items.Select(ToDto);
    }

    public async Task<RollReceivingDto?> GetByIdAsync(int id)
    {
        var item = await _context.RollReceivings
            .Include(r => r.Receiving)
            .Include(r => r.FabricPO).ThenInclude(p => p.Supplier)
            .Include(r => r.FGPO)
                .ThenInclude(f => f!.Customer)
            .FirstOrDefaultAsync(r => r.ID == id && r.Active);

        return item is null ? null : ToDto(item);
    }

    public async Task<IEnumerable<RollReceivingDto>> GetByReceivingAsync(int receivingId)
    {
        var items = await _context.RollReceivings
            .Include(r => r.Receiving)
            .Include(r => r.FabricPO).ThenInclude(p => p.Supplier)
            .Include(r => r.FGPO)
                .ThenInclude(f => f!.Customer)
            .Where(r => r.Active && r.ReceivingId == receivingId)
            .OrderByDescending(r => r.CreatedAt)
            .ToListAsync();

        return items.Select(ToDto);
    }

    public async Task<RollReceivingDto> CreateAsync(CreateRollReceivingDto dto)
    {
        Validate(dto);

        var receiving = await _context.FabricReceivings
            .Include(r => r.FGPO)
                .ThenInclude(f => f!.Customer)
            .FirstOrDefaultAsync(r => r.ID == dto.ReceivingId && r.Active);
        if (receiving is null)
            throw new Exception("El Fabric Receiving seleccionado no es válido.");

        var lot = await GetOrCreateLotAsync(receiving.FabricPOId, receiving.FGPOId, dto.LotNumber);

        var entity = new RollReceiving
        {
            ReceivingId = receiving.ID,
            ReceivingNumber = receiving.ReceivingNumber,
            // Derivado del Fabric Receiving: nunca se pide ni se desincroniza
            FabricPOId = receiving.FabricPOId,
            FGPOId = receiving.FGPOId,
            Color = receiving.FGPO?.FgpoLines?.FirstOrDefault(l => l.Active)?.Color?.ColorName,
            LotNumber = dto.LotNumber,
            Lot = lot,
            RollNumber = dto.RollNumber,
            SupplierRollNumber = dto.SupplierRollNumber,
            GrossWeight = dto.GrossWeight,
            NetWeight = dto.NetWeight,
            ActualYardage = dto.ActualYardage,
            ActualWidth = dto.ActualWidth,
            ActualGSM = dto.ActualGSM,
            ShadeGroup = dto.ShadeGroup,
            DamagedQty = dto.DamagedQty,
            Condition = dto.Condition,
            // Defaults desde el parent si no se envían
            WarehouseLocation = string.IsNullOrWhiteSpace(dto.WarehouseLocation) ? receiving.WarehouseLocation : dto.WarehouseLocation,
            ReceivedDate = dto.ReceivedDate == default ? receiving.ReceivingDate : dto.ReceivedDate,
            DataOwnerId = dto.DataOwnerId ?? receiving.DataOwnerId,
            Comments = dto.Comments,
            Active = true,
            CreatedAt = DateTime.UtcNow,
        };

        _context.RollReceivings.Add(entity);
        await _context.SaveChangesAsync();

        await _context.Entry(entity).Reference(r => r.Receiving).LoadAsync();
        await _context.Entry(entity).Reference(r => r.FabricPO).Query().Include(p => p.Supplier).LoadAsync();
        await _context.Entry(entity).Reference(r => r.FGPO).Query()
            .Include(f => f!.Customer).LoadAsync();

        return ToDto(entity);
    }

    public async Task<bool> UpdateAsync(int id, UpdateRollReceivingDto dto)
    {
        var entity = await _context.RollReceivings
            .FirstOrDefaultAsync(r => r.ID == id && r.Active);

        if (entity is null)
            return false;

        ValidateUpdate(dto);

        var receiving = await _context.FabricReceivings
            .Include(r => r.FGPO)
            .FirstOrDefaultAsync(r => r.ID == dto.ReceivingId && r.Active);
        if (receiving is null)
            throw new Exception("El Fabric Receiving seleccionado no es válido.");

        var lot = await GetOrCreateLotAsync(receiving.FabricPOId, receiving.FGPOId, dto.LotNumber);

        entity.ReceivingId = receiving.ID;
        entity.ReceivingNumber = receiving.ReceivingNumber;
        // Derivado del Fabric Receiving: nunca se pide ni se desincroniza
        entity.FabricPOId = receiving.FabricPOId;
        entity.FGPOId = receiving.FGPOId;
        entity.Color = receiving.FGPO?.FgpoLines?.FirstOrDefault(l => l.Active)?.Color?.ColorName;
        entity.LotNumber = dto.LotNumber;
        entity.Lot = lot;
        entity.RollNumber = dto.RollNumber;
        entity.SupplierRollNumber = dto.SupplierRollNumber;
        entity.GrossWeight = dto.GrossWeight;
        entity.NetWeight = dto.NetWeight;
        entity.ActualYardage = dto.ActualYardage;
        entity.ActualWidth = dto.ActualWidth;
        entity.ActualGSM = dto.ActualGSM;
        entity.ShadeGroup = dto.ShadeGroup;
        entity.DamagedQty = dto.DamagedQty;
        entity.Condition = dto.Condition;
        // Defaults desde el parent si no se envían
        entity.WarehouseLocation = string.IsNullOrWhiteSpace(dto.WarehouseLocation) ? receiving.WarehouseLocation : dto.WarehouseLocation;
        entity.ReceivedDate = dto.ReceivedDate == default ? receiving.ReceivingDate : dto.ReceivedDate;
        entity.DataOwnerId = dto.DataOwnerId ?? receiving.DataOwnerId;
        entity.Comments = dto.Comments;
        entity.UpdatedAt = DateTime.UtcNow;

        _context.RollReceivings.Update(entity);
        await _context.SaveChangesAsync();

        return true;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var entity = await _context.RollReceivings
            .FirstOrDefaultAsync(r => r.ID == id && r.Active);

        if (entity is null)
            return false;

        entity.Active = false;
        entity.UpdatedAt = DateTime.UtcNow;

        _context.RollReceivings.Update(entity);
        await _context.SaveChangesAsync();

        return true;
    }

    public async Task<PagedResultDto<RollReceivingDto>> GetPagedAsync(int page, int pageSize, string? search, string? sortBy, string? sortOrder, string? receiving, string? fabricPO, string? lotNumber)
    {
        if (page < 1) page = 1;
        if (pageSize < 1) pageSize = 10;
        if (pageSize > 100) pageSize = 100;

        var query = _context.RollReceivings
            .Include(r => r.Receiving)
            .Include(r => r.FabricPO).ThenInclude(p => p.Supplier)
            .Include(r => r.FGPO)
                .ThenInclude(f => f!.Customer)
            .Where(r => r.Active);

        if (!string.IsNullOrWhiteSpace(search))
        {
            var searchTerm = search.Trim();
            query = query.Where(r =>
                (r.RollNumber != null && r.RollNumber.Contains(searchTerm)) ||
                (r.SupplierRollNumber != null && r.SupplierRollNumber.Contains(searchTerm)) ||
                (r.LotNumber != null && r.LotNumber.Contains(searchTerm)) ||
                (r.FabricPO != null && r.FabricPO.Supplier != null && r.FabricPO.Supplier.Name.Contains(searchTerm)) ||
                (r.ShadeGroup != null && r.ShadeGroup.Contains(searchTerm)) ||
                (r.ReceivingNumber != null && r.ReceivingNumber.Contains(searchTerm)) ||
                (r.FabricPO != null && r.FabricPO.FabricPONumber.Contains(searchTerm)) ||
                (r.FGPO != null && r.FGPO.FGPONumber.Contains(searchTerm)));
        }

        if (!string.IsNullOrWhiteSpace(receiving))
            query = query.Where(r => r.ReceivingNumber != null && r.ReceivingNumber.Contains(receiving.Trim()));

        if (!string.IsNullOrWhiteSpace(fabricPO))
            query = query.Where(r => r.FabricPO != null && r.FabricPO.FabricPONumber.Contains(fabricPO.Trim()));

        if (!string.IsNullOrWhiteSpace(lotNumber))
            query = query.Where(r => r.LotNumber != null && r.LotNumber.Contains(lotNumber.Trim()));

        var totalCount = await query.CountAsync();
        var totalPages = (int)Math.Ceiling(totalCount / (double)pageSize);

        var sortByLower = sortBy?.ToLowerInvariant();
        var sortOrderLower = sortOrder?.ToLowerInvariant();
        var descending = sortOrderLower == "desc";

        IQueryable<RollReceiving> orderedQuery = (sortByLower, descending) switch
        {
            ("rollnumber", false) => query.OrderBy(r => r.RollNumber),
            ("rollnumber", true) => query.OrderByDescending(r => r.RollNumber),
            ("lotnumber", false) => query.OrderBy(r => r.LotNumber),
            ("lotnumber", true) => query.OrderByDescending(r => r.LotNumber),
            ("supplier", false) => query.OrderBy(r => r.FabricPO != null && r.FabricPO.Supplier != null ? r.FabricPO.Supplier.Name : null),
            ("supplier", true) => query.OrderByDescending(r => r.FabricPO != null && r.FabricPO.Supplier != null ? r.FabricPO.Supplier.Name : null),
            ("actualyardage", false) => query.OrderBy(r => r.ActualYardage),
            ("actualyardage", true) => query.OrderByDescending(r => r.ActualYardage),
            ("receiveddate", false) => query.OrderBy(r => r.ReceivedDate),
            ("receiveddate", true) => query.OrderByDescending(r => r.ReceivedDate),
            ("createdat", false) => query.OrderBy(r => r.CreatedAt),
            ("createdat", true) => query.OrderByDescending(r => r.CreatedAt),
            _ => query.OrderByDescending(r => r.CreatedAt),
        };

        var items = await orderedQuery
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return new PagedResultDto<RollReceivingDto>
        {
            Items = items.Select(ToDto),
            TotalCount = totalCount,
            Page = page,
            PageSize = pageSize,
            TotalPages = totalPages,
        };
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

    private static void Validate(CreateRollReceivingDto dto)
    {
        if (dto.GrossWeight < 0)
            throw new Exception("El Gross Weight no puede ser negativo.");

        if (dto.NetWeight < 0)
            throw new Exception("El Net Weight no puede ser negativo.");

        if (dto.ActualYardage < 0)
            throw new Exception("El Actual Yardage no puede ser negativo.");

        if (string.IsNullOrWhiteSpace(dto.RollNumber))
            throw new Exception("El Roll Number es obligatorio.");
    }

    private static void ValidateUpdate(UpdateRollReceivingDto dto)
    {
        if (dto.GrossWeight < 0)
            throw new Exception("El Gross Weight no puede ser negativo.");

        if (dto.NetWeight < 0)
            throw new Exception("El Net Weight no puede ser negativo.");

        if (dto.ActualYardage < 0)
            throw new Exception("El Actual Yardage no puede ser negativo.");

        if (string.IsNullOrWhiteSpace(dto.RollNumber))
            throw new Exception("El Roll Number es obligatorio.");
    }

    private static RollReceivingDto ToDto(RollReceiving item)
    {
        return new RollReceivingDto
        {
            ID = item.ID,
            ReceivingId = item.ReceivingId,
            ReceivingNumber = item.Receiving?.ReceivingNumber ?? item.ReceivingNumber ?? string.Empty,
            FabricPOId = item.FabricPOId,
            FabricPONumber = item.FabricPO?.FabricPONumber ?? string.Empty,
            FGPOId = item.FGPOId,
            FGPONumber = item.FGPO?.FGPONumber ?? string.Empty,
            CustomerName = item.FGPO?.Customer?.Name ?? string.Empty,
            Supplier = item.FabricPO?.Supplier?.Name,
            LotNumber = item.LotNumber,
            LotId = item.LotId,
            RollNumber = item.RollNumber,
            SupplierRollNumber = item.SupplierRollNumber,
            Color = item.Color,
            GrossWeight = item.GrossWeight,
            NetWeight = item.NetWeight,
            ActualYardage = item.ActualYardage,
            ActualWidth = item.ActualWidth,
            ActualGSM = item.ActualGSM,
            ShadeGroup = item.ShadeGroup,
            DamagedQty = item.DamagedQty,
            Condition = item.Condition,
            WarehouseLocation = item.WarehouseLocation,
            ReceivedDate = item.ReceivedDate,
            DataOwner = item.DataOwner?.UserName,
            Comments = item.Comments,
            Active = item.Active,
            CreatedAt = item.CreatedAt,
            UpdatedAt = item.UpdatedAt,
        };
    }
}
