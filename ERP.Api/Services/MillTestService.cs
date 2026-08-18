using Microsoft.EntityFrameworkCore;
using ERP.Api.Data;
using ERP.Api.Dtos;
using ERP.Api.Domain;
using ERP.Api.Interfaces;

namespace ERP.Api.Services;

public class MillTestService : IMillTestService
{
    private readonly ErpDbContext _context;

    private static readonly string[] ValidTestResults =
    {
        "Pending", "Testing", "Passed", "Conditionally Passed", "Failed"
    };

    public MillTestService(ErpDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<MillTestDto>> GetAllAsync()
    {
        var items = await _context.MillTests
            .Include(m => m.FabricPO)
            .Include(m => m.FGPO)
                .ThenInclude(f => f!.Customer)
            .Where(m => m.Active)
            .OrderByDescending(m => m.CreatedAt)
            .ToListAsync();

        return items.Select(ToDto);
    }

    public async Task<MillTestDto?> GetByIdAsync(int id)
    {
        var item = await _context.MillTests
            .Include(m => m.FabricPO)
            .Include(m => m.FGPO)
                .ThenInclude(f => f!.Customer)
            .FirstOrDefaultAsync(m => m.ID == id && m.Active);

        return item is null ? null : ToDto(item);
    }

    public async Task<IEnumerable<MillTestDto>> GetByFabricPOAsync(int fabricPOId)
    {
        var items = await _context.MillTests
            .Include(m => m.FabricPO)
            .Include(m => m.FGPO)
                .ThenInclude(f => f!.Customer)
            .Where(m => m.Active && m.FabricPOId == fabricPOId)
            .OrderByDescending(m => m.CreatedAt)
            .ToListAsync();

        return items.Select(ToDto);
    }

    public async Task<IEnumerable<MillTestDto>> GetByFgpoAsync(int fgpoId)
    {
        var items = await _context.MillTests
            .Include(m => m.FabricPO)
            .Include(m => m.FGPO)
                .ThenInclude(f => f!.Customer)
            .Where(m => m.Active && m.FGPOId == fgpoId)
            .OrderByDescending(m => m.CreatedAt)
            .ToListAsync();

        return items.Select(ToDto);
    }

    public async Task<MillTestDto> CreateAsync(CreateMillTestDto dto)
    {
        Validate(dto);

        var fabricPO = await _context.FabricPOs
            .FirstOrDefaultAsync(p => p.ID == dto.FabricPOId && p.Active);
        if (fabricPO is null)
            throw new Exception("El Fabric PO seleccionado no es válido.");

        var fgpo = await _context.Fgpos
            .FirstOrDefaultAsync(f => f.ID == dto.FGPOId && f.Active);
        if (fgpo is null)
            throw new Exception("El FGPO seleccionado no es válido.");

        var lot = await GetOrCreateLotAsync(dto.FabricPOId, dto.FGPOId, dto.LotNumber);

        var entity = new MillTest
        {
            FabricPOId = dto.FabricPOId,
            FGPOId = dto.FGPOId,
            LotNumber = dto.LotNumber,
            Lot = lot,
            Color = dto.Color,
            RollQty = dto.RollQty,
            ActualWidth = dto.ActualWidth,
            ActualGSM = dto.ActualGSM,
            LengthShrinkagePercentage = dto.LengthShrinkagePercentage,
            WidthShrinkagePercentage = dto.WidthShrinkagePercentage,
            TorquePercentage = dto.TorquePercentage,
            BowingPercentage = dto.BowingPercentage,
            SkewingPercentage = dto.SkewingPercentage,
            Colorfastness = dto.Colorfastness,
            WashAppearance = dto.WashAppearance,
            HandFeel = dto.HandFeel,
            TestDate = dto.TestDate,
            TestedByUserId = dto.TestedByUserId,
            TestResult = dto.TestResult,
            ApprovedForExport = dto.ApprovedForExport,
            ReportLink = dto.ReportLink,
            Comments = dto.Comments,
            Active = true,
            CreatedAt = DateTime.UtcNow,
        };

        _context.MillTests.Add(entity);
        await _context.SaveChangesAsync();

        await _context.Entry(entity).Reference(m => m.FabricPO).LoadAsync();
        await _context.Entry(entity).Reference(m => m.FGPO).Query()
            .Include(f => f!.Customer).LoadAsync();

        return ToDto(entity);
    }

    public async Task<bool> UpdateAsync(int id, UpdateMillTestDto dto)
    {
        var entity = await _context.MillTests
            .FirstOrDefaultAsync(m => m.ID == id && m.Active);

        if (entity is null)
            return false;

        ValidateUpdate(dto);

        var fabricPO = await _context.FabricPOs
            .FirstOrDefaultAsync(p => p.ID == dto.FabricPOId && p.Active);
        if (fabricPO is null)
            throw new Exception("El Fabric PO seleccionado no es válido.");

        var fgpo = await _context.Fgpos
            .FirstOrDefaultAsync(f => f.ID == dto.FGPOId && f.Active);
        if (fgpo is null)
            throw new Exception("El FGPO seleccionado no es válido.");

        var lot = await GetOrCreateLotAsync(dto.FabricPOId, dto.FGPOId, dto.LotNumber);

        entity.FabricPOId = dto.FabricPOId;
        entity.FGPOId = dto.FGPOId;
        entity.LotNumber = dto.LotNumber;
        entity.Lot = lot;
        entity.Color = dto.Color;
        entity.RollQty = dto.RollQty;
        entity.ActualWidth = dto.ActualWidth;
        entity.ActualGSM = dto.ActualGSM;
        entity.LengthShrinkagePercentage = dto.LengthShrinkagePercentage;
        entity.WidthShrinkagePercentage = dto.WidthShrinkagePercentage;
        entity.TorquePercentage = dto.TorquePercentage;
        entity.BowingPercentage = dto.BowingPercentage;
        entity.SkewingPercentage = dto.SkewingPercentage;
        entity.Colorfastness = dto.Colorfastness;
        entity.WashAppearance = dto.WashAppearance;
        entity.HandFeel = dto.HandFeel;
        entity.TestDate = dto.TestDate;
        entity.TestedByUserId = dto.TestedByUserId;
        entity.TestResult = dto.TestResult;
        entity.ApprovedForExport = dto.ApprovedForExport;
        entity.ReportLink = dto.ReportLink;
        entity.Comments = dto.Comments;
        entity.UpdatedAt = DateTime.UtcNow;

        _context.MillTests.Update(entity);
        await _context.SaveChangesAsync();

        return true;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var entity = await _context.MillTests
            .FirstOrDefaultAsync(m => m.ID == id && m.Active);

        if (entity is null)
            return false;

        entity.Active = false;
        entity.UpdatedAt = DateTime.UtcNow;

        _context.MillTests.Update(entity);
        await _context.SaveChangesAsync();

        return true;
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
        }

        return lot;
    }

