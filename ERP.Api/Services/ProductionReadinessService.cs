using Microsoft.EntityFrameworkCore;
using ERP.Api.Data;
using ERP.Api.Dtos;
using ERP.Api.Domain;
using ERP.Api.Interfaces;

namespace ERP.Api.Services;

public class ProductionReadinessService : IProductionReadinessService
{
    private readonly ErpDbContext _context;

    public ProductionReadinessService(ErpDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<ProductionReadinessDto>> GetAllAsync()
    {
        var items = await _context.ProductionReadiness
            .Include(p => p.FGPO)
                .ThenInclude(f => f!.Customer)
            .Where(p => p.Active)
            .OrderByDescending(p => p.ReviewDate)
            .ToListAsync();

        return items.Select(ToDto);
    }

    public async Task<ProductionReadinessDto?> GetByIdAsync(int id)
    {
        var item = await _context.ProductionReadiness
            .Include(p => p.FGPO)
                .ThenInclude(f => f!.Customer)
            .FirstOrDefaultAsync(p => p.ID == id && p.Active);

        return item is null ? null : ToDto(item);
    }

    public async Task<IEnumerable<ProductionReadinessDto>> GetByFgpoAsync(int fgpoId)
    {
        var items = await _context.ProductionReadiness
            .Include(p => p.FGPO)
                .ThenInclude(f => f!.Customer)
            .Where(p => p.Active && p.FGPOId == fgpoId)
            .OrderByDescending(p => p.ReviewDate)
            .ToListAsync();

        return items.Select(ToDto);
    }

    public async Task<ProductionReadinessDto> CreateAsync(CreateProductionReadinessDto dto)
    {
        Validate(dto);

        var fgpo = await _context.Fgpos
            .FirstOrDefaultAsync(f => f.ID == dto.FGPOId && f.Active)
            ?? throw new Exception("El FGPO seleccionado no es válido.");

        var entity = new ProductionReadiness
        {
            ReviewDate = dto.ReviewDate,
            FGPOId = dto.FGPOId,
            PoConfirmed = dto.PoConfirmed,
            TechPackCurrent = dto.TechPackCurrent,
            FabricApproved = dto.FabricApproved,
            TrimsApproved = dto.TrimsApproved,
            TrimsAvailable = dto.TrimsAvailable,
            PpSampleApproved = dto.PpSampleApproved,
            PatternApproved = dto.PatternApproved,
            MarkerApproved = dto.MarkerApproved,
            FabricWidthConfirmed = dto.FabricWidthConfirmed,
            ShrinkageApproved = dto.ShrinkageApproved,
            TorqueApproved = dto.TorqueApproved,
            QualityStandardReady = dto.QualityStandardReady,
            LinePlanned = dto.LinePlanned,
            OpenConditions = dto.OpenConditions,
            ResponsibleOwnerId = dto.ResponsibleOwnerId,
            DueDate = dto.DueDate,
            ApprovedByUserId = dto.ApprovedByUserId,
            Active = true,
            CreatedAt = DateTime.UtcNow,
        };

        _context.ProductionReadiness.Add(entity);
        await _context.SaveChangesAsync();

        await _context.Entry(entity).Reference(p => p.FGPO).Query()
            .Include(f => f!.Customer).LoadAsync();

        return ToDto(entity);
    }

    public async Task<bool> UpdateAsync(int id, UpdateProductionReadinessDto dto)
    {
        var entity = await _context.ProductionReadiness
            .FirstOrDefaultAsync(p => p.ID == id && p.Active);
        if (entity is null)
            return false;

        Validate(dto);

        var fgpo = await _context.Fgpos
            .FirstOrDefaultAsync(f => f.ID == dto.FGPOId && f.Active)
            ?? throw new Exception("El FGPO seleccionado no es válido.");

        entity.ReviewDate = dto.ReviewDate;
        entity.FGPOId = dto.FGPOId;
        entity.PoConfirmed = dto.PoConfirmed;
        entity.TechPackCurrent = dto.TechPackCurrent;
        entity.FabricApproved = dto.FabricApproved;
        entity.TrimsApproved = dto.TrimsApproved;
        entity.TrimsAvailable = dto.TrimsAvailable;
        entity.PpSampleApproved = dto.PpSampleApproved;
        entity.PatternApproved = dto.PatternApproved;
        entity.MarkerApproved = dto.MarkerApproved;
        entity.FabricWidthConfirmed = dto.FabricWidthConfirmed;
        entity.ShrinkageApproved = dto.ShrinkageApproved;
        entity.TorqueApproved = dto.TorqueApproved;
        entity.QualityStandardReady = dto.QualityStandardReady;
        entity.LinePlanned = dto.LinePlanned;
        entity.OpenConditions = dto.OpenConditions;
        entity.ResponsibleOwnerId = dto.ResponsibleOwnerId;
        entity.DueDate = dto.DueDate;
        entity.ApprovedByUserId = dto.ApprovedByUserId;
        entity.UpdatedAt = DateTime.UtcNow;

        _context.ProductionReadiness.Update(entity);
        await _context.SaveChangesAsync();

        return true;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var entity = await _context.ProductionReadiness
            .FirstOrDefaultAsync(p => p.ID == id && p.Active);
        if (entity is null)
            return false;

        entity.Active = false;
        entity.UpdatedAt = DateTime.UtcNow;
        _context.ProductionReadiness.Update(entity);
        await _context.SaveChangesAsync();

        return true;
    }

    public async Task<PagedResultDto<ProductionReadinessDto>> GetPagedAsync(int page, int pageSize, string? search, string? sortBy, string? sortOrder, string? fgpo, string? result)
    {
        if (page < 1) page = 1;
        if (pageSize < 1) pageSize = 10;
        if (pageSize > 100) pageSize = 100;

        var query = _context.ProductionReadiness
            .Include(p => p.FGPO)
                .ThenInclude(f => f!.Customer)
            .Where(p => p.Active);

        if (!string.IsNullOrWhiteSpace(search))
        {
            var term = search.Trim();
            query = query.Where(p =>
                (p.ResponsibleOwner != null && p.ResponsibleOwner.UserName.Contains(term)) ||
                (p.OverallResult != null && p.OverallResult.Contains(term)) ||
                (p.OpenConditions != null && p.OpenConditions.Contains(term)) ||
                (p.FGPO != null && p.FGPO.FGPONumber.Contains(term)));
        }

        if (!string.IsNullOrWhiteSpace(fgpo))
            query = query.Where(p => p.FGPO != null && p.FGPO.FGPONumber.Contains(fgpo.Trim()));

        if (!string.IsNullOrWhiteSpace(result))
            query = query.Where(p => p.OverallResult != null && p.OverallResult.Contains(result.Trim()));

        var totalCount = await query.CountAsync();
        var totalPages = (int)Math.Ceiling(totalCount / (double)pageSize);

        var sortByLower = sortBy?.ToLowerInvariant();
        var descending = sortOrder?.ToLowerInvariant() == "desc";

        IQueryable<ProductionReadiness> orderedQuery = (sortByLower, descending) switch
        {
            ("reviewdate", false) => query.OrderBy(p => p.ReviewDate),
            ("reviewdate", true) => query.OrderByDescending(p => p.ReviewDate),
            ("overallresult", false) => query.OrderBy(p => p.OverallResult),
            ("overallresult", true) => query.OrderByDescending(p => p.OverallResult),
            ("createdat", false) => query.OrderBy(p => p.CreatedAt),
            ("createdat", true) => query.OrderByDescending(p => p.CreatedAt),
            _ => query.OrderByDescending(p => p.ReviewDate),
        };

        var items = await orderedQuery
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return new PagedResultDto<ProductionReadinessDto>
        {
            Items = items.Select(ToDto),
            TotalCount = totalCount,
            Page = page,
            PageSize = pageSize,
            TotalPages = totalPages,
        };
    }

    private static void Validate(CreateProductionReadinessDto dto)
    {
        if (dto.ReviewDate == default)
            throw new Exception("La Review Date es obligatoria.");
    }

    private static void Validate(UpdateProductionReadinessDto dto)
    {
        if (dto.ReviewDate == default)
            throw new Exception("La Review Date es obligatoria.");
    }

    private static ProductionReadinessDto ToDto(ProductionReadiness item)
    {
        return new ProductionReadinessDto
        {
            ID = item.ID,
            ReviewDate = item.ReviewDate,
            FGPOId = item.FGPOId,
            FgpoNumber = item.FGPO?.FGPONumber ?? string.Empty,
            PoConfirmed = item.PoConfirmed,
            TechPackCurrent = item.TechPackCurrent,
            FabricApproved = item.FabricApproved,
            TrimsApproved = item.TrimsApproved,
            TrimsAvailable = item.TrimsAvailable,
            PpSampleApproved = item.PpSampleApproved,
            PatternApproved = item.PatternApproved,
            MarkerApproved = item.MarkerApproved,
            FabricWidthConfirmed = item.FabricWidthConfirmed,
            ShrinkageApproved = item.ShrinkageApproved,
            TorqueApproved = item.TorqueApproved,
            QualityStandardReady = item.QualityStandardReady,
            LinePlanned = item.LinePlanned,
            OverallResult = item.OverallResult,
            OpenConditions = item.OpenConditions,
            ResponsibleOwnerName = item.ResponsibleOwner?.UserName,
            DueDate = item.DueDate,
            ApprovedByName = item.ApprovedBy?.UserName,
            Active = item.Active,
            CreatedAt = item.CreatedAt,
            UpdatedAt = item.UpdatedAt,
        };
    }
}
