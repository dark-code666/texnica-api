using Microsoft.EntityFrameworkCore;
using ERP.Api.Data;
using ERP.Api.Dtos;
using ERP.Api.Domain;
using ERP.Api.Interfaces;

namespace ERP.Api.Services;

public class FourPointService : IFourPointService
{
    private readonly ErpDbContext _context;

    public FourPointService(ErpDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<FourPointDto>> GetAllAsync()
    {
        var items = await _context.FourPointInspections
            .Include(i => i.Receiving)
            .Include(i => i.FabricPO)
            .Include(i => i.FGPO)
                .ThenInclude(f => f!.Customer)
            .Where(i => i.Active)
            .OrderByDescending(i => i.InspectionDate)
            .ToListAsync();

        return items.Select(ToDto);
    }

    public async Task<FourPointDto?> GetByIdAsync(int id)
    {
        var item = await _context.FourPointInspections
            .Include(i => i.Receiving)
            .Include(i => i.FabricPO)
            .Include(i => i.FGPO)
                .ThenInclude(f => f!.Customer)
            .FirstOrDefaultAsync(i => i.ID == id && i.Active);

        return item is null ? null : ToDto(item);
    }

    public async Task<IEnumerable<FourPointDto>> GetByFabricPOAsync(int fabricPOId)
    {
        var items = await _context.FourPointInspections
            .Include(i => i.Receiving)
            .Include(i => i.FabricPO)
            .Include(i => i.FGPO)
                .ThenInclude(f => f!.Customer)
            .Where(i => i.Active && i.FabricPOId == fabricPOId)
            .OrderByDescending(i => i.InspectionDate)
            .ToListAsync();

        return items.Select(ToDto);
    }

    public async Task<IEnumerable<FourPointDto>> GetByFgpoAsync(int fgpoId)
    {
        var items = await _context.FourPointInspections
            .Include(i => i.Receiving)
            .Include(i => i.FabricPO)
            .Include(i => i.FGPO)
                .ThenInclude(f => f!.Customer)
            .Where(i => i.Active && i.FGPOId == fgpoId)
            .OrderByDescending(i => i.InspectionDate)
            .ToListAsync();

        return items.Select(ToDto);
    }

    public async Task<IEnumerable<FourPointDto>> GetByReceivingAsync(int receivingId)
    {
        var items = await _context.FourPointInspections
            .Include(i => i.Receiving)
            .Include(i => i.FabricPO)
            .Include(i => i.FGPO)
                .ThenInclude(f => f!.Customer)
            .Where(i => i.Active && i.ReceivingId == receivingId)
            .OrderByDescending(i => i.InspectionDate)
            .ToListAsync();

        return items.Select(ToDto);
    }

    public async Task<FourPointDto> CreateAsync(CreateFourPointDto dto)
    {
        Validate(dto);

        var receiving = await ResolveReceivingAsync(dto.ReceivingId);
        var fabricPO = await _context.FabricPOs
            .FirstOrDefaultAsync(p => p.ID == dto.FabricPOId && p.Active)
            ?? throw new Exception("El Fabric PO seleccionado no es válido.");
        var fgpo = await _context.Fgpos
            .FirstOrDefaultAsync(f => f.ID == dto.FGPOId && f.Active)
            ?? throw new Exception("El FGPO seleccionado no es válido.");

        var lot = await GetOrCreateLotAsync(dto.FabricPOId, dto.FGPOId, dto.LotNumber);

        var entity = new FourPointInspection
        {
            InspectionDate = dto.InspectionDate,
            ReceivingId = dto.ReceivingId,
            ReceivingNumber = receiving?.ReceivingNumber,
            FabricPOId = dto.FabricPOId,
            FGPOId = dto.FGPOId,
            LotNumber = dto.LotNumber,
            Lot = lot,
            RollNumber = dto.RollNumber,
            Width = dto.Width,
            InspectedLength = dto.InspectedLength,
            Points1 = dto.Points1,
            Points2 = dto.Points2,
            Points3 = dto.Points3,
            Points4 = dto.Points4,
            MaxAllowed = dto.MaxAllowed,
            AcceptedQty = dto.AcceptedQty,
            RejectedQty = dto.RejectedQty,
            HoldQty = dto.HoldQty,
            InspectorId = dto.InspectorId,
            ReportLink = dto.ReportLink,
            Comments = dto.Comments,
            Active = true,
            CreatedAt = DateTime.UtcNow,
        };

        _context.FourPointInspections.Add(entity);
        await _context.SaveChangesAsync();

        await _context.Entry(entity).Reference(i => i.Receiving).LoadAsync();
        await _context.Entry(entity).Reference(i => i.FabricPO).LoadAsync();
        await _context.Entry(entity).Reference(i => i.FGPO).Query()
            .Include(f => f!.Customer).LoadAsync();

        return ToDto(entity);
    }

    public async Task<bool> UpdateAsync(int id, UpdateFourPointDto dto)
    {
        var entity = await _context.FourPointInspections
            .FirstOrDefaultAsync(i => i.ID == id && i.Active);
        if (entity is null)
            return false;

        Validate(dto);

        var receiving = await ResolveReceivingAsync(dto.ReceivingId);
        var fabricPO = await _context.FabricPOs
            .FirstOrDefaultAsync(p => p.ID == dto.FabricPOId && p.Active)
            ?? throw new Exception("El Fabric PO seleccionado no es válido.");
        var fgpo = await _context.Fgpos
            .FirstOrDefaultAsync(f => f.ID == dto.FGPOId && f.Active)
            ?? throw new Exception("El FGPO seleccionado no es válido.");

        var lot = await GetOrCreateLotAsync(dto.FabricPOId, dto.FGPOId, dto.LotNumber);

        entity.InspectionDate = dto.InspectionDate;
        entity.ReceivingId = dto.ReceivingId;
        entity.ReceivingNumber = receiving?.ReceivingNumber;
        entity.FabricPOId = dto.FabricPOId;
        entity.FGPOId = dto.FGPOId;
        entity.LotNumber = dto.LotNumber;
        entity.Lot = lot;
        entity.RollNumber = dto.RollNumber;
        entity.Width = dto.Width;
        entity.InspectedLength = dto.InspectedLength;
        entity.Points1 = dto.Points1;
        entity.Points2 = dto.Points2;
        entity.Points3 = dto.Points3;
        entity.Points4 = dto.Points4;
        entity.MaxAllowed = dto.MaxAllowed;
        entity.AcceptedQty = dto.AcceptedQty;
        entity.RejectedQty = dto.RejectedQty;
        entity.HoldQty = dto.HoldQty;
        entity.InspectorId = dto.InspectorId;
        entity.ReportLink = dto.ReportLink;
        entity.Comments = dto.Comments;
        entity.UpdatedAt = DateTime.UtcNow;

        _context.FourPointInspections.Update(entity);
        await _context.SaveChangesAsync();

        return true;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var entity = await _context.FourPointInspections
            .FirstOrDefaultAsync(i => i.ID == id && i.Active);
        if (entity is null)
            return false;

        entity.Active = false;
        entity.UpdatedAt = DateTime.UtcNow;
        _context.FourPointInspections.Update(entity);
        await _context.SaveChangesAsync();

        return true;
    }

    public async Task<PagedResultDto<FourPointDto>> GetPagedAsync(int page, int pageSize, string? search, string? sortBy, string? sortOrder, string? fabricPO, string? fgpo, string? result)
    {
        if (page < 1) page = 1;
        if (pageSize < 1) pageSize = 10;
        if (pageSize > 100) pageSize = 100;

        var query = _context.FourPointInspections
            .Include(i => i.Receiving)
            .Include(i => i.FabricPO)
            .Include(i => i.FGPO)
                .ThenInclude(f => f!.Customer)
            .Where(i => i.Active);

        if (!string.IsNullOrWhiteSpace(search))
        {
            var term = search.Trim();
            query = query.Where(i =>
                (i.RollNumber != null && i.RollNumber.Contains(term)) ||
                (i.LotNumber != null && i.LotNumber.Contains(term)) ||
                (i.ReceivingNumber != null && i.ReceivingNumber.Contains(term)) ||
                (i.Inspector != null && i.Inspector.UserName.Contains(term)) ||
                (i.Result != null && i.Result.Contains(term)) ||
                (i.FabricPO != null && i.FabricPO.FabricPONumber.Contains(term)) ||
                (i.FGPO != null && i.FGPO.FGPONumber.Contains(term)));
        }

        if (!string.IsNullOrWhiteSpace(fabricPO))
            query = query.Where(i => i.FabricPO != null && i.FabricPO.FabricPONumber.Contains(fabricPO.Trim()));

        if (!string.IsNullOrWhiteSpace(fgpo))
            query = query.Where(i => i.FGPO != null && i.FGPO.FGPONumber.Contains(fgpo.Trim()));

        if (!string.IsNullOrWhiteSpace(result))
            query = query.Where(i => i.Result != null && i.Result.Contains(result.Trim()));

        var totalCount = await query.CountAsync();
        var totalPages = (int)Math.Ceiling(totalCount / (double)pageSize);

        var sortByLower = sortBy?.ToLowerInvariant();
        var descending = sortOrder?.ToLowerInvariant() == "desc";

        IQueryable<FourPointInspection> orderedQuery = (sortByLower, descending) switch
        {
            ("inspectiondate", false) => query.OrderBy(i => i.InspectionDate),
            ("inspectiondate", true) => query.OrderByDescending(i => i.InspectionDate),
            ("rollnumber", false) => query.OrderBy(i => i.RollNumber),
            ("rollnumber", true) => query.OrderByDescending(i => i.RollNumber),
            ("totalpoints", false) => query.OrderBy(i => i.TotalPoints),
            ("totalpoints", true) => query.OrderByDescending(i => i.TotalPoints),
            ("pointsper100sqyd", false) => query.OrderBy(i => i.PointsPer100SqYd),
            ("pointsper100sqyd", true) => query.OrderByDescending(i => i.PointsPer100SqYd),
            ("result", false) => query.OrderBy(i => i.Result),
            ("result", true) => query.OrderByDescending(i => i.Result),
            ("createdat", false) => query.OrderBy(i => i.CreatedAt),
            ("createdat", true) => query.OrderByDescending(i => i.CreatedAt),
            _ => query.OrderByDescending(i => i.InspectionDate),
        };

        var items = await orderedQuery
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return new PagedResultDto<FourPointDto>
        {
            Items = items.Select(ToDto),
            TotalCount = totalCount,
            Page = page,
            PageSize = pageSize,
            TotalPages = totalPages,
        };
    }

    private async Task<FabricReceiving?> ResolveReceivingAsync(int? receivingId)
    {
        if (receivingId is null or 0)
            return null;

        return await _context.FabricReceivings
            .FirstOrDefaultAsync(r => r.ID == receivingId && r.Active);
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
            // Persistir el lote ahora para que tenga ID antes de insertar el registro padre
            await _context.SaveChangesAsync();
        }

        return lot;
    }

