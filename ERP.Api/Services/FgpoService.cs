using Microsoft.EntityFrameworkCore;
using ERP.Api.Data;
using ERP.Api.Dtos;
using ERP.Api.Domain;
using ERP.Api.Interfaces;

namespace ERP.Api.Services;

public class FgpoService : IFgpoService
{
    private readonly ErpDbContext _context;

    private static readonly string[] ValidStatuses =
    {
        "Not Started", "Pending", "In Progress", "Partially Completed", "Completed",
        "Approved", "Conditionally Approved", "Rejected", "On Hold", "Closed",
        "Cancelled", "FGPO Pending"
    };

    public FgpoService(ErpDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<FgpoDto>> GetAllAsync()
    {
        var fgpos = await _context.Fgpos
            .Include(f => f.Customer)
            .Where(f => f.Active)
            .OrderByDescending(f => f.CreatedAt)
            .ToListAsync();

        return fgpos.Select(ToDto);
    }

    public async Task<FgpoDto?> GetByIdAsync(int id)
    {
        var fgpo = await _context.Fgpos
            .Include(f => f.Customer)
            .FirstOrDefaultAsync(f => f.ID == id && f.Active);

        return fgpo is null ? null : ToDto(fgpo);
    }

    public async Task<FgpoDto> CreateAsync(CreateFgpoDto dto)
    {
        Validate(dto.FGPONumber, dto.TemporaryNumber, dto.Status, dto.CustomerId, dto.DeliveryDate, dto.OrderQuantity);

        // Validar que el Status sea uno de la lista fija
        ValidateStatus(dto.Status);

        // Validar unicidad del FGPO Number (obligatorio salvo status "FGPO Pending")
        if (!IsPending(dto.Status))
        {
            var exists = await _context.Fgpos.AnyAsync(f => f.FGPONumber == dto.FGPONumber);
            if (exists)
                throw new Exception("El FGPO Number ya existe.");
        }
        else
        {
            // Con status "FGPO Pending" el Temporary Order ID debe ser único
            if (!string.IsNullOrWhiteSpace(dto.TemporaryNumber))
            {
                var tempExists = await _context.Fgpos.AnyAsync(f => f.TemporaryNumber == dto.TemporaryNumber);
                if (tempExists)
                    throw new Exception("El Temporary Order ID ya existe.");
            }
        }

        // Validar que el Customer exista y esté activo
        var customer = await _context.Customers
            .FirstOrDefaultAsync(c => c.ID == dto.CustomerId && c.Active);
        if (customer is null)
            throw new Exception("El Customer seleccionado no es válido.");

        var entity = new Fgpo
        {
            FGPONumber = dto.FGPONumber ?? string.Empty,
            TemporaryNumber = dto.TemporaryNumber,
            Status = dto.Status,
            CustomerId = dto.CustomerId,
            Style = dto.Style,
            Color = dto.Color,
            OrderQuantity = dto.OrderQuantity,
            DeliveryDate = dto.DeliveryDate,
            InTransitQty = dto.InTransitQty,
            ReceivedQty = dto.ReceivedQty,
            TotalShippedQty = dto.TotalShippedQty,
            ProducedQty = dto.ProducedQty,
            DataOwner = dto.DataOwner,
            Remarks = dto.Remarks,
            Active = true,
            CreatedAt = DateTime.UtcNow,
        };

        ApplyCalculations(entity);

        _context.Fgpos.Add(entity);
        await _context.SaveChangesAsync();

        // Cargar las relaciones para devolver los nombres
        await _context.Entry(entity).Reference(f => f.Customer).LoadAsync();

        return ToDto(entity);
    }

    public async Task<bool> UpdateAsync(int id, UpdateFgpoDto dto)
    {
        var entity = await _context.Fgpos
            .FirstOrDefaultAsync(f => f.ID == id && f.Active);

        if (entity is null)
        {
            return false;
        }

        Validate(dto.FGPONumber, dto.TemporaryNumber, dto.Status, dto.CustomerId, dto.DeliveryDate, dto.OrderQuantity);
        ValidateStatus(dto.Status);

        // Validar unicidad del FGPO Number (excluyendo el registro actual)
        if (!IsPending(dto.Status))
        {
            var exists = await _context.Fgpos.AnyAsync(f => f.FGPONumber == dto.FGPONumber && f.ID != id);
            if (exists)
                throw new Exception("El FGPO Number ya existe.");
        }
        else
        {
            if (!string.IsNullOrWhiteSpace(dto.TemporaryNumber))
            {
                var tempExists = await _context.Fgpos.AnyAsync(f => f.TemporaryNumber == dto.TemporaryNumber && f.ID != id);
                if (tempExists)
                    throw new Exception("El Temporary Order ID ya existe.");
            }
        }

        // Validar que el Customer exista y esté activo
        var customer = await _context.Customers
            .FirstOrDefaultAsync(c => c.ID == dto.CustomerId && c.Active);
        if (customer is null)
            throw new Exception("El Customer seleccionado no es válido.");

        entity.FGPONumber = dto.FGPONumber ?? string.Empty;
        entity.TemporaryNumber = dto.TemporaryNumber;
        entity.Status = dto.Status;
        entity.CustomerId = dto.CustomerId;
        entity.Style = dto.Style;
        entity.Color = dto.Color;
        entity.OrderQuantity = dto.OrderQuantity;
        entity.DeliveryDate = dto.DeliveryDate;
        entity.InTransitQty = dto.InTransitQty;
        entity.ReceivedQty = dto.ReceivedQty;
        entity.TotalShippedQty = dto.TotalShippedQty;
        entity.ProducedQty = dto.ProducedQty;
        entity.DataOwner = dto.DataOwner;
        entity.Remarks = dto.Remarks;
        entity.UpdatedAt = DateTime.UtcNow;

        ApplyCalculations(entity);

        _context.Fgpos.Update(entity);
        await _context.SaveChangesAsync();

        return true;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var entity = await _context.Fgpos
            .FirstOrDefaultAsync(f => f.ID == id && f.Active);

        if (entity is null)
        {
            return false;
        }

        // Soft Delete: se desactiva el registro en lugar de eliminarlo físicamente
        entity.Active = false;
        entity.UpdatedAt = DateTime.UtcNow;

        _context.Fgpos.Update(entity);
        await _context.SaveChangesAsync();

        return true;
    }

    public async Task<IEnumerable<FgpoDto>> SearchAsync(string? term)
    {
        var query = _context.Fgpos
            .Include(f => f.Customer)
            .Where(f => f.Active);

        if (!string.IsNullOrWhiteSpace(term))
        {
            var searchTerm = term.Trim();
            query = query.Where(f =>
                f.FGPONumber.Contains(searchTerm) ||
                (f.TemporaryNumber != null && f.TemporaryNumber.Contains(searchTerm)) ||
                (f.Customer != null && f.Customer.Name.Contains(searchTerm)) ||
                (f.Style != null && f.Style.Contains(searchTerm)) ||
                (f.Color != null && f.Color.Contains(searchTerm)) ||
                (f.DataOwner != null && f.DataOwner.Contains(searchTerm)));
        }

        var fgpos = await query
            .OrderByDescending(f => f.CreatedAt)
            .ToListAsync();

        return fgpos.Select(ToDto);
    }

    public async Task<PagedResultDto<FgpoDto>> GetPagedAsync(int page, int pageSize, string? search, string? sortBy, string? sortOrder, string? status, string? customer)
    {
        if (page < 1) page = 1;
        if (pageSize < 1) pageSize = 10;
        if (pageSize > 100) pageSize = 100;

        var query = _context.Fgpos
            .Include(f => f.Customer)
            .Where(f => f.Active);

        // Búsqueda general
        if (!string.IsNullOrWhiteSpace(search))
        {
            var searchTerm = search.Trim();
            query = query.Where(f =>
                f.FGPONumber.Contains(searchTerm) ||
                (f.TemporaryNumber != null && f.TemporaryNumber.Contains(searchTerm)) ||
                (f.Customer != null && f.Customer.Name.Contains(searchTerm)) ||
                (f.Style != null && f.Style.Contains(searchTerm)) ||
                (f.Color != null && f.Color.Contains(searchTerm)) ||
                (f.DataOwner != null && f.DataOwner.Contains(searchTerm)));
        }

        // Filtros
        if (!string.IsNullOrWhiteSpace(status))
        {
            query = query.Where(f => f.Status == status);
        }

        if (!string.IsNullOrWhiteSpace(customer))
        {
            query = query.Where(f => f.Customer != null && f.Customer.Name.Contains(customer.Trim()));
        }

        // Total de registros (sin ordenamiento para evitar errores de traducción SQL)
        var totalCount = await query.CountAsync();
        var totalPages = (int)Math.Ceiling(totalCount / (double)pageSize);

        // Ordenamiento
        var sortByLower = sortBy?.ToLowerInvariant();
        var sortOrderLower = sortOrder?.ToLowerInvariant();
        var descending = sortOrderLower == "desc";

        IQueryable<Fgpo> orderedQuery = (sortByLower, descending) switch
        {
            ("fgponumber", false) => query.OrderBy(f => f.FGPONumber),
            ("fgponumber", true) => query.OrderByDescending(f => f.FGPONumber),
            ("customer", false) => query.OrderBy(f => f.Customer!.Name),
            ("customer", true) => query.OrderByDescending(f => f.Customer!.Name),
            ("style", false) => query.OrderBy(f => f.Style),
            ("style", true) => query.OrderByDescending(f => f.Style),
            ("color", false) => query.OrderBy(f => f.Color),
            ("color", true) => query.OrderByDescending(f => f.Color),
            ("deliverydate", false) => query.OrderBy(f => f.DeliveryDate),
            ("deliverydate", true) => query.OrderByDescending(f => f.DeliveryDate),
            ("orderquantity", false) => query.OrderBy(f => f.OrderQuantity),
            ("orderquantity", true) => query.OrderByDescending(f => f.OrderQuantity),
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

        return new PagedResultDto<FgpoDto>
        {
            Items = items.Select(ToDto),
            TotalCount = totalCount,
            Page = page,
            PageSize = pageSize,
            TotalPages = totalPages,
        };
    }

    private static bool IsPending(string? status) =>
        string.Equals(status, "FGPO Pending", StringComparison.OrdinalIgnoreCase);

    private static void ValidateStatus(string? status)
    {
        if (string.IsNullOrWhiteSpace(status))
            throw new Exception("El FGPO Status es obligatorio.");

        if (!ValidStatuses.Contains(status, StringComparer.OrdinalIgnoreCase))
            throw new Exception($"El FGPO Status '{status}' no es válido.");
    }

    private static void Validate(string? fgpoNumber, string? temporaryNumber, string? status, int customerId, DateTime deliveryDate, int orderQuantity)
    {
        // Si el status es "FGPO Pending", el FGPO Number puede estar vacío y se usa el Temporary Order ID
        if (IsPending(status))
        {
            if (string.IsNullOrWhiteSpace(temporaryNumber))
                throw new Exception("Con status 'FGPO Pending' el Temporary Order ID es obligatorio.");
        }
        else
        {
            if (string.IsNullOrWhiteSpace(fgpoNumber))
                throw new Exception("El FGPO Number es obligatorio.");
        }

        if (customerId <= 0)
            throw new Exception("El Customer es obligatorio.");

        if (deliveryDate == default || deliveryDate < DateTime.UtcNow.Date)
            throw new Exception("La DeliveryDate debe ser una fecha válida y no puede ser anterior a hoy.");

        if (orderQuantity <= 0)
            throw new Exception("El OrderQuantity debe ser mayor que 0.");
    }

    private static void ApplyCalculations(Fgpo fgpo)
    {
        // Shipment Variance = Total Shipped Qty − Order Quantity
        fgpo.ShipmentVariance = fgpo.TotalShippedQty - fgpo.OrderQuantity;

        // Pending to Ship = MAX(Order Quantity − Total Shipped Qty, 0)
        fgpo.PendingToShip = Math.Max(fgpo.OrderQuantity - fgpo.TotalShippedQty, 0);

        // Over-shipment Qty = MAX(Total Shipped Qty − Order Quantity, 0)
        fgpo.OvershipmentQty = Math.Max(fgpo.TotalShippedQty - fgpo.OrderQuantity, 0);

        // Production Variance = Produced Qty − Order Quantity
        fgpo.ProductionVariance = fgpo.ProducedQty - fgpo.OrderQuantity;

        // Pending Production = MAX(Order Quantity − Produced Qty, 0)
        fgpo.PendingProduction = Math.Max(fgpo.OrderQuantity - fgpo.ProducedQty, 0);

        // Overproduction Qty = MAX(Produced Qty − Order Quantity, 0)
        fgpo.OverproductionQty = Math.Max(fgpo.ProducedQty - fgpo.OrderQuantity, 0);
    }

    private static FgpoDto ToDto(Fgpo fgpo)
    {
        return new FgpoDto
        {
            ID = fgpo.ID,
            FGPONumber = fgpo.FGPONumber,
            TemporaryNumber = fgpo.TemporaryNumber,
            Status = fgpo.Status,
            CustomerId = fgpo.CustomerId,
            CustomerName = fgpo.Customer?.Name ?? string.Empty,
            Style = fgpo.Style,
            Color = fgpo.Color,
            OrderQuantity = fgpo.OrderQuantity,
            DeliveryDate = fgpo.DeliveryDate,
            InTransitQty = fgpo.InTransitQty,
            ReceivedQty = fgpo.ReceivedQty,
            TotalShippedQty = fgpo.TotalShippedQty,
            ShipmentVariance = fgpo.ShipmentVariance,
            PendingToShip = fgpo.PendingToShip,
            OvershipmentQty = fgpo.OvershipmentQty,
            ProducedQty = fgpo.ProducedQty,
            ProductionVariance = fgpo.ProductionVariance,
            PendingProduction = fgpo.PendingProduction,
            OverproductionQty = fgpo.OverproductionQty,
            DataOwner = fgpo.DataOwner,
            Remarks = fgpo.Remarks,
            Active = fgpo.Active,
            CreatedAt = fgpo.CreatedAt,
            UpdatedAt = fgpo.UpdatedAt,
        };
    }
}
