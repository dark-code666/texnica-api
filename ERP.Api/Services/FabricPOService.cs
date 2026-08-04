using Microsoft.EntityFrameworkCore;
using ERP.Api.Data;
using ERP.Api.Dtos;
using ERP.Api.Domain;
using ERP.Api.Interfaces;

namespace ERP.Api.Services;

public class FabricPOService : IFabricPOService
{
    private readonly ErpDbContext _context;

    private static readonly string[] ValidUoms =
    {
        "Yards", "Meters", "Kilograms", "Pounds", "Rolls", "Pieces"
    };

    private static readonly string[] ValidFabricComponents =
    {
        "Body Fabric", "Rib", "Shoulder Tape", "Neck Tape", "Pocketing", "Other"
    };

    private static readonly string[] ValidPoStatuses =
    {
        "Not Started", "Pending", "In Progress", "Partially Completed", "Completed",
        "Approved", "Conditionally Approved", "Rejected", "On Hold", "Closed", "Cancelled"
    };

    public FabricPOService(ErpDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<FabricPODto>> GetAllAsync()
    {
        var items = await _context.FabricPOs
            .Include(p => p.FabricPOFgpos)
                .ThenInclude(pf => pf.FGPO)
                    .ThenInclude(f => f!.Customer)
            .Where(p => p.Active)
            .OrderByDescending(p => p.CreatedAt)
            .ToListAsync();

        return items.Select(ToDto);
    }

    public async Task<FabricPODto?> GetByIdAsync(int id)
    {
        var item = await _context.FabricPOs
            .Include(p => p.FabricPOFgpos)
                .ThenInclude(pf => pf.FGPO)
                    .ThenInclude(f => f!.Customer)
            .FirstOrDefaultAsync(p => p.ID == id && p.Active);

        return item is null ? null : ToDto(item);
    }

    public async Task<IEnumerable<FabricPODto>> GetByFgpoAsync(int fgpoId)
    {
        var items = await _context.FabricPOs
            .Include(p => p.FabricPOFgpos)
                .ThenInclude(pf => pf.FGPO)
                    .ThenInclude(f => f!.Customer)
            .Where(p => p.Active && p.FabricPOFgpos.Any(pf => pf.FGPOId == fgpoId))
            .OrderByDescending(p => p.CreatedAt)
            .ToListAsync();

        return items.Select(ToDto);
    }

    public async Task<FabricPODto> CreateAsync(CreateFabricPODto dto)
    {
        Validate(dto.FabricPONumber, dto.FgpoItems, dto.FabricComponent, dto.UOM, dto.OrderedQuantity, dto.UnitPrice, dto.OrderDate, dto.RequiredCompletion, dto.POStatus);

        // Validar que el número de Fabric PO sea único
        var exists = await _context.FabricPOs
            .AnyAsync(p => p.FabricPONumber == dto.FabricPONumber && p.Active);
        if (exists)
            throw new Exception("El Fabric PO Number ya existe.");

        // Validar que todos los FGPO existan y estén activos
        var fgpoIds = dto.FgpoItems.Select(i => i.FGPOId).Distinct().ToList();
        var fgpos = await _context.Fgpos
            .Where(f => fgpoIds.Contains(f.ID) && f.Active)
            .ToListAsync();
        if (fgpos.Count != fgpoIds.Count)
            throw new Exception("Uno o más FGPO seleccionados no son válidos.");

        // Validar que la suma de Allocated Quantity no exceda el Ordered Quantity total
        ValidateAllocatedQuantity(dto.FgpoItems, dto.OrderedQuantity);

        // PO Amount = Ordered Quantity × Unit Price (ambos valores absolutos, misma moneda)
        var poAmount = dto.OrderedQuantity * dto.UnitPrice;

        var entity = new FabricPO
        {
            FabricPONumber = dto.FabricPONumber,
            Supplier = dto.Supplier,
            FabricMill = dto.FabricMill,
            FabricComponent = dto.FabricComponent,
            OrderedQuantity = dto.OrderedQuantity,
            UOM = dto.UOM,
            UnitPrice = dto.UnitPrice,
            POAmount = poAmount,
            OrderDate = dto.OrderDate,
            RequiredCompletion = dto.RequiredCompletion,
            PlannedExport = dto.PlannedExport,
            PlannedArrival = dto.PlannedArrival,
            POStatus = dto.POStatus,
            PurchaseOwner = dto.PurchaseOwner,
            ApprovedBy = dto.ApprovedBy,
            Remarks = dto.Remarks,
            Active = true,
            CreatedAt = DateTime.UtcNow,
            FabricPOFgpos = dto.FgpoItems.Select(item => new FabricPOFgpo
            {
                FGPOId = item.FGPOId,
                Style = item.Style,
                Color = item.Color,
                AllocatedQuantity = item.AllocatedQuantity,
                LastUpdated = DateTime.UtcNow,
            }).ToList(),
        };

        _context.FabricPOs.Add(entity);
        await _context.SaveChangesAsync();

        // Cargar las relaciones para devolver los nombres
        await _context.Entry(entity).Collection(p => p.FabricPOFgpos).Query()
            .Include(pf => pf.FGPO)
                .ThenInclude(f => f!.Customer)
            .LoadAsync();

        return ToDto(entity);
    }

    public async Task<bool> UpdateAsync(int id, UpdateFabricPODto dto)
    {
        var entity = await _context.FabricPOs
            .Include(p => p.FabricPOFgpos)
            .FirstOrDefaultAsync(p => p.ID == id && p.Active);

        if (entity is null)
        {
            return false;
        }

        Validate(dto.FabricPONumber, dto.FgpoItems, dto.FabricComponent, dto.UOM, dto.OrderedQuantity, dto.UnitPrice, dto.OrderDate, dto.RequiredCompletion, dto.POStatus);

        // Validar que el número de Fabric PO sea único (excluyendo el registro actual)
        var exists = await _context.FabricPOs
            .AnyAsync(p => p.FabricPONumber == dto.FabricPONumber && p.Active && p.ID != id);
        if (exists)
            throw new Exception("El Fabric PO Number ya existe.");

        // Validar que todos los FGPO existan y estén activos
        var fgpoIds = dto.FgpoItems.Select(i => i.FGPOId).Distinct().ToList();
        var fgpos = await _context.Fgpos
            .Where(f => fgpoIds.Contains(f.ID) && f.Active)
            .ToListAsync();
        if (fgpos.Count != fgpoIds.Count)
            throw new Exception("Uno o más FGPO seleccionados no son válidos.");

        // Validar que la suma de Allocated Quantity no exceda el Ordered Quantity total
        ValidateAllocatedQuantity(dto.FgpoItems, dto.OrderedQuantity);

        // PO Amount = Ordered Quantity × Unit Price
        var poAmount = dto.OrderedQuantity * dto.UnitPrice;

        entity.FabricPONumber = dto.FabricPONumber;
        entity.Supplier = dto.Supplier;
        entity.FabricMill = dto.FabricMill;
        entity.FabricComponent = dto.FabricComponent;
        entity.OrderedQuantity = dto.OrderedQuantity;
        entity.UOM = dto.UOM;
        entity.UnitPrice = dto.UnitPrice;
        entity.POAmount = poAmount;
        entity.OrderDate = dto.OrderDate;
        entity.RequiredCompletion = dto.RequiredCompletion;
        entity.PlannedExport = dto.PlannedExport;
        entity.PlannedArrival = dto.PlannedArrival;
        entity.POStatus = dto.POStatus;
        entity.PurchaseOwner = dto.PurchaseOwner;
        entity.ApprovedBy = dto.ApprovedBy;
        entity.Remarks = dto.Remarks;
        entity.LastUpdated = DateTime.UtcNow;
        entity.UpdatedAt = DateTime.UtcNow;

        // Actualizar la relación muchos-a-muchos: reemplazar los FGPO asociados
        entity.FabricPOFgpos.Clear();
        foreach (var item in dto.FgpoItems)
        {
            entity.FabricPOFgpos.Add(new FabricPOFgpo
            {
                FabricPOId = entity.ID,
                FGPOId = item.FGPOId,
                Style = item.Style,
                Color = item.Color,
                AllocatedQuantity = item.AllocatedQuantity,
                LastUpdated = DateTime.UtcNow,
            });
        }

        _context.FabricPOs.Update(entity);
        await _context.SaveChangesAsync();

        return true;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var entity = await _context.FabricPOs
            .FirstOrDefaultAsync(p => p.ID == id && p.Active);

        if (entity is null)
        {
            return false;
        }

        // Soft Delete: se desactiva el registro en lugar de eliminarlo físicamente
        entity.Active = false;
        entity.UpdatedAt = DateTime.UtcNow;

        _context.FabricPOs.Update(entity);
        await _context.SaveChangesAsync();

        return true;
    }

    public async Task<PagedResultDto<FabricPODto>> GetPagedAsync(int page, int pageSize, string? search, string? sortBy, string? sortOrder, string? fgpo, string? supplier, string? fabricMill, string? fabricComponent, string? poStatus)
    {
        if (page < 1) page = 1;
        if (pageSize < 1) pageSize = 10;
        if (pageSize > 100) pageSize = 100;

        var query = _context.FabricPOs
            .Include(p => p.FabricPOFgpos)
                .ThenInclude(pf => pf.FGPO)
                    .ThenInclude(f => f!.Customer)
            .Where(p => p.Active);

        // Búsqueda general
        if (!string.IsNullOrWhiteSpace(search))
        {
            var searchTerm = search.Trim();
            query = query.Where(p =>
                p.FabricPONumber.Contains(searchTerm) ||
                (p.Supplier != null && p.Supplier.Contains(searchTerm)) ||
                (p.FabricMill != null && p.FabricMill.Contains(searchTerm)) ||
                (p.FabricComponent != null && p.FabricComponent.Contains(searchTerm)) ||
                (p.PurchaseOwner != null && p.PurchaseOwner.Contains(searchTerm)) ||
                p.FabricPOFgpos.Any(pf =>
                    (pf.Style != null && pf.Style.Contains(searchTerm)) ||
                    (pf.Color != null && pf.Color.Contains(searchTerm))));
        }

        // Filtros
        if (!string.IsNullOrWhiteSpace(fgpo))
        {
            query = query.Where(p => p.FabricPOFgpos.Any(pf => pf.FGPO != null && pf.FGPO.FGPONumber.Contains(fgpo.Trim())));
        }

        if (!string.IsNullOrWhiteSpace(supplier))
        {
            query = query.Where(p => p.Supplier != null && p.Supplier.Contains(supplier.Trim()));
        }

        if (!string.IsNullOrWhiteSpace(fabricMill))
        {
            query = query.Where(p => p.FabricMill != null && p.FabricMill.Contains(fabricMill.Trim()));
        }

        if (!string.IsNullOrWhiteSpace(fabricComponent))
        {
            query = query.Where(p => p.FabricComponent != null && p.FabricComponent.Contains(fabricComponent.Trim()));
        }

        if (!string.IsNullOrWhiteSpace(poStatus))
        {
            query = query.Where(p => p.POStatus == poStatus);
        }

        // Total de registros (sin ordenamiento para evitar errores de traducción SQL)
        var totalCount = await query.CountAsync();
        var totalPages = (int)Math.Ceiling(totalCount / (double)pageSize);

        // Ordenamiento
        var sortByLower = sortBy?.ToLowerInvariant();
        var sortOrderLower = sortOrder?.ToLowerInvariant();
        var descending = sortOrderLower == "desc";

        IQueryable<FabricPO> orderedQuery = (sortByLower, descending) switch
        {
            ("fabricponumber", false) => query.OrderBy(p => p.FabricPONumber),
            ("fabricponumber", true) => query.OrderByDescending(p => p.FabricPONumber),
            ("supplier", false) => query.OrderBy(p => p.Supplier),
            ("supplier", true) => query.OrderByDescending(p => p.Supplier),
            ("fabricmill", false) => query.OrderBy(p => p.FabricMill),
            ("fabricmill", true) => query.OrderByDescending(p => p.FabricMill),
            ("fabriccomponent", false) => query.OrderBy(p => p.FabricComponent),
            ("fabriccomponent", true) => query.OrderByDescending(p => p.FabricComponent),
            ("orderedquantity", false) => query.OrderBy(p => p.OrderedQuantity),
            ("orderedquantity", true) => query.OrderByDescending(p => p.OrderedQuantity),
            ("unitprice", false) => query.OrderBy(p => p.UnitPrice),
            ("unitprice", true) => query.OrderByDescending(p => p.UnitPrice),
            ("poamount", false) => query.OrderBy(p => p.POAmount),
            ("poamount", true) => query.OrderByDescending(p => p.POAmount),
            ("orderdate", false) => query.OrderBy(p => p.OrderDate),
            ("orderdate", true) => query.OrderByDescending(p => p.OrderDate),
            ("requiredcompletion", false) => query.OrderBy(p => p.RequiredCompletion),
            ("requiredcompletion", true) => query.OrderByDescending(p => p.RequiredCompletion),
            ("postatus", false) => query.OrderBy(p => p.POStatus),
            ("postatus", true) => query.OrderByDescending(p => p.POStatus),
            ("createdat", false) => query.OrderBy(p => p.CreatedAt),
            ("createdat", true) => query.OrderByDescending(p => p.CreatedAt),
            _ => query.OrderByDescending(p => p.CreatedAt),
        };

        var items = await orderedQuery
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return new PagedResultDto<FabricPODto>
        {
            Items = items.Select(ToDto),
            TotalCount = totalCount,
            Page = page,
            PageSize = pageSize,
            TotalPages = totalPages,
        };
    }

    private static void Validate(string fabricPONumber, List<FabricPOFgpoItemDto> fgpoItems, string? fabricComponent, string? uom, decimal orderedQuantity, decimal unitPrice, DateTime orderDate, DateTime requiredCompletion, string? poStatus)
    {
        if (string.IsNullOrWhiteSpace(fabricPONumber))
            throw new Exception("El Fabric PO Number es obligatorio.");

        if (fgpoItems == null || fgpoItems.Count == 0)
            throw new Exception("Debe seleccionar al menos un FGPO.");

        if (string.IsNullOrWhiteSpace(fabricComponent))
            throw new Exception("El Fabric Component es obligatorio.");

        if (!ValidFabricComponents.Contains(fabricComponent, StringComparer.OrdinalIgnoreCase))
            throw new Exception($"El Fabric Component '{fabricComponent}' no es válido.");

        if (string.IsNullOrWhiteSpace(uom))
            throw new Exception("El UOM es obligatorio.");

        if (!ValidUoms.Contains(uom, StringComparer.OrdinalIgnoreCase))
            throw new Exception($"El UOM '{uom}' no es válido.");

        if (orderedQuantity <= 0)
            throw new Exception("El Ordered Quantity debe ser mayor que 0.");

        if (unitPrice <= 0)
            throw new Exception("El Unit Price debe ser mayor que 0.");

        if (orderDate == default)
            throw new Exception("La Order Date es obligatoria.");

        if (requiredCompletion == default)
            throw new Exception("La Required Completion es obligatoria.");

        if (!string.IsNullOrWhiteSpace(poStatus) && !ValidPoStatuses.Contains(poStatus, StringComparer.OrdinalIgnoreCase))
            throw new Exception($"El PO Status '{poStatus}' no es válido.");
    }

    private static void ValidateAllocatedQuantity(List<FabricPOFgpoItemDto> fgpoItems, decimal orderedQuantity)
    {
        foreach (var item in fgpoItems)
        {
            if (item.AllocatedQuantity < 0)
                throw new Exception($"La Allocated Quantity para el FGPO {item.FGPOId} no puede ser negativa.");

            if (item.AllocatedQuantity == 0)
                throw new Exception($"La Allocated Quantity para el FGPO {item.FGPOId} debe ser mayor que 0.");
        }

        var duplicateIds = fgpoItems
            .GroupBy(i => i.FGPOId)
            .Where(g => g.Count() > 1)
            .Select(g => g.Key)
            .ToList();

        if (duplicateIds.Any())
        {
            throw new Exception(
                $"No se puede asignar el mismo FGPO más de una vez en la misma solicitud. FGPO(s) duplicado(s): {string.Join(", ", duplicateIds)}.");
        }

        var totalAllocated = fgpoItems.Sum(i => i.AllocatedQuantity);
        if (totalAllocated > orderedQuantity)
        {
            throw new Exception(
                $"La cantidad total asignada ({totalAllocated}) excede la cantidad ordenada ({orderedQuantity}). Excedente: {totalAllocated - orderedQuantity}.");
        }
    }

    private static FabricPODto ToDto(FabricPO item)
    {
        return new FabricPODto
        {
            ID = item.ID,
            FabricPONumber = item.FabricPONumber,
            Fgpos = item.FabricPOFgpos
                .Where(pf => pf.FGPO != null)
                .Select(pf => new FabricPOFgpoDto
                {
                    FGPOId = pf.FGPOId,
                    FGPONumber = pf.FGPO!.FGPONumber,
                    CustomerName = pf.FGPO.Customer?.Name ?? string.Empty,
                    Style = pf.Style,
                    Color = pf.Color,
                    AllocatedQuantity = pf.AllocatedQuantity,
                })
                .ToList(),
            Supplier = item.Supplier,
            FabricMill = item.FabricMill,
            FabricComponent = item.FabricComponent,
            OrderedQuantity = item.OrderedQuantity,
            UOM = item.UOM,
            UnitPrice = item.UnitPrice,
            POAmount = item.POAmount,
            OrderDate = item.OrderDate,
            RequiredCompletion = item.RequiredCompletion,
            PlannedExport = item.PlannedExport,
            PlannedArrival = item.PlannedArrival,
            POStatus = item.POStatus,
            PurchaseOwner = item.PurchaseOwner,
            ApprovedBy = item.ApprovedBy,
            LastUpdated = item.LastUpdated,
            Remarks = item.Remarks,
            Active = item.Active,
            CreatedAt = item.CreatedAt,
            UpdatedAt = item.UpdatedAt,
        };
    }
}
