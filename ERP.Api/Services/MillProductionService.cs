using Microsoft.EntityFrameworkCore;
using ERP.Api.Data;
using ERP.Api.Dtos;
using ERP.Api.Domain;
using ERP.Api.Interfaces;

namespace ERP.Api.Services;

public class MillProductionService : IMillProductionService
{
    private readonly ErpDbContext _context;

    private static readonly string[] ValidStatuses =
    {
        "Not Started", "Pending", "In Progress", "Partially Completed", "Completed",
        "On Hold", "Cancelled"
    };

    public MillProductionService(ErpDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<MillProductionDto>> GetAllAsync()
    {
        var items = await _context.MillProductions
            .Include(m => m.FabricPO)
            .Include(m => m.FGPO)
                .ThenInclude(f => f!.Customer)
            .Where(m => m.Active)
            .OrderByDescending(m => m.CreatedAt)
            .ToListAsync();

        return items.Select(ToDto);
    }

    public async Task<MillProductionDto?> GetByIdAsync(int id)
    {
        var item = await _context.MillProductions
            .Include(m => m.FabricPO)
            .Include(m => m.FGPO)
                .ThenInclude(f => f!.Customer)
            .FirstOrDefaultAsync(m => m.ID == id && m.Active);

        return item is null ? null : ToDto(item);
    }

    public async Task<IEnumerable<MillProductionDto>> GetByFabricPOAsync(int fabricPOId)
    {
        var items = await _context.MillProductions
            .Include(m => m.FabricPO)
            .Include(m => m.FGPO)
                .ThenInclude(f => f!.Customer)
            .Where(m => m.Active && m.FabricPOId == fabricPOId)
            .OrderByDescending(m => m.CreatedAt)
            .ToListAsync();

        return items.Select(ToDto);
    }

    public async Task<IEnumerable<MillProductionDto>> GetByFgpoAsync(int fgpoId)
    {
        var items = await _context.MillProductions
            .Include(m => m.FabricPO)
            .Include(m => m.FGPO)
                .ThenInclude(f => f!.Customer)
            .Where(m => m.Active && m.FGPOId == fgpoId)
            .OrderByDescending(m => m.CreatedAt)
            .ToListAsync();

        return items.Select(ToDto);
    }

    public async Task<MillProductionDto> CreateAsync(CreateMillProductionDto dto)
    {
        Validate(dto);

        var fabricPO = await _context.FabricPOs
            .FirstOrDefaultAsync(p => p.ID == dto.FabricPOId && p.Active);
        if (fabricPO is null)
            throw new Exception("El Fabric PO seleccionado no es válido.");

        var fgpo = await _context.Fgpos
            .FirstOrDefaultAsync(f => f.ID == dto.FGPOId && f.Active);
        if (fgpo is null)
            throw new Exception("El FGPO seleccionado no es válido.");

        // Crear o reutilizar el Lot (integridad referencial del lote)
        var lot = await GetOrCreateLotAsync(dto.FabricPOId, dto.FGPOId, dto.LotNumber, dto.ProducedQuantity);

        var entity = new MillProduction
        {
            FabricPOId = dto.FabricPOId,
            FGPOId = dto.FGPOId,
            Style = dto.Style,
            Color = dto.Color,
            PlannedQuantity = dto.PlannedQuantity,
            ProducedQuantity = dto.ProducedQuantity,
            LotNumber = dto.LotNumber,
            Lot = lot,
            RollQuantity = dto.RollQuantity,
            YardageOrQty = dto.YardageOrQty,
            Weight = dto.Weight,
            StartDate = dto.StartDate,
            FinishDate = dto.FinishDate,
            PlannedExport = dto.PlannedExport,
            ActualExport = dto.ActualExport,
            Status = dto.Status,
            DataOwnerId = dto.DataOwnerId,
            Remarks = dto.Remarks,
            Active = true,
            CreatedAt = DateTime.UtcNow,
        };

        _context.MillProductions.Add(entity);
        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateException ex)
        {
            throw new Exception($"Error al guardar: {ex.InnerException?.Message ?? ex.Message}");
        }

        await _context.Entry(entity).Reference(m => m.FabricPO).LoadAsync();
        await _context.Entry(entity).Reference(m => m.FGPO).Query()
            .Include(f => f!.Customer).LoadAsync();

        return ToDto(entity);
    }