    public async Task<PagedResultDto<MillTestDto>> GetPagedAsync(int page, int pageSize, string? search, string? sortBy, string? sortOrder, string? fabricPO, string? fgpo, string? lotNumber, string? testResult)
    {
        if (page < 1) page = 1;
        if (pageSize < 1) pageSize = 10;
        if (pageSize > 100) pageSize = 100;

        var query = _context.MillTests
            .Include(m => m.FabricPO).ThenInclude(p => p.Supplier)
            .Include(m => m.FGPO)
                .ThenInclude(f => f!.Customer)
            .Where(m => m.Active);

        if (!string.IsNullOrWhiteSpace(search))
        {
            var searchTerm = search.Trim();
            query = query.Where(m =>
                (m.LotNumber != null && m.LotNumber.Contains(searchTerm)) ||
                (m.FabricPO != null && m.FabricPO.Supplier != null && m.FabricPO.Supplier.Name.Contains(searchTerm)) ||
                (m.TestedBy != null && m.TestedBy.UserName.Contains(searchTerm)) ||
                (m.FabricPO != null && m.FabricPO.FabricPONumber.Contains(searchTerm)) ||
                (m.FGPO != null && m.FGPO.FGPONumber.Contains(searchTerm)));
        }

        if (!string.IsNullOrWhiteSpace(fabricPO))
            query = query.Where(m => m.FabricPO != null && m.FabricPO.FabricPONumber.Contains(fabricPO.Trim()));

        if (!string.IsNullOrWhiteSpace(fgpo))
            query = query.Where(m => m.FGPO != null && m.FGPO.FGPONumber.Contains(fgpo.Trim()));

        if (!string.IsNullOrWhiteSpace(lotNumber))
            query = query.Where(m => m.LotNumber != null && m.LotNumber.Contains(lotNumber.Trim()));

        if (!string.IsNullOrWhiteSpace(testResult))
            query = query.Where(m => m.TestResult == testResult);

        var totalCount = await query.CountAsync();
        var totalPages = (int)Math.Ceiling(totalCount / (double)pageSize);

        var sortByLower = sortBy?.ToLowerInvariant();
        var sortOrderLower = sortOrder?.ToLowerInvariant();
        var descending = sortOrderLower == "desc";

        IQueryable<MillTest> orderedQuery = (sortByLower, descending) switch
        {
            ("lotnumber", false) => query.OrderBy(m => m.LotNumber),
            ("lotnumber", true) => query.OrderByDescending(m => m.LotNumber),
            ("supplier", false) => query.OrderBy(m => m.FabricPO != null && m.FabricPO.Supplier != null ? m.FabricPO.Supplier.Name : null),
            ("supplier", true) => query.OrderByDescending(m => m.FabricPO != null && m.FabricPO.Supplier != null ? m.FabricPO.Supplier.Name : null),
            ("testdate", false) => query.OrderBy(m => m.TestDate),
            ("testdate", true) => query.OrderByDescending(m => m.TestDate),
            ("testresult", false) => query.OrderBy(m => m.TestResult),
            ("testresult", true) => query.OrderByDescending(m => m.TestResult),
            ("testedby", false) => query.OrderBy(m => m.TestedBy != null ? m.TestedBy.UserName : null),
            ("testedby", true) => query.OrderByDescending(m => m.TestedBy != null ? m.TestedBy.UserName : null),
            ("createdat", false) => query.OrderBy(m => m.CreatedAt),
            ("createdat", true) => query.OrderByDescending(m => m.CreatedAt),
            _ => query.OrderByDescending(m => m.CreatedAt),
        };

        var items = await orderedQuery
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return new PagedResultDto<MillTestDto>
        {
            Items = items.Select(ToDto),
            TotalCount = totalCount,
            Page = page,
            PageSize = pageSize,
            TotalPages = totalPages,
        };
    }

