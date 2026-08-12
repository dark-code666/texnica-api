using Microsoft.EntityFrameworkCore;
using ERP.Api.Data;
using ERP.Api.Dtos;
using ERP.Api.Domain;
using ERP.Api.Interfaces;

namespace ERP.Api.Services;

public class PpSampleService : IPpSampleService
{
    private readonly ErpDbContext _context;

    public PpSampleService(ErpDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<PpSampleDto>> GetAllAsync()
    {
        var items = await _context.PpSamples
            .Include(s => s.FGPO)
                .ThenInclude(f => f!.Customer)
            .Where(s => s.Active)
            .OrderByDescending(s => s.CreatedAt)
            .ToListAsync();

        return items.Select(ToDto);
    }

    public async Task<PpSampleDto?> GetByIdAsync(int id)
    {
        var item = await _context.PpSamples
            .Include(s => s.FGPO)
                .ThenInclude(f => f!.Customer)
            .FirstOrDefaultAsync(s => s.ID == id && s.Active);

        return item is null ? null : ToDto(item);
    }

    public async Task<IEnumerable<PpSampleDto>> GetByFgpoAsync(int fgpoId)
    {
        var items = await _context.PpSamples
            .Include(s => s.FGPO)
                .ThenInclude(f => f!.Customer)
            .Where(s => s.Active && s.FGPOId == fgpoId)
            .OrderByDescending(s => s.CreatedAt)
            .ToListAsync();

        return items.Select(ToDto);
    }

    public async Task<PpSampleDto> CreateAsync(CreatePpSampleDto dto)
    {
        Validate(dto);

        var fgpo = await _context.Fgpos
            .FirstOrDefaultAsync(f => f.ID == dto.FGPOId && f.Active)
            ?? throw new Exception("El FGPO seleccionado no es válido.");

        var entity = new PpSample
        {
            FGPOId = dto.FGPOId,
            SizeId = dto.SizeId,
            SampleVersion = dto.SampleVersion,
            FabricLot = dto.FabricLot,
            TrimVersion = dto.TrimVersion,
            PreparationDate = dto.PreparationDate,
            SubmissionDate = dto.SubmissionDate,
            MeasurementResult = dto.MeasurementResult,
            ConstructionResult = dto.ConstructionResult,
            FitResult = dto.FitResult,
            FabricResult = dto.FabricResult,
            TrimResult = dto.TrimResult,
            LabelResult = dto.LabelResult,
            InternalReview = dto.InternalReview,
            CustomerReview = dto.CustomerReview,
            CustomerComments = dto.CustomerComments,
            ApprovalDate = dto.ApprovalDate,
            ApprovedByUserId = dto.ApprovedByUserId,
            Status = dto.Status,
            DocumentLink = dto.DocumentLink,
            PhotoLink = dto.PhotoLink,
            Active = true,
            CreatedAt = DateTime.UtcNow,
        };

        _context.PpSamples.Add(entity);
        await _context.SaveChangesAsync();

        await _context.Entry(entity).Reference(s => s.FGPO).Query()
            .Include(f => f!.Customer).LoadAsync();

        return ToDto(entity);
    }

    public async Task<bool> UpdateAsync(int id, UpdatePpSampleDto dto)
    {
        var entity = await _context.PpSamples
            .FirstOrDefaultAsync(s => s.ID == id && s.Active);
        if (entity is null)
            return false;

        Validate(dto);

        var fgpo = await _context.Fgpos
            .FirstOrDefaultAsync(f => f.ID == dto.FGPOId && f.Active)
            ?? throw new Exception("El FGPO seleccionado no es válido.");

        entity.FGPOId = dto.FGPOId;
        entity.SizeId = dto.SizeId;
        entity.SampleVersion = dto.SampleVersion;
        entity.FabricLot = dto.FabricLot;
        entity.TrimVersion = dto.TrimVersion;
        entity.PreparationDate = dto.PreparationDate;
        entity.SubmissionDate = dto.SubmissionDate;
        entity.MeasurementResult = dto.MeasurementResult;
        entity.ConstructionResult = dto.ConstructionResult;
        entity.FitResult = dto.FitResult;
        entity.FabricResult = dto.FabricResult;
        entity.TrimResult = dto.TrimResult;
        entity.LabelResult = dto.LabelResult;
        entity.InternalReview = dto.InternalReview;
        entity.CustomerReview = dto.CustomerReview;
        entity.CustomerComments = dto.CustomerComments;
        entity.ApprovalDate = dto.ApprovalDate;
        entity.ApprovedByUserId = dto.ApprovedByUserId;
        entity.Status = dto.Status;
        entity.DocumentLink = dto.DocumentLink;
        entity.PhotoLink = dto.PhotoLink;
        entity.UpdatedAt = DateTime.UtcNow;

        _context.PpSamples.Update(entity);
        await _context.SaveChangesAsync();

        return true;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var entity = await _context.PpSamples
            .FirstOrDefaultAsync(s => s.ID == id && s.Active);
        if (entity is null)
            return false;

        entity.Active = false;
        entity.UpdatedAt = DateTime.UtcNow;
        _context.PpSamples.Update(entity);
        await _context.SaveChangesAsync();

        return true;
    }

    public async Task<PagedResultDto<PpSampleDto>> GetPagedAsync(int page, int pageSize, string? search, string? sortBy, string? sortOrder, string? fgpo, string? status)
    {
        if (page < 1) page = 1;
        if (pageSize < 1) pageSize = 10;
        if (pageSize > 100) pageSize = 100;

        var query = _context.PpSamples
            .Include(s => s.FGPO)
                .ThenInclude(f => f!.Customer)
            .Where(s => s.Active);

        if (!string.IsNullOrWhiteSpace(search))
        {
            var term = search.Trim();
            query = query.Where(s =>
                (s.SampleVersion != null && s.SampleVersion.Contains(term)) ||
                (s.FabricLot != null && s.FabricLot.Contains(term)) ||
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

        IQueryable<PpSample> orderedQuery = (sortByLower, descending) switch
        {
            ("sampleversion", false) => query.OrderBy(s => s.SampleVersion),
            ("sampleversion", true) => query.OrderByDescending(s => s.SampleVersion),
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

        return new PagedResultDto<PpSampleDto>
        {
            Items = items.Select(ToDto),
            TotalCount = totalCount,
            Page = page,
            PageSize = pageSize,
            TotalPages = totalPages,
        };
    }

    private static void Validate(CreatePpSampleDto dto)
    {
        if (dto.FGPOId <= 0)
            throw new Exception("El FGPO es obligatorio.");
    }

    private static void Validate(UpdatePpSampleDto dto)
    {
        if (dto.FGPOId <= 0)
            throw new Exception("El FGPO es obligatorio.");
    }

    private static PpSampleDto ToDto(PpSample item)
    {
        return new PpSampleDto
        {
            ID = item.ID,
            FGPOId = item.FGPOId,
            FGPONumber = item.FGPO?.FGPONumber ?? string.Empty,
            CustomerName = item.FGPO?.Customer?.Name ?? string.Empty,
            Style = item.FGPO?.FgpoLines?.FirstOrDefault(l => l.Active)?.Style?.StyleCode,
            Color = item.FGPO?.FgpoLines?.FirstOrDefault(l => l.Active)?.Color?.ColorName,
            Size = item.Size?.SizeCode,
            SampleVersion = item.SampleVersion,
            FabricLot = item.FabricLot,
            TrimVersion = item.TrimVersion,
            PreparationDate = item.PreparationDate,
            SubmissionDate = item.SubmissionDate,
            MeasurementResult = item.MeasurementResult,
            ConstructionResult = item.ConstructionResult,
            FitResult = item.FitResult,
            FabricResult = item.FabricResult,
            TrimResult = item.TrimResult,
            LabelResult = item.LabelResult,
            InternalReview = item.InternalReview,
            CustomerReview = item.CustomerReview,
            CustomerComments = item.CustomerComments,
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
