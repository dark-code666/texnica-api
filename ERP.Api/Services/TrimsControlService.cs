using Microsoft.EntityFrameworkCore;
using ERP.Api.Data;
using ERP.Api.Dtos;
using ERP.Api.Domain;
using ERP.Api.Interfaces;

namespace ERP.Api.Services;

public class TrimsControlService : ITrimsControlService
{
    private readonly ErpDbContext _context;

    public TrimsControlService(ErpDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<TrimsControlDto>> GetAllAsync()
    {
        var items = await _context.TrimsControls
            .Include(t => t.FGPO).ThenInclude(f => f!.Customer)
            .Include(t => t.Supplier)
            .Where(t => t.Active)
            .OrderByDescending(t => t.CreatedAt)
            .ToListAsync();
        return items.Select(ToDto);
    }

    public async Task<TrimsControlDto?> GetByIdAsync(int id)
    {
        var item = await _context.TrimsControls
            .Include(t => t.FGPO).ThenInclude(f => f!.Customer)
            .Include(t => t.Supplier)
            .FirstOrDefaultAsync(t => t.ID == id && t.Active);
        return item is null ? null : ToDto(item);
    }

    public async Task<IEnumerable<TrimsControlDto>> GetByFgpoAsync(int fgpoId)
    {
        var items = await _context.TrimsControls
            .Include(t => t.FGPO).ThenInclude(f => f!.Customer)
            .Include(t => t.Supplier)
            .Where(t => t.Active && t.FGPOId == fgpoId)
            .OrderBy(t => t.TrimType)
            .ToListAsync();
        return items.Select(ToDto);
    }

    public async Task<TrimsControlDto> CreateAsync(CreateTrimsControlDto dto)
    {
        Validate(dto);
        var fgpo = await _context.Fgpos.FirstOrDefaultAsync(f => f.ID == dto.FGPOId && f.Active)
            ?? throw new Exception("El FGPO seleccionado no es válido.");
        await ValidateSupplierAsync(dto.SupplierId);

        var entity = new TrimsControl
        {
            FGPOId = dto.FGPOId,
            TrimType = dto.TrimType,
            Description = dto.Description,
            SupplierId = dto.SupplierId,
            Uom = dto.Uom,
            ConsumptionPerGarment = dto.ConsumptionPerGarment,
            RequiredQty = dto.RequiredQty,
            OrderedQty = dto.OrderedQty,
            ReceivedQty = dto.ReceivedQty,
            ApprovedQty = dto.ApprovedQty,
            RejectedQty = dto.RejectedQty,
            ReservedQty = dto.ReservedQty,
            IssuedQty = dto.IssuedQty,
            Eta = dto.Eta,
            DevelopmentStatus = dto.DevelopmentStatus,
            ApprovalStatus = dto.ApprovalStatus,
            DataOwner = dto.DataOwner,
            Comments = dto.Comments,
            Active = true,
            CreatedAt = DateTime.UtcNow,
        };

        _context.TrimsControls.Add(entity);
        await _context.SaveChangesAsync();
        await _context.Entry(entity).Reference(t => t.FGPO).Query().Include(f => f!.Customer).LoadAsync();
        return ToDto(entity);
    }

    public async Task<bool> UpdateAsync(int id, UpdateTrimsControlDto dto)
    {
        var entity = await _context.TrimsControls.FirstOrDefaultAsync(t => t.ID == id && t.Active);
        if (entity is null)
            return false;

        Validate(dto);
        var fgpo = await _context.Fgpos.FirstOrDefaultAsync(f => f.ID == dto.FGPOId && f.Active)
            ?? throw new Exception("El FGPO seleccionado no es válido.");
        await ValidateSupplierAsync(dto.SupplierId);

        entity.FGPOId = dto.FGPOId;
        entity.TrimType = dto.TrimType;
        entity.Description = dto.Description;
        entity.SupplierId = dto.SupplierId;
        entity.Uom = dto.Uom;
        entity.ConsumptionPerGarment = dto.ConsumptionPerGarment;
        entity.RequiredQty = dto.RequiredQty;
        entity.OrderedQty = dto.OrderedQty;
        entity.ReceivedQty = dto.ReceivedQty;
        entity.ApprovedQty = dto.ApprovedQty;
        entity.RejectedQty = dto.RejectedQty;
        entity.ReservedQty = dto.ReservedQty;
        entity.IssuedQty = dto.IssuedQty;
        entity.Eta = dto.Eta;
        entity.DevelopmentStatus = dto.DevelopmentStatus;
        entity.ApprovalStatus = dto.ApprovalStatus;
        entity.DataOwner = dto.DataOwner;
        entity.Comments = dto.Comments;
        entity.UpdatedAt = DateTime.UtcNow;

        _context.TrimsControls.Update(entity);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var entity = await _context.TrimsControls.FirstOrDefaultAsync(t => t.ID == id && t.Active);
        if (entity is null)
            return false;

        entity.Active = false;
        entity.UpdatedAt = DateTime.UtcNow;
        _context.TrimsControls.Update(entity);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<PagedResultDto<TrimsControlDto>> GetPagedAsync(int page, int pageSize, string? search, string? sortBy, string? sortOrder, string? fgpo, string? status)
    {
        if (page < 1) page = 1;
        if (pageSize < 1) pageSize = 10;
        if (pageSize > 100) pageSize = 100;

        var query = _context.TrimsControls
            .Include(t => t.FGPO).ThenInclude(f => f!.Customer)
            .Include(t => t.Supplier)
            .Where(t => t.Active);

        if (!string.IsNullOrWhiteSpace(search))
        {
            var term = search.Trim();
            query = query.Where(t =>
                (t.TrimType != null && t.TrimType.Contains(term)) ||
                (t.Description != null && t.Description.Contains(term)) ||
                (t.Supplier != null && t.Supplier.Name.Contains(term)) ||
                (t.AvailabilityStatus != null && t.AvailabilityStatus.Contains(term)) ||
                (t.FGPO != null && t.FGPO.FGPONumber.Contains(term)));
        }

        if (!string.IsNullOrWhiteSpace(fgpo))
            query = query.Where(t => t.FGPO != null && t.FGPO.FGPONumber.Contains(fgpo.Trim()));

        if (!string.IsNullOrWhiteSpace(status))
            query = query.Where(t => t.AvailabilityStatus != null && t.AvailabilityStatus.Contains(status.Trim()));

        var totalCount = await query.CountAsync();
        var totalPages = (int)Math.Ceiling(totalCount / (double)pageSize);

        var sortByLower = sortBy?.ToLowerInvariant();
        var descending = sortOrder?.ToLowerInvariant() == "desc";

        IQueryable<TrimsControl> orderedQuery = (sortByLower, descending) switch
        {
            ("trmtype", false) => query.OrderBy(t => t.TrimType),
            ("trmtype", true) => query.OrderByDescending(t => t.TrimType),
            ("availabilitystatus", false) => query.OrderBy(t => t.AvailabilityStatus),
            ("availabilitystatus", true) => query.OrderByDescending(t => t.AvailabilityStatus),
            ("shortageqty", false) => query.OrderBy(t => t.ShortageQty),
            ("shortageqty", true) => query.OrderByDescending(t => t.ShortageQty),
            ("createdat", false) => query.OrderBy(t => t.CreatedAt),
            ("createdat", true) => query.OrderByDescending(t => t.CreatedAt),
            _ => query.OrderByDescending(t => t.CreatedAt),
        };

        var items = await orderedQuery
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return new PagedResultDto<TrimsControlDto>
        {
            Items = items.Select(ToDto),
            TotalCount = totalCount,
            Page = page,
            PageSize = pageSize,
            TotalPages = totalPages,
        };
    }

    private async Task ValidateSupplierAsync(int? supplierId)
    {
        if (supplierId.HasValue && await _context.Suppliers.FirstOrDefaultAsync(s => s.ID == supplierId.Value && s.Active) is null)
            throw new Exception("El Supplier seleccionado no es válido.");
    }

    private static void Validate(CreateTrimsControlDto dto)
    {
        if (dto.RequiredQty < 0 || dto.OrderedQty < 0 || dto.ReceivedQty < 0 || dto.ApprovedQty < 0 || dto.RejectedQty < 0 || dto.ReservedQty < 0 || dto.IssuedQty < 0)
            throw new Exception("Las cantidades no pueden ser negativas.");
    }

    private static void Validate(UpdateTrimsControlDto dto)
    {
        if (dto.RequiredQty < 0 || dto.OrderedQty < 0 || dto.ReceivedQty < 0 || dto.ApprovedQty < 0 || dto.RejectedQty < 0 || dto.ReservedQty < 0 || dto.IssuedQty < 0)
            throw new Exception("Las cantidades no pueden ser negativas.");
    }

    private static TrimsControlDto ToDto(TrimsControl item) => new()
    {
        ID = item.ID,
        FGPOId = item.FGPOId,
        FGPONumber = item.FGPO?.FGPONumber ?? string.Empty,
        CustomerName = item.FGPO?.Customer?.Name ?? string.Empty,
        Style = item.FGPO?.FgpoLines?.FirstOrDefault(l => l.Active)?.Style?.StyleCode,
        Color = item.FGPO?.FgpoLines?.FirstOrDefault(l => l.Active)?.Color?.ColorName,
        TrimType = item.TrimType,
        Description = item.Description,
        SupplierId = item.SupplierId,
        SupplierName = item.Supplier?.Name ?? string.Empty,
        Uom = item.Uom,
        ConsumptionPerGarment = item.ConsumptionPerGarment,
        RequiredQty = item.RequiredQty,
        OrderedQty = item.OrderedQty,
        ReceivedQty = item.ReceivedQty,
        ApprovedQty = item.ApprovedQty,
        RejectedQty = item.RejectedQty,
        ReservedQty = item.ReservedQty,
        IssuedQty = item.IssuedQty,
        AvailableQty = item.AvailableQty,
        ShortageQty = item.ShortageQty,
        AvailabilityStatus = item.AvailabilityStatus,
        Eta = item.Eta,
        DevelopmentStatus = item.DevelopmentStatus,
        ApprovalStatus = item.ApprovalStatus,
        DataOwner = item.DataOwner,
        Comments = item.Comments,
        Active = item.Active,
        CreatedAt = item.CreatedAt,
        UpdatedAt = item.UpdatedAt,
    };
}
