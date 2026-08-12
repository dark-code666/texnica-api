using Microsoft.EntityFrameworkCore;
using ERP.Api.Data;
using ERP.Api.Dtos;
using ERP.Api.Domain;
using ERP.Api.Interfaces;

namespace ERP.Api.Services;

public class TopSampleService : ITopSampleService
{
    private readonly ErpDbContext _context;

    public TopSampleService(ErpDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<TopSampleDto>> GetAllAsync()
    {
        var items = await _context.TopSamples
            .Include(s => s.FGPO)
                .ThenInclude(f => f!.Customer)
            .Where(s => s.Active)
            .OrderByDescending(s => s.CreatedAt)
            .ToListAsync();

        return items.Select(ToDto);
    }

    public async Task<TopSampleDto?> GetByIdAsync(int id)
    {
        var item = await _context.TopSamples
            .Include(s => s.FGPO)
                .ThenInclude(f => f!.Customer)
            .FirstOrDefaultAsync(s => s.ID == id && s.Active);

        return item is null ? null : ToDto(item);
    }

    public async Task<IEnumerable<TopSampleDto>> GetByFgpoAsync(int fgpoId)
    {
        var items = await _context.TopSamples
            .Include(s => s.FGPO)
                .ThenInclude(f => f!.Customer)
            .Where(s => s.Active && s.FGPOId == fgpoId)
            .OrderByDescending(s => s.CreatedAt)
            .ToListAsync();

        return items.Select(ToDto);
    }

    public async Task<TopSampleDto> CreateAsync(CreateTopSampleDto dto)
    {
        Validate(dto);

        var fgpo = await _context.Fgpos
            .FirstOrDefaultAsync(f => f.ID == dto.FGPOId && f.Active)
            ?? throw new Exception("El FGPO seleccionado no es válido.");

        var entity = new TopSample
        {
            FGPOId = dto.FGPOId,
            SizeId = dto.SizeId,
            ProductionLine = dto.ProductionLine,
            FabricLot = dto.FabricLot,
            CutLotBundle = dto.CutLotBundle,
            TrimVersion = dto.TrimVersion,
            ThreadLot = dto.ThreadLot,
            TopQty = dto.TopQty,
            ProductionDate = dto.ProductionDate,
            MeasurementResult = dto.MeasurementResult,
            ConstructionResult = dto.ConstructionResult,
            WorkmanshipResult = dto.WorkmanshipResult,
            LabelResult = dto.LabelResult,
            PackingResult = dto.PackingResult,
            InternalReview = dto.InternalReview,
            CustomerReview = dto.CustomerReview,
            CorrectiveAction = dto.CorrectiveAction,
            ApprovalDate = dto.ApprovalDate,
            ApprovedByUserId = dto.ApprovedByUserId,
            Status = dto.Status,
            DocumentLink = dto.DocumentLink,
            PhotoLink = dto.PhotoLink,
            Active = true,
            CreatedAt = DateTime.UtcNow,
        };

        _context.TopSamples.Add(entity);
        await _context.SaveChangesAsync();

        await _context.Entry(entity).Reference(s => s.FGPO).Query()
            .Include(f => f!.Customer).LoadAsync();

        return ToDto(entity);
    }

    public async Task<bool> UpdateAsync(int id, UpdateTopSampleDto dto)
    {
        var entity = await _context.TopSamples
            .FirstOrDefaultAsync(s => s.ID == id && s.Active);
        if (entity is null)
            return false;

        Validate(dto);

        var fgpo = await _context.Fgpos
            .FirstOrDefaultAsync(f => f.ID == dto.FGPOId && f.Active)
            ?? throw new Exception("El FGPO seleccionado no es válido.");

        entity.FGPOId = dto.FGPOId;
        entity.SizeId = dto.SizeId;
        entity.ProductionLine = dto.ProductionLine;
        entity.FabricLot = dto.FabricLot;
        entity.CutLotBundle = dto.CutLotBundle;
        entity.TrimVersion = dto.TrimVersion;
        entity.ThreadLot = dto.ThreadLot;
        entity.TopQty = dto.TopQty;
        entity.ProductionDate = dto.ProductionDate;
        entity.MeasurementResult = dto.MeasurementResult;
        entity.ConstructionResult = dto.ConstructionResult;
        entity.WorkmanshipResult = dto.WorkmanshipResult;
        entity.LabelResult = dto.LabelResult;
        entity.PackingResult = dto.PackingResult;
        entity.InternalReview = dto.InternalReview;
        entity.CustomerReview = dto.CustomerReview;
        entity.CorrectiveAction = dto.CorrectiveAction;
        entity.ApprovalDate = dto.ApprovalDate;
        entity.ApprovedByUserId = dto.ApprovedByUserId;
        entity.Status = dto.Status;
        entity.DocumentLink = dto.DocumentLink;
        entity.PhotoLink = dto.PhotoLink;
        entity.UpdatedAt = DateTime.UtcNow;

        _context.TopSamples.Update(entity);
        await _context.SaveChangesAsync();

        return true;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var entity = await _context.TopSamples
            .FirstOrDefaultAsync(s => s.ID == id && s.Active);
        if (entity is null)
            return false;

        entity.Active = false;
        entity.UpdatedAt = DateTime.UtcNow;
        _context.TopSamples.Update(entity);
        await _context.SaveChangesAsync();

        return true;
    }

    public async Task<PagedResultDto<TopSampleDto>> GetPagedAsync(int page, int pageSize, string? search, string? sortBy, string? sortOrder, string? fgpo, string? status)
    {
        if (page < 1) page = 1;
        if (pageSize < 1) pageSize = 10;
        if (pageSize > 100) pageSize = 100;

        var query = _context.TopSamples
            .Include(s => s.FGPO)
                .ThenInclude(f => f!.Customer)
            .Where(s => s.Active);

        if (!string.IsNullOrWhiteSpace(search))
        {
            var term = search.Trim();
            query = query.Where(s =>
                (s.ProductionLine != null && s.ProductionLine.Contains(term)) ||
                (s.FabricLot != null && s.FabricLot.Contains(term)) ||
                (s.CutLotBundle != null && s.CutLotBundle.Contains(term)) ||
                (s.Size != null && s.Size.SizeCode.Contains(term)) ||
                (s.Status != null && s.Status.Contains(term)) ||
                (s.FGPO != null && s.FGPO.FGPONumber.Contains(term)));
        }

        if (!string.IsNullOrWhiteSpace(fgpo))
            query = query.Where(s => s.FGPO != null && s.FGPO.FGPONumber.Contains(fgpo.Trim()));

        if (!string.IsNullOrWhiteSpace(status))
            query = query.Where(s => s.Status != null && s.Status.Contains(status.Trim()));

        var totalCount = await query.CountAsync();
        var totalPages = (int)Math.Ceiling(totalCount / (double)pageSize);

        var sortByLower = sortBy?.ToLowerInvariant();
        var descending = sortOrder?.ToLowerInvariant() == "desc";

        IQueryable<TopSample> orderedQuery = (sortByLower, descending) switch
        {
            ("productionline", false) => query.OrderBy(s => s.ProductionLine),
            ("productionline", true) => query.OrderByDescending(s => s.ProductionLine),
            ("status", false) => query.OrderBy(s => s.Status),
            ("status", true) => query.OrderByDescending(s => s.Status),
            ("createdat", false) => query.OrderBy(s => s.CreatedAt),
            ("createdat", true) => query.OrderByDescending(s => s.CreatedAt),
            _ => query.OrderByDescending(s => s.CreatedAt),
        };

        var items = await orderedQuery
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return new PagedResultDto<TopSampleDto>
        {
            Items = items.Select(ToDto),
            TotalCount = totalCount,
            Page = page,
            PageSize = pageSize,
            TotalPages = totalPages,
        };
    }

    private static void Validate(CreateTopSampleDto dto)
    {
        if (dto.FGPOId <= 0)
            throw new Exception("El FGPO es obligatorio.");

        if (dto.TopQty < 0)
            throw new Exception("La TOP Qty no puede ser negativa.");
    }

    private static void Validate(UpdateTopSampleDto dto)
    {
        if (dto.FGPOId <= 0)
            throw new Exception("El FGPO es obligatorio.");

        if (dto.TopQty < 0)
            throw new Exception("La TOP Qty no puede ser negativa.");
    }

    private static TopSampleDto ToDto(TopSample item)
    {
        return new TopSampleDto
        {
            ID = item.ID,
            FGPOId = item.FGPOId,
            FGPONumber = item.FGPO?.FGPONumber ?? string.Empty,
            CustomerName = item.FGPO?.Customer?.Name ?? string.Empty,
            Style = item.FGPO?.FgpoLines?.FirstOrDefault(l => l.Active)?.Style?.StyleCode,
            Color = item.FGPO?.FgpoLines?.FirstOrDefault(l => l.Active)?.Color?.ColorName,
            Size = item.Size?.SizeCode,
            ProductionLine = item.ProductionLine,
            FabricLot = item.FabricLot,
            CutLotBundle = item.CutLotBundle,
            TrimVersion = item.TrimVersion,
            ThreadLot = item.ThreadLot,
            TopQty = item.TopQty,
            ProductionDate = item.ProductionDate,
            MeasurementResult = item.MeasurementResult,
            ConstructionResult = item.ConstructionResult,
            WorkmanshipResult = item.WorkmanshipResult,
            LabelResult = item.LabelResult,
            PackingResult = item.PackingResult,
            InternalReview = item.InternalReview,
            CustomerReview = item.CustomerReview,
            CorrectiveAction = item.CorrectiveAction,
            ApprovalDate = item.ApprovalDate,
            ApprovedBy = item.ApprovedBy?.UserName,
            Status = item.Status,
            DocumentLink = item.DocumentLink,
            PhotoLink = item.PhotoLink,
            Active = item.Active,
            CreatedAt = item.CreatedAt,
            UpdatedAt = item.UpdatedAt,
        };
    }
}