    private static void Validate(CreateFourPointDto dto)
    {
        if (dto.InspectionDate == default)
            throw new Exception("La Inspection Date es obligatoria.");

        if (dto.Width <= 0)
            throw new Exception("El Width debe ser mayor a 0.");

        if (dto.InspectedLength <= 0)
            throw new Exception("El Inspected Length debe ser mayor a 0.");

        if (dto.Points1 < 0 || dto.Points2 < 0 || dto.Points3 < 0 || dto.Points4 < 0)
            throw new Exception("Los conteos de puntos no pueden ser negativos.");

        if (dto.MaxAllowed < 0)
            throw new Exception("El Max Allowed no puede ser negativo.");
    }

    private static void Validate(UpdateFourPointDto dto)
    {
        if (dto.InspectionDate == default)
            throw new Exception("La Inspection Date es obligatoria.");

        if (dto.Width <= 0)
            throw new Exception("El Width debe ser mayor a 0.");

        if (dto.InspectedLength <= 0)
            throw new Exception("El Inspected Length debe ser mayor a 0.");

        if (dto.Points1 < 0 || dto.Points2 < 0 || dto.Points3 < 0 || dto.Points4 < 0)
            throw new Exception("Los conteos de puntos no pueden ser negativos.");

        if (dto.MaxAllowed < 0)
            throw new Exception("El Max Allowed no puede ser negativo.");
    }

    private static FourPointDto ToDto(FourPointInspection item)
    {
        return new FourPointDto
        {
            ID = item.ID,
            InspectionDate = item.InspectionDate,
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
            Width = item.Width,
            InspectedLength = item.InspectedLength,
            Points1 = item.Points1,
            Points2 = item.Points2,
            Points3 = item.Points3,
            Points4 = item.Points4,
            TotalPoints = item.TotalPoints,
            PointsPer100SqYd = item.PointsPer100SqYd,
            MaxAllowed = item.MaxAllowed,
            AcceptedQty = item.AcceptedQty,
            RejectedQty = item.RejectedQty,
            HoldQty = item.HoldQty,
            Result = item.Result,
            Inspector = item.Inspector?.UserName,
            ReportLink = item.ReportLink,
            Comments = item.Comments,
            Active = item.Active,
            CreatedAt = item.CreatedAt,
            UpdatedAt = item.UpdatedAt,
        };
    }
}