    private static void Validate(CreateMillTestDto dto)
    {
        if (dto.RollQty <= 0)
            throw new Exception("La Roll Qty debe ser mayor que 0.");

        if (dto.TestDate == default)
            throw new Exception("La Test Date es obligatoria.");

        if (string.IsNullOrWhiteSpace(dto.LotNumber))
            throw new Exception("El Lot Number es obligatorio.");

        if (!string.IsNullOrWhiteSpace(dto.TestResult) && !ValidTestResults.Contains(dto.TestResult, StringComparer.OrdinalIgnoreCase))
            throw new Exception($"El Test Result '{dto.TestResult}' no es válido.");
    }

    private static void ValidateUpdate(UpdateMillTestDto dto)
    {
        if (dto.RollQty <= 0)
            throw new Exception("La Roll Qty debe ser mayor que 0.");

        if (dto.TestDate == default)
            throw new Exception("La Test Date es obligatoria.");

        if (string.IsNullOrWhiteSpace(dto.LotNumber))
            throw new Exception("El Lot Number es obligatorio.");

        if (!string.IsNullOrWhiteSpace(dto.TestResult) && !ValidTestResults.Contains(dto.TestResult, StringComparer.OrdinalIgnoreCase))
            throw new Exception($"El Test Result '{dto.TestResult}' no es válido.");
    }

    private static MillTestDto ToDto(MillTest item)
    {
        return new MillTestDto
        {
            ID = item.ID,
            FabricPOId = item.FabricPOId,
            FabricPONumber = item.FabricPO?.FabricPONumber ?? string.Empty,
            FGPOId = item.FGPOId,
            FGPONumber = item.FGPO?.FGPONumber ?? string.Empty,
            CustomerName = item.FGPO?.Customer?.Name ?? string.Empty,
            Supplier = item.FabricPO?.Supplier?.Name,
            LotNumber = item.LotNumber,
            LotId = item.LotId,
            Color = item.Color,
            RollQty = item.RollQty,
            ActualWidth = item.ActualWidth,
            ActualGSM = item.ActualGSM,
            LengthShrinkagePercentage = item.LengthShrinkagePercentage,
            WidthShrinkagePercentage = item.WidthShrinkagePercentage,
            TorquePercentage = item.TorquePercentage,
            BowingPercentage = item.BowingPercentage,
            SkewingPercentage = item.SkewingPercentage,
            Colorfastness = item.Colorfastness,
            WashAppearance = item.WashAppearance,
            HandFeel = item.HandFeel,
            TestDate = item.TestDate,
            TestedBy = item.TestedBy?.UserName,
            TestResult = item.TestResult,
            ApprovedForExport = item.ApprovedForExport,
            ReportLink = item.ReportLink,
            Comments = item.Comments,
            Active = item.Active,
            CreatedAt = item.CreatedAt,
            UpdatedAt = item.UpdatedAt,
        };
    }
}
