using Microsoft.EntityFrameworkCore;
using ERP.Api.Data;
using ERP.Api.Dtos;
using ERP.Api.Domain;
using ERP.Api.Interfaces;

namespace ERP.Api.Services;

public class FabricReservationService : IFabricReservationService
{
    private readonly ErpDbContext _context;

    public FabricReservationService(ErpDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<FabricReservationDto>> GetAllAsync()
    {
        var items = await _context.FabricReservations
            .Include(r => r.FabricPO)
                .ThenInclude(po => po!.Component)
            .Include(r => r.FGPO).ThenInclude(f => f!.Customer)
            .Include(r => r.Lot)
            .Include(r => r.ReservedBy)
            .Include(r => r.ApprovedBy)
            .Where(r => r.Active)
            .OrderByDescending(r => r.ReservationDate)
            .ToListAsync();
        return items.Select(ToDto);
    }

    public async Task<FabricReservationDto?> GetByIdAsync(int id)
    {
        var item = await _context.FabricReservations
            .Include(r => r.FabricPO)
                .ThenInclude(po => po!.Component)
            .Include(r => r.FGPO).ThenInclude(f => f!.Customer)
            .Include(r => r.Lot)
            .Include(r => r.ReservedBy)
            .Include(r => r.ApprovedBy)
            .FirstOrDefaultAsync(r => r.ID == id && r.Active);
        return item is null ? null : ToDto(item);
    }

    public async Task<IEnumerable<FabricReservationDto>> GetByFgpoAsync(int fgpoId)
    {
        var items = await _context.FabricReservations
            .Include(r => r.FabricPO)
                .ThenInclude(po => po!.Component)
            .Include(r => r.FGPO).ThenInclude(f => f!.Customer)
            .Include(r => r.Lot)
            .Include(r => r.ReservedBy)
            .Include(r => r.ApprovedBy)
            .Where(r => r.Active && r.FGPOId == fgpoId)
            .OrderByDescending(r => r.ReservationDate)
            .ToListAsync();
        return items.Select(ToDto);
    }

    public async Task<FabricReservationDto> CreateAsync(CreateFabricReservationDto dto)
    {
        Validate(dto);
        var fgpo = await _context.Fgpos.FirstOrDefaultAsync(f => f.ID == dto.FGPOId && f.Active)
            ?? throw new Exception("El FGPO seleccionado no es válido.");
        var fabricPo = await _context.FabricPOs.FirstOrDefaultAsync(p => p.ID == dto.FabricPOId && p.Active)
            ?? throw new Exception("El Fabric PO seleccionado no es válido.");
        await ValidateLotAsync(dto.LotId);

        var entity = new FabricReservation
        {
            ReservationDate = dto.ReservationDate,
            FabricPOId = dto.FabricPOId,
            FGPOId = dto.FGPOId,
            LotId = dto.LotId,
            ReservedQuantity = dto.ReservedQuantity,
            ReleasedQuantity = dto.ReleasedQuantity,
            Status = dto.Status,
            ReservedByUserId = dto.ReservedByUserId,
            ApprovedByUserId = dto.ApprovedByUserId,
            LastUpdated = dto.LastUpdated ?? DateTime.UtcNow,
            Comments = dto.Comments,
            Active = true,
            CreatedAt = DateTime.UtcNow,
        };

        _context.FabricReservations.Add(entity);
        await _context.SaveChangesAsync();
        await _context.Entry(entity).Reference(r => r.FabricPO).Query()
            .Include(po => po!.Component).LoadAsync();
        await _context.Entry(entity).Reference(r => r.FGPO).Query().Include(f => f!.Customer).LoadAsync();
        await _context.Entry(entity).Reference(r => r.Lot).LoadAsync();
        await _context.Entry(entity).Reference(r => r.ReservedBy).LoadAsync();
        await _context.Entry(entity).Reference(r => r.ApprovedBy).LoadAsync();
        return ToDto(entity);
    }

    public async Task<bool> UpdateAsync(int id, UpdateFabricReservationDto dto)
    {
        var entity = await _context.FabricReservations.FirstOrDefaultAsync(r => r.ID == id && r.Active);
        if (entity is null)
            return false;

        Validate(dto);
        var fgpo = await _context.Fgpos.FirstOrDefaultAsync(f => f.ID == dto.FGPOId && f.Active)
            ?? throw new Exception("El FGPO seleccionado no es válido.");
        var fabricPo = await _context.FabricPOs.FirstOrDefaultAsync(p => p.ID == dto.FabricPOId && p.Active)
            ?? throw new Exception("El Fabric PO seleccionado no es válido.");
        await ValidateLotAsync(dto.LotId);

        entity.ReservationDate = dto.ReservationDate;
        entity.FabricPOId = dto.FabricPOId;
        entity.FGPOId = dto.FGPOId;
        entity.LotId = dto.LotId;
        entity.ReservedQuantity = dto.ReservedQuantity;
        entity.ReleasedQuantity = dto.ReleasedQuantity;
        entity.Status = dto.Status;
        entity.ReservedByUserId = dto.ReservedByUserId;
        entity.ApprovedByUserId = dto.ApprovedByUserId;
        entity.LastUpdated = dto.LastUpdated ?? DateTime.UtcNow;
        entity.Comments = dto.Comments;
        entity.UpdatedAt = DateTime.UtcNow;

        _context.FabricReservations.Update(entity);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var entity = await _context.FabricReservations.FirstOrDefaultAsync(r => r.ID == id && r.Active);
        if (entity is null)
            return false;

        entity.Active = false;
        entity.UpdatedAt = DateTime.UtcNow;
        _context.FabricReservations.Update(entity);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<PagedResultDto<FabricReservationDto>> GetPagedAsync(int page, int pageSize, string? search, string? sortBy, string? sortOrder, string? fgpo, string? status)
    {
        if (page < 1) page = 1;
        if (pageSize < 1) pageSize = 10;
        if (pageSize > 100) pageSize = 100;

        var query = _context.FabricReservations
            .Include(r => r.FabricPO)
                .ThenInclude(po => po!.Component)
            .Include(r => r.FGPO).ThenInclude(f => f!.Customer)
            .Include(r => r.Lot)
            .Include(r => r.ReservedBy)
            .Include(r => r.ApprovedBy)
            .Where(r => r.Active);

        if (!string.IsNullOrWhiteSpace(search))
        {
            var term = search.Trim();
            query = query.Where(r =>
                (r.FabricPO != null && r.FabricPO.Component != null && r.FabricPO.Component.ComponentCode.Contains(term)) ||
                (r.Lot != null && r.Lot.LotNumber.Contains(term)) ||
                (r.Status != null && r.Status.Contains(term)) ||
                (r.ReservedBy != null && r.ReservedBy.UserName.Contains(term)) ||
                (r.FabricPO != null && r.FabricPO.FabricPONumber.Contains(term)) ||
                (r.FGPO != null && r.FGPO.FGPONumber.Contains(term)));
        }

        if (!string.IsNullOrWhiteSpace(fgpo))
            query = query.Where(r => r.FGPO != null && r.FGPO.FGPONumber.Contains(fgpo.Trim()));

        if (!string.IsNullOrWhiteSpace(status))
            query = query.Where(r => r.Status != null && r.Status.Contains(status.Trim()));

        var totalCount = await query.CountAsync();
        var totalPages = (int)Math.Ceiling(totalCount / (double)pageSize);

        var sortByLower = sortBy?.ToLowerInvariant();
        var descending = sortOrder?.ToLowerInvariant() == "desc";

        IQueryable<FabricReservation> orderedQuery = (sortByLower, descending) switch
        {
            ("reservationdate", false) => query.OrderBy(r => r.ReservationDate),
            ("reservationdate", true) => query.OrderByDescending(r => r.ReservationDate),
            ("reservedquantity", false) => query.OrderBy(r => r.ReservedQuantity),
            ("reservedquantity", true) => query.OrderByDescending(r => r.ReservedQuantity),
            ("remainingreservation", false) => query.OrderBy(r => r.RemainingReservation),
            ("remainingreservation", true) => query.OrderByDescending(r => r.RemainingReservation),
            ("status", false) => query.OrderBy(r => r.Status),
            ("status", true) => query.OrderByDescending(r => r.Status),
            ("createdat", false) => query.OrderBy(r => r.CreatedAt),
            ("createdat", true) => query.OrderByDescending(r => r.CreatedAt),
            _ => query.OrderByDescending(r => r.ReservationDate),
        };

        var items = await orderedQuery
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return new PagedResultDto<FabricReservationDto>
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

    private static void Validate(CreateFabricReservationDto dto)
    {
        if (dto.ReservationDate == default)
            throw new Exception("La Reservation Date es obligatoria.");
        if (dto.ReservedQuantity < 0 || dto.ReleasedQuantity < 0)
            throw new Exception("Las cantidades no pueden ser negativas.");
    }

    private static void Validate(UpdateFabricReservationDto dto)
    {
        if (dto.ReservationDate == default)
            throw new Exception("La Reservation Date es obligatoria.");
        if (dto.ReservedQuantity < 0 || dto.ReleasedQuantity < 0)
            throw new Exception("Las cantidades no pueden ser negativas.");
    }

    private static FabricReservationDto ToDto(FabricReservation item) => new()
    {
        ID = item.ID,
        ReservationDate = item.ReservationDate,
        FabricPOId = item.FabricPOId,
        FabricPONumber = item.FabricPO?.FabricPONumber ?? string.Empty,
        FGPOId = item.FGPOId,
        FGPONumber = item.FGPO?.FGPONumber ?? string.Empty,
        CustomerName = item.FGPO?.Customer?.Name ?? string.Empty,
        ComponentId = item.FabricPO?.ComponentId,
        ComponentCode = item.FabricPO?.Component?.ComponentCode,
        LotId = item.LotId,
        LotNumber = item.Lot?.LotNumber ?? string.Empty,
        ReservedQuantity = item.ReservedQuantity,
        UOM = item.FabricPO?.UOM,
        ReleasedQuantity = item.ReleasedQuantity,
        RemainingReservation = item.RemainingReservation,
        Status = item.Status,
        ReservedByUserId = item.ReservedByUserId,
        ReservedByName = item.ReservedBy?.UserName,
        ApprovedByUserId = item.ApprovedByUserId,
        ApprovedByName = item.ApprovedBy?.UserName,
        LastUpdated = item.LastUpdated,
        Comments = item.Comments,
        Active = item.Active,
        CreatedAt = item.CreatedAt,
        UpdatedAt = item.UpdatedAt,
    };
}