    public async Task<bool> UpdateAsync(int id, UpdateMillProductionDto dto)
    {
        var entity = await _context.MillProductions
            .FirstOrDefaultAsync(m => m.ID == id && m.Active);

        if (entity is null)
            return false;

        ValidateUpdate(dto);

        var fabricPO = await _context.FabricPOs
            .FirstOrDefaultAsync(p => p.ID == dto.FabricPOId && p.Active);
        if (fabricPO is null)
            throw new Exception("El Fabric PO seleccionado no es válido.");

        var fgpo = await _context.Fgpos
            .FirstOrDefaultAsync(f => f.ID == dto.FGPOId && f.Active);
        if (fgpo is null)
            throw new Exception("El FGPO seleccionado no es válido.");

        var lot = await GetOrCreateLotAsync(dto.FabricPOId, dto.FGPOId, dto.LotNumber, dto.ProducedQuantity);

        entity.FabricPOId = dto.FabricPOId;
        entity.FGPOId = dto.FGPOId;
        entity.Style = dto.Style;
        entity.Color = dto.Color;
        entity.PlannedQuantity = dto.PlannedQuantity;
        entity.ProducedQuantity = dto.ProducedQuantity;
        entity.LotNumber = dto.LotNumber;
        entity.Lot = lot;
        entity.RollQuantity = dto.RollQuantity;
        entity.YardageOrQty = dto.YardageOrQty;
        entity.Weight = dto.Weight;
        entity.StartDate = dto.StartDate;
        entity.FinishDate = dto.FinishDate;
        entity.PlannedExport = dto.PlannedExport;
        entity.ActualExport = dto.ActualExport;
        entity.Status = dto.Status;
        entity.DataOwnerId = dto.DataOwnerId;
        entity.Remarks = dto.Remarks;
        entity.UpdatedAt = DateTime.UtcNow;

        _context.MillProductions.Update(entity);
        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateException ex)
        {
            throw new Exception($"Error al guardar: {ex.InnerException?.Message ?? ex.Message}");
        }

        return true;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var entity = await _context.MillProductions
            .FirstOrDefaultAsync(m => m.ID == id && m.Active);

        if (entity is null)
            return false;

        entity.Active = false;
        entity.UpdatedAt = DateTime.UtcNow;

        _context.MillProductions.Update(entity);
        await _context.SaveChangesAsync();

