using Microsoft.EntityFrameworkCore;
using ERP.Api.Data;
using ERP.Api.Dtos;
using ERP.Api.Domain;
using ERP.Api.Interfaces;

namespace ERP.Api.Services;

public class InternalTestService : IInternalTestService
{
    private readonly ErpDbContext _context;

    public InternalTestService(ErpDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<InternalTestDto>> GetAllAsync()
    {
        var items = await _context.InternalTests
            .Include(t => t.FabricPO)
            .Include(t => t.FGPO)
                .ThenInclude(f => f!.Customer)
            .Where(t => t.Active)
            .OrderByDescending(t => t.TestDate)
            .ToListAsync();

        return items.Select(ToDto);
    }

    public async Task<InternalTestDto?> GetByIdAsync(int id)
    {
        var item = await _context.InternalTests
            .Include(t => t.FabricPO)
            .Include(t => t.FGPO)
                .ThenInclude(f => f!.Customer)
            .FirstOrDefaultAsync(t => t.ID == id && t.Active);

        return item is null ? null : ToDto(item);
    }

    public async Task<IEnumerable<InternalTestDto>> GetByFabricPOAsync(int fabricPOId)
    {
        var items = await _context.InternalTests
            .Include(t => t.FabricPO)
            .Include(t => t.FGPO)
                .ThenInclude(f => f!.Customer)
            .Where(t => t.Active && t.FabricPOId == fabricPOId)
            .OrderByDescending(t => t.TestDate)
            .ToListAsync();

        return items.Select(ToDto);
    }

    public async Task<IEnumerable<InternalTestDto>> GetByFgpoAsync(int fgpoId)
    {
        var items = await _context.InternalTests
            .Include(t => t.FabricPO)
            .Include(t => t.FGPO)
                .ThenInclude(f => f!.Customer)
            .Where(t => t.Active && t.FGPOId == fgpoId)
            .OrderByDescending(t => t.TestDate)
            .ToListAsync();

        return items.Select(ToDto);
    }

    public async Task<InternalTestDto> CreateAsync(CreateInternalTestDto dto)
    {
        Validate(dto);

        var fabricPO = await _context.FabricPOs
            .FirstOrDefaultAsync(p => p.ID == dto.FabricPOId && p.Active)
            ?? throw new Exception("El Fabric PO seleccionado no es válido.");
        var fgpo = await _context.Fgpos
            .FirstOrDefaultAsync(f => f.ID == dto.FGPOId && f.Active)
            ?? throw new Exception("El FGPO seleccionado no es válido.");

        var lot = await GetOrCreateLotAsync(dto.FabricPOId, dto.FGPOId, dto.LotNumber);

        var entity = new InternalTest
        {
            TestDate = dto.TestDate,
            FabricPOId = dto.FabricPOId,
            FGPOId = dto.FGPOId,
            LotNumber = dto.LotNumber,
            Lot = lot,
            Color = dto.Color,
            ActualWidth = dto.ActualWidth,
            SpecimenAreaCm2 = dto.SpecimenAreaCm2,
            WeightBeforeG = dto.WeightBeforeG,
            WeightAfterG = dto.WeightAfterG,
            TargetGSM = dto.TargetGSM,
            LengthBefore = dto.LengthBefore,
            LengthAfter = dto.LengthAfter,
            WidthBefore = dto.WidthBefore,
            WidthAfter = dto.WidthAfter,
            TorquePct = dto.TorquePct,
            BowingPct = dto.BowingPct,
            SkewingPct = dto.SkewingPct,
            ShadeResult = dto.ShadeResult,
            WashAppearance = dto.WashAppearance,
            HandFeel = dto.HandFeel,
            TestResult = dto.TestResult,
            TestedByUserId = dto.TestedByUserId,
            ApprovedByUserId = dto.ApprovedByUserId,
            ReportLink = dto.ReportLink,
            Comments = dto.Comments,
            Active = true,
            CreatedAt = DateTime.UtcNow,
        };

        _context.InternalTests.Add(entity);
        await _context.SaveChangesAsync();

        await _context.Entry(entity).Reference(t => t.FabricPO).LoadAsync();
        await _context.Entry(entity).Reference(t => t.FGPO).Query()
            .Include(f => f!.Customer).LoadAsync();

        return ToDto(entity);
    }

    public async Task<bool> UpdateAsync(int id, UpdateInternalTestDto dto)
    {
        var entity = await _context.InternalTests
            .FirstOrDefaultAsync(t => t.ID == id && t.Active);
        if (entity is null)
            return false;

        Validate(dto);

        var fabricPO = await _context.FabricPOs
            .FirstOrDefaultAsync(p => p.ID == dto.FabricPOId && p.Active)
            ?? throw new Exception("El Fabric PO seleccionado no es válido.");
        var fgpo = await _context.Fgpos
            .FirstOrDefaultAsync(f => f.ID == dto.FGPOId && f.Active)
            ?? throw new Exception("El FGPO seleccionado no es válido.");

        var lot = await GetOrCreateLotAsync(dto.FabricPOId, dto.FGPOId, dto.LotNumber);

        entity.TestDate = dto.TestDate;
        entity.FabricPOId = dto.FabricPOId;
        entity.FGPOId = dto.FGPOId;
        entity.LotNumber = dto.LotNumber;
        entity.Lot = lot;
        entity.Color = dto.Color;
        entity.ActualWidth = dto.ActualWidth;
        entity.SpecimenAreaCm2 = dto.SpecimenAreaCm2;
        entity.WeightBeforeG = dto.WeightBeforeG;
        entity.WeightAfterG = dto.WeightAfterG;
        entity.TargetGSM = dto.TargetGSM;
        entity.LengthBefore = dto.LengthBefore;
        entity.LengthAfter = dto.LengthAfter;
        entity.WidthBefore = dto.WidthBefore;
        entity.WidthAfter = dto.WidthAfter;
        entity.TorquePct = dto.TorquePct;
        entity.BowingPct = dto.BowingPct;
        entity.SkewingPct = dto.SkewingPct;
        entity.ShadeResult = dto.ShadeResult;
        entity.WashAppearance = dto.WashAppearance;
        entity.HandFeel = dto.HandFeel;
        entity.TestResult = dto.TestResult;
        entity.TestedByUserId = dto.TestedByUserId;
        entity.ApprovedByUserId = dto.ApprovedByUserId;
        entity.ReportLink = dto.ReportLink;
        entity.Comments = dto.Comments;
        entity.UpdatedAt = DateTime.UtcNow;

        _context.InternalTests.Update(entity);
        await _context.SaveChangesAsync();

        return true;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var entity = await _context.InternalTests
            .FirstOrDefaultAsync(t => t.ID == id && t.Active);
        if (entity is null)
            return false;

        entity.Active = false;
        entity.UpdatedAt = DateTime.UtcNow;
        _context.InternalTests.Update(entity);
        await _context.SaveChangesAsync();

        return true;
    }

    public async Task<PagedResultDto<InternalTestDto>> GetPagedAsync(int page, int pageSize, string? search, string? sortBy, string? sortOrder, string? fabricPO, string? fgpo, string? testResult)
    {
        if (page < 1) page = 1;
        if (pageSize < 1) pageSize = 10;
        if (pageSize > 100) pageSize = 100;

        var query = _context.InternalTests
            .Include(t => t.FabricPO).ThenInclude(p => p.Supplier)
            .Include(t => t.FGPO)
                .ThenInclude(f => f!.Customer)
            .Where(t => t.Active);

        if (!string.IsNullOrWhiteSpace(search))
        {
            var term = search.Trim();
            query = query.Where(t =>
                (t.LotNumber != null && t.LotNumber.Contains(term)) ||
                (t.Color != null && t.Color.Contains(term)) ||
                (t.FabricPO != null && t.FabricPO.Supplier != null && t.FabricPO.Supplier.Name.Contains(term)) ||
                (t.TestedBy != null && t.TestedBy.UserName.Contains(term)) ||
                (t.TestResult != null && t.TestResult.Contains(term)) ||
                (t.FabricPO != null && t.FabricPO.FabricPONumber.Contains(term)) ||
                (t.FGPO != null && t.FGPO.FGPONumber.Contains(term)));
        }

        if (!string.IsNullOrWhiteSpace(fabricPO))
            query = query.Where(t => t.FabricPO != null && t.FabricPO.FabricPONumber.Contains(fabricPO.Trim()));

        if (!string.IsNullOrWhiteSpace(fgpo))
            query = query.Where(t => t.FGPO != null && t.FGPO.FGPONumber.Contains(fgpo.Trim()));

        if (!string.IsNullOrWhiteSpace(testResult))
            query = query.Where(t => t.TestResult != null && t.TestResult.Contains(testResult.Trim()));

        var totalCount = await query.CountAsync();
        var totalPages = (int)Math.Ceiling(totalCount / (double)pageSize);

        var sortByLower = sortBy?.ToLowerInvariant();
        var descending = sortOrder?.ToLowerInvariant() == "desc";

        IQueryable<InternalTest> orderedQuery = (sortByLower, descending) switch
        {
            ("testdate", false) => query.OrderBy(t => t.TestDate),
            ("testdate", true) => query.OrderByDescending(t => t.TestDate),
            ("lotnumber", false) => query.OrderBy(t => t.LotNumber),
            ("lotnumber", true) => query.OrderByDescending(t => t.LotNumber),
            ("gsmvariancepct", false) => query.OrderBy(t => t.GsmVariancePct),
            ("gsmvariancepct", true) => query.OrderByDescending(t => t.GsmVariancePct),
            ("lengthshrinkagepct", false) => query.OrderBy(t => t.LengthShrinkagePct),
            ("lengthshrinkagepct", true) => query.OrderByDescending(t => t.LengthShrinkagePct),
            ("testresult", false) => query.OrderBy(t => t.TestResult),
            ("testresult", true) => query.OrderByDescending(t => t.TestResult),
            ("createdat", false) => query.OrderBy(t => t.CreatedAt),
            ("createdat", true) => query.OrderByDescending(t => t.CreatedAt),
            _ => query.OrderByDescending(t => t.TestDate),
        };

        var items = await orderedQuery
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return new PagedResultDto<InternalTestDto>
        {
            Items = items.Select(ToDto),
            TotalCount = totalCount,
            Page = page,
            PageSize = pageSize,
            TotalPages = totalPages,
        };
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

    private static void Validate(CreateInternalTestDto dto)
    {
        if (dto.TestDate == default)
            throw new Exception("La Test Date es obligatoria.");

        if (dto.ActualWidth <= 0)
            throw new Exception("El Actual Width debe ser mayor a 0.");

        if (dto.SpecimenAreaCm2 <= 0)
            throw new Exception("El Specimen Area debe ser mayor a 0.");

        if (dto.TargetGSM <= 0)
            throw new Exception("El Target GSM debe ser mayor a 0.");

        if (dto.WeightBeforeG < 0 || dto.WeightAfterG < 0)
            throw new Exception("Los pesos no pueden ser negativos.");
    }

    private static void Validate(UpdateInternalTestDto dto)
    {
        if (dto.TestDate == default)
            throw new Exception("La Test Date es obligatoria.");

        if (dto.ActualWidth <= 0)
            throw new Exception("El Actual Width debe ser mayor a 0.");

        if (dto.SpecimenAreaCm2 <= 0)
            throw new Exception("El Specimen Area debe ser mayor a 0.");

        if (dto.TargetGSM <= 0)
            throw new Exception("El Target GSM debe ser mayor a 0.");

        if (dto.WeightBeforeG < 0 || dto.WeightAfterG < 0)
            throw new Exception("Los pesos no pueden ser negativos.");
    }

    private static InternalTestDto ToDto(InternalTest item)
    {
        return new InternalTestDto
        {
            ID = item.ID,
            TestDate = item.TestDate,
            FabricPOId = item.FabricPOId,
            FabricPONumber = item.FabricPO?.FabricPONumber ?? string.Empty,
            FGPOId = item.FGPOId,
            FGPONumber = item.FGPO?.FGPONumber ?? string.Empty,
            CustomerName = item.FGPO?.Customer?.Name ?? string.Empty,
            Supplier = item.FabricPO?.Supplier?.Name,
            LotNumber = item.LotNumber,
            LotId = item.LotId,
            Color = item.Color,
            ActualWidth = item.ActualWidth,
            SpecimenAreaCm2 = item.SpecimenAreaCm2,
            WeightBeforeG = item.WeightBeforeG,
            WeightAfterG = item.WeightAfterG,
            TargetGSM = item.TargetGSM,
            GsmBefore = item.GsmBefore,
            GsmAfter = item.GsmAfter,
            GsmVariancePct = item.GsmVariancePct,
            LengthBefore = item.LengthBefore,
            LengthAfter = item.LengthAfter,
            LengthShrinkagePct = item.LengthShrinkagePct,
            WidthBefore = item.WidthBefore,
            WidthAfter = item.WidthAfter,
            WidthShrinkagePct = item.WidthShrinkagePct,
            TorquePct = item.TorquePct,
            BowingPct = item.BowingPct,
            SkewingPct = item.SkewingPct,
            ShadeResult = item.ShadeResult,
            WashAppearance = item.WashAppearance,
            HandFeel = item.HandFeel,
            TestResult = item.TestResult,
            TestedBy = item.TestedBy?.UserName,
            ApprovedBy = item.ApprovedBy?.UserName,
            ReportLink = item.ReportLink,
            Comments = item.Comments,
            Active = item.Active,
            CreatedAt = item.CreatedAt,
            UpdatedAt = item.UpdatedAt,
        };
    }
}
