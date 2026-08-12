using Microsoft.EntityFrameworkCore;
using ERP.Api.Data;
using ERP.Api.Dtos;
using ERP.Api.Domain;
using ERP.Api.Interfaces;

namespace ERP.Api.Services;

public class FabricRequirementService : IFabricRequirementService
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

    public FabricRequirementService(ErpDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<FabricRequirementDto>> GetAllAsync()
    {
        var items = await _context.FabricRequirements
            .Include(f => f.FGPO)
                .ThenInclude(f => f!.Customer)
            .Include(f => f.Component)
            .Include(f => f.DataOwner)
            .Where(f => f.Active)
            .OrderByDescending(f => f.CreatedAt)
            .ToListAsync();

        return items.Select(ToDto);
    }

    public async Task<FabricRequirementDto?> GetByIdAsync(int id)
    {
        var item = await _context.FabricRequirements
            .Include(f => f.FGPO)
                .ThenInclude(f => f!.Customer)
            .Include(f => f.Component)
            .Include(f => f.DataOwner)
            .FirstOrDefaultAsync(f => f.ID == id && f.Active);

        return item is null ? null : ToDto(item);
    }

    public async Task<IEnumerable<FabricRequirementDto>> GetByFgpoAsync(int fgpoId)
    {
        var items = await _context.FabricRequirements
            .Include(f => f.FGPO)
                .ThenInclude(f => f!.Customer)
            .Include(f => f.Component)
            .Include(f => f.DataOwner)
            .Where(f => f.FGPOId == fgpoId && f.Active)
            .OrderBy(f => f.CreatedAt)
            .ToListAsync();

        return items.Select(ToDto);
    }

    public async Task<FabricRequirementDto> CreateAsync(CreateFabricRequirementDto dto)
    {
        Validate(dto.FGPOId, dto.UOM, dto.OrderQuantity, dto.ApprovedYield, dto.AllowancePercentage, dto.RequiredDate);

        // Validar que la FGPO exista y esté activa
        var fgpo = await _context.Fgpos
            .FirstOrDefaultAsync(f => f.ID == dto.FGPOId && f.Active);
        if (fgpo is null)
            throw new Exception("La FGPO seleccionada no es válida.");

        // Cálculos en el backend (consistente con la arquitectura del proyecto)
        var grossRequirement = dto.OrderQuantity * dto.ApprovedYield;
        var allowanceQty = grossRequirement * dto.AllowancePercentage;  
        var netPurchaseRequirement = Math.Max(grossRequirement + allowanceQty - dto.AvailableInventory, 0);

        var entity = new FabricRequirement
        {
            FGPOId = dto.FGPOId,
            Style = dto.Style,
            Color = dto.Color,
            ComponentId = dto.ComponentId,
            FabricDescription = dto.FabricDescription,
            Composition = dto.Composition,
            GSM = dto.GSM,
            RequiredWidth = dto.RequiredWidth,
            UOM = dto.UOM,
            OrderQuantity = dto.OrderQuantity,
            ApprovedYield = dto.ApprovedYield,
            GrossRequirement = grossRequirement,
            AllowancePercentage = dto.AllowancePercentage,
            AllowanceQty = allowanceQty,
            AvailableInventory = dto.AvailableInventory,
            NetPurchaseRequirement = netPurchaseRequirement,
            RequiredDate = dto.RequiredDate,
            Status = dto.Status,
            DataOwnerId = dto.DataOwnerId,
            Remarks = dto.Remarks,
            Active = true,
            CreatedAt = DateTime.UtcNow,
        };

        _context.FabricRequirements.Add(entity);
        await _context.SaveChangesAsync();

        // Cargar las relaciones para devolver los nombres
        await _context.Entry(entity).Reference(f => f.FGPO).LoadAsync();
        await _context.Entry(entity).Reference(f => f.FGPO).Query().Include(f => f!.Customer).LoadAsync();
        await _context.Entry(entity).Reference(f => f.Component).LoadAsync();
        await _context.Entry(entity).Reference(f => f.DataOwner).LoadAsync();

        return ToDto(entity);
    }

    public async Task<bool> UpdateAsync(int id, UpdateFabricRequirementDto dto)
    {
        var entity = await _context.FabricRequirements
            .FirstOrDefaultAsync(f => f.ID == id && f.Active);

        if (entity is null)
        {
            return false;
        }

        Validate(dto.FGPOId, dto.UOM, dto.OrderQuantity, dto.ApprovedYield, dto.AllowancePercentage, dto.RequiredDate);

        // Validar que la FGPO exista y esté activa
        var fgpo = await _context.Fgpos
            .FirstOrDefaultAsync(f => f.ID == dto.FGPOId && f.Active);
        if (fgpo is null)
            throw new Exception("La FGPO seleccionada no es válida.");

        // Cálculos en el backend
        var grossRequirement = dto.OrderQuantity * dto.ApprovedYield;
        var allowanceQty = grossRequirement * dto.AllowancePercentage / 100m;
        var netPurchaseRequirement = Math.Max(grossRequirement + allowanceQty - dto.AvailableInventory, 0);

        entity.FGPOId = dto.FGPOId;
        entity.Style = dto.Style;
        entity.Color = dto.Color;
        entity.ComponentId = dto.ComponentId;
        entity.FabricDescription = dto.FabricDescription;
        entity.Composition = dto.Composition;
        entity.GSM = dto.GSM;
        entity.RequiredWidth = dto.RequiredWidth;
        entity.UOM = dto.UOM;
        entity.OrderQuantity = dto.OrderQuantity;
        entity.ApprovedYield = dto.ApprovedYield;
        entity.GrossRequirement = grossRequirement;
        entity.AllowancePercentage = dto.AllowancePercentage;
        entity.AllowanceQty = allowanceQty;
        entity.AvailableInventory = dto.AvailableInventory;
        entity.NetPurchaseRequirement = netPurchaseRequirement;
        entity.RequiredDate = dto.RequiredDate;
        entity.Status = dto.Status;
        entity.DataOwnerId = dto.DataOwnerId;
        entity.Remarks = dto.Remarks;
        entity.UpdatedAt = DateTime.UtcNow;

        _context.FabricRequirements.Update(entity);
        await _context.SaveChangesAsync();

        return true;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var entity = await _context.FabricRequirements
            .FirstOrDefaultAsync(f => f.ID == id && f.Active);

        if (entity is null)
        {
            return false;
        }

        // Soft Delete: se desactiva el registro en lugar de eliminarlo físicamente
        entity.Active = false;
        entity.UpdatedAt = DateTime.UtcNow;

        _context.FabricRequirements.Update(entity);
        await _context.SaveChangesAsync();

        return true;
    }

    public async Task<PagedResultDto<FabricRequirementDto>> GetPagedAsync(int page, int pageSize, string? search, string? sortBy, string? sortOrder, string? fgpo, string? customer, string? style, string? fabricComponent, string? status)
    {
        if (page < 1) page = 1;
        if (pageSize < 1) pageSize = 10;
        if (pageSize > 100) pageSize = 100;

        var query = _context.FabricRequirements
            .Include(f => f.FGPO)
                .ThenInclude(f => f!.Customer)
            .Include(f => f.Component)
            .Include(f => f.DataOwner)
            .Where(f => f.Active);

        // Búsqueda general
        if (!string.IsNullOrWhiteSpace(search))
        {
            var searchTerm = search.Trim();
            query = query.Where(f =>
                (f.FGPO != null && f.FGPO.FGPONumber.Contains(searchTerm)) ||
                (f.FabricDescription != null && f.FabricDescription.Contains(searchTerm)) ||
                (f.Component != null && f.Component.ComponentCode.Contains(searchTerm)) ||
                (f.Color != null && f.Color.Contains(searchTerm)) ||
                (f.DataOwner != null && f.DataOwner.UserName.Contains(searchTerm)));
        }

        // Filtros
        if (!string.IsNullOrWhiteSpace(fgpo))
        {
            query = query.Where(f => f.FGPO != null && f.FGPO.FGPONumber.Contains(fgpo.Trim()));
        }

        if (!string.IsNullOrWhiteSpace(customer))
        {
            query = query.Where(f => f.FGPO != null && f.FGPO.Customer != null && f.FGPO.Customer.Name.Contains(customer.Trim()));
        }

        if (!string.IsNullOrWhiteSpace(style))
        {
            query = query.Where(f => f.Style != null && f.Style.Contains(style.Trim()));
        }

        if (!string.IsNullOrWhiteSpace(fabricComponent))
        {
            query = query.Where(f => f.Component != null && f.Component.ComponentCode.Contains(fabricComponent.Trim()));
        }

        if (!string.IsNullOrWhiteSpace(status))
        {
            query = query.Where(f => f.Status == status);
        }

        // Total de registros (sin ordenamiento para evitar errores de traducción SQL)
        var totalCount = await query.CountAsync();
        var totalPages = (int)Math.Ceiling(totalCount / (double)pageSize);

        // Ordenamiento
        var sortByLower = sortBy?.ToLowerInvariant();
        var sortOrderLower = sortOrder?.ToLowerInvariant();
        var descending = sortOrderLower == "desc";

        IQueryable<FabricRequirement> orderedQuery = (sortByLower, descending) switch
        {
            ("fgpo", false) => query.OrderBy(f => f.FGPO!.FGPONumber),
            ("fgpo", true) => query.OrderByDescending(f => f.FGPO!.FGPONumber),
            ("customer", false) => query.OrderBy(f => f.FGPO!.Customer!.Name),
            ("customer", true) => query.OrderByDescending(f => f.FGPO!.Customer!.Name),
            ("style", false) => query.OrderBy(f => f.Style),
            ("style", true) => query.OrderByDescending(f => f.Style),
            ("color", false) => query.OrderBy(f => f.Color),
            ("color", true) => query.OrderByDescending(f => f.Color),
            ("fabriccomponent", false) => query.OrderBy(f => f.Component != null ? f.Component.ComponentCode : null),
            ("fabriccomponent", true) => query.OrderByDescending(f => f.Component != null ? f.Component.ComponentCode : null),
            ("orderquantity", false) => query.OrderBy(f => f.OrderQuantity),
            ("orderquantity", true) => query.OrderByDescending(f => f.OrderQuantity),
            ("grossrequirement", false) => query.OrderBy(f => f.GrossRequirement),
            ("grossrequirement", true) => query.OrderByDescending(f => f.GrossRequirement),
            ("netpurchaserequirement", false) => query.OrderBy(f => f.NetPurchaseRequirement),
            ("netpurchaserequirement", true) => query.OrderByDescending(f => f.NetPurchaseRequirement),
            ("requireddate", false) => query.OrderBy(f => f.RequiredDate),
            ("requireddate", true) => query.OrderByDescending(f => f.RequiredDate),
            ("status", false) => query.OrderBy(f => f.Status),
            ("status", true) => query.OrderByDescending(f => f.Status),
            ("createdat", false) => query.OrderBy(f => f.CreatedAt),
            ("createdat", true) => query.OrderByDescending(f => f.CreatedAt),
            _ => query.OrderByDescending(f => f.CreatedAt),
        };

        var items = await orderedQuery
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return new PagedResultDto<FabricRequirementDto>
        {
            Items = items.Select(ToDto),
            TotalCount = totalCount,
            Page = page,
            PageSize = pageSize,
            TotalPages = totalPages,
        };
    }

    private static void Validate(int fgpoId, string? uom, decimal orderQuantity, decimal approvedYield, decimal allowancePercentage, DateTime requiredDate)
    {
        if (fgpoId <= 0)
            throw new Exception("La FGPO es obligatoria.");

        if (string.IsNullOrWhiteSpace(uom))
            throw new Exception("El UOM es obligatorio.");

        if (!ValidUoms.Contains(uom, StringComparer.OrdinalIgnoreCase))
            throw new Exception($"El UOM '{uom}' no es válido.");

        if (orderQuantity <= 0)
            throw new Exception("El Order Quantity debe ser mayor que 0.");

        if (approvedYield <= 0)
            throw new Exception("El Approved Yield debe ser mayor que 0.");

        if (allowancePercentage < 0 || allowancePercentage > 100)
            throw new Exception("El Allowance % debe estar entre 0 y 100.");

        if (requiredDate == default)
            throw new Exception("La Required Date es obligatoria.");
    }

    private static FabricRequirementDto ToDto(FabricRequirement item)
    {
        return new FabricRequirementDto
        {
            ID = item.ID,
            FGPOId = item.FGPOId,
            FGPONumber = item.FGPO?.FGPONumber ?? string.Empty,
            CustomerName = item.FGPO?.Customer?.Name ?? string.Empty,
            Style = item.Style,
            Color = item.Color,
            ComponentId = item.ComponentId,
            ComponentCode = item.Component?.ComponentCode,
            FabricDescription = item.FabricDescription,
            Composition = item.Composition,
            GSM = item.GSM,
            RequiredWidth = item.RequiredWidth,
            UOM = item.UOM,
            OrderQuantity = item.OrderQuantity,
            ApprovedYield = item.ApprovedYield,
            GrossRequirement = item.GrossRequirement,
            AllowancePercentage = item.AllowancePercentage,
            AllowanceQty = item.AllowanceQty,
            AvailableInventory = item.AvailableInventory,
            NetPurchaseRequirement = item.NetPurchaseRequirement,
            RequiredDate = item.RequiredDate,
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