        return true;
    }

    private async Task<Lot?> GetOrCreateLotAsync(int fabricPOId, int fgpoId, string? lotNumber, decimal producedQuantity)
    {
        if (string.IsNullOrWhiteSpace(lotNumber))
            return null;

        // Buscar por LotNumber SIN filtrar Active: si existiera un lote inactivo
        // con el mismo número, el índice único lo bloquearía al intentar crear otro.
        var lot = await _context.Lots.FirstOrDefaultAsync(l => l.LotNumber == lotNumber);
        if (lot is null)
        {
            lot = new Lot
            {
                LotNumber = lotNumber,
                FabricPOId = fabricPOId,
                FGPOId = fgpoId,
                ProducedQuantity = producedQuantity,
                Active = true,
                CreatedAt = DateTime.UtcNow,
            };
            _context.Lots.Add(lot);
        }
        else
        {
            lot.ProducedQuantity = producedQuantity;
            lot.FabricPOId = fabricPOId;
            lot.FGPOId = fgpoId;
            lot.Active = true;
        }

        return lot;
    }

    public async Task<PagedResultDto<MillProductionDto>> GetPagedAsync(int page, int pageSize, string? search, string? sortBy, string? sortOrder, string? fabricPO, string? fgpo, string? supplier, string? status)
    {
        if (page < 1) page = 1;
        if (pageSize < 1) pageSize = 10;
        if (pageSize > 100) pageSize = 100;

        var query = _context.MillProductions
            .Include(m => m.FabricPO).ThenInclude(p => p!.Supplier)
            .Include(m => m.FabricPO).ThenInclude(p => p!.Component)
            .Include(m => m.FGPO).ThenInclude(f => f!.Customer)
            .Include(m => m.DataOwner)
            .Where(m => m.Active);

        if (!string.IsNullOrWhiteSpace(search))
        {
            var searchTerm = search.Trim();
            query = query.Where(m =>
                (m.LotNumber != null && m.LotNumber.Contains(searchTerm)) ||
                (m.FabricPO != null && m.FabricPO.Supplier != null && m.FabricPO.Supplier.Name.Contains(searchTerm)) ||
                (m.Style != null && m.Style.Contains(searchTerm)) ||
                (m.FabricPO != null && m.FabricPO.Component != null && m.FabricPO.Component.ComponentCode.Contains(searchTerm)) ||
                (m.FabricPO != null && m.FabricPO.FabricPONumber.Contains(searchTerm)) ||
                (m.FGPO != null && m.FGPO.FGPONumber.Contains(searchTerm)));
        }

        if (!string.IsNullOrWhiteSpace(fabricPO))
            query = query.Where(m => m.FabricPO != null && m.FabricPO.FabricPONumber.Contains(fabricPO.Trim()));

        if (!string.IsNullOrWhiteSpace(fgpo))
            query = query.Where(m => m.FGPO != null && m.FGPO.FGPONumber.Contains(fgpo.Trim()));

        if (!string.IsNullOrWhiteSpace(supplier))
            query = query.Where(m => m.FabricPO != null && m.FabricPO.Supplier != null && m.FabricPO.Supplier.Name.Contains(supplier.Trim()));

        if (!string.IsNullOrWhiteSpace(status))
            query = query.Where(m => m.Status == status);

        var totalCount = await query.CountAsync();
        var totalPages = (int)Math.Ceiling(totalCount / (double)pageSize);

        var sortByLower = sortBy?.ToLowerInvariant();
        var sortOrderLower = sortOrder?.ToLowerInvariant();
        var descending = sortOrderLower == "desc";

        IQueryable<MillProduction> orderedQuery = (sortByLower, descending) switch
        {
            ("lotnumber", false) => query.OrderBy(m => m.LotNumber),
            ("lotnumber", true) => query.OrderByDescending(m => m.LotNumber),
            ("supplier", false) => query.OrderBy(m => m.FabricPO != null && m.FabricPO.Supplier != null ? m.FabricPO.Supplier.Name : null),
            ("supplier", true) => query.OrderByDescending(m => m.FabricPO != null && m.FabricPO.Supplier != null ? m.FabricPO.Supplier.Name : null),
            ("plannedquantity", false) => query.OrderBy(m => m.PlannedQuantity),
            ("plannedquantity", true) => query.OrderByDescending(m => m.PlannedQuantity),
            ("producedquantity", false) => query.OrderBy(m => m.ProducedQuantity),
            ("producedquantity", true) => query.OrderByDescending(m => m.ProducedQuantity),
            ("startdate", false) => query.OrderBy(m => m.StartDate),
            ("startdate", true) => query.OrderByDescending(m => m.StartDate),
            ("completionpercentage", false) => query.OrderBy(m => m.CompletionPercentage),
            ("completionpercentage", true) => query.OrderByDescending(m => m.CompletionPercentage),
            ("status", false) => query.OrderBy(m => m.Status),
            ("status", true) => query.OrderByDescending(m => m.Status),
            ("createdat", false) => query.OrderBy(m => m.CreatedAt),
            ("createdat", true) => query.OrderByDescending(m => m.CreatedAt),
            _ => query.OrderByDescending(m => m.CreatedAt),
        };

        var items = await orderedQuery
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return new PagedResultDto<MillProductionDto>
        {
            Items = items.Select(ToDto),
            TotalCount = totalCount,
            Page = page,
            PageSize = pageSize,
            TotalPages = totalPages,
        };
    }

    private static void Validate(CreateMillProductionDto dto)
    {
        if (dto.PlannedQuantity <= 0)
            throw new Exception("La Planned Quantity debe ser mayor que 0.");

        if (dto.ProducedQuantity < 0)
            throw new Exception("La Produced Quantity no puede ser negativa.");

        if (dto.StartDate == default)
            throw new Exception("La Start Date es obligatoria.");

        if (string.IsNullOrWhiteSpace(dto.LotNumber))
            throw new Exception("El Lot Number es obligatorio.");

        if (!string.IsNullOrWhiteSpace(dto.Status) && !ValidStatuses.Contains(dto.Status, StringComparer.OrdinalIgnoreCase))
            throw new Exception($"El Status '{dto.Status}' no es válido.");
    }

    private static void ValidateUpdate(UpdateMillProductionDto dto)
    {
        if (dto.PlannedQuantity <= 0)
            throw new Exception("La Planned Quantity debe ser mayor que 0.");

        if (dto.ProducedQuantity < 0)
            throw new Exception("La Produced Quantity no puede ser negativa.");

        if (dto.StartDate == default)
            throw new Exception("La Start Date es obligatoria.");

        if (string.IsNullOrWhiteSpace(dto.LotNumber))
            throw new Exception("El Lot Number es obligatorio.");

        if (!string.IsNullOrWhiteSpace(dto.Status) && !ValidStatuses.Contains(dto.Status, StringComparer.OrdinalIgnoreCase))
            throw new Exception($"El Status '{dto.Status}' no es válido.");
    }

    private static MillProductionDto ToDto(MillProduction item)
    {
        return new MillProductionDto
        {
            ID = item.ID,
            FabricPOId = item.FabricPOId,
            FabricPONumber = item.FabricPO?.FabricPONumber ?? string.Empty,
            FGPOId = item.FGPOId,
            FGPONumber = item.FGPO?.FGPONumber ?? string.Empty,
            SupplierName = item.FabricPO?.Supplier?.Name,
            ComponentCode = item.FabricPO?.Component?.ComponentCode,
            Style = item.Style,
            Color = item.Color,
            PlannedQuantity = item.PlannedQuantity,
            ProducedQuantity = item.ProducedQuantity,
            CompletionPercentage = item.CompletionPercentage,
            LotNumber = item.LotNumber,
            LotId = item.LotId,
            RollQuantity = item.RollQuantity,
            YardageOrQty = item.YardageOrQty,
            Weight = item.Weight,
            StartDate = item.StartDate,
            FinishDate = item.FinishDate,
            PlannedExport = item.PlannedExport,
            ActualExport = item.ActualExport,
            Status = item.Status,
            DataOwnerId = item.DataOwnerId,
            DataOwnerName = item.DataOwner?.UserName,
            Remarks = item.Remarks,
            Active = item.Active,
            CreatedAt = item.CreatedAt,
            UpdatedAt = item.UpdatedAt,
        };
    }
}
