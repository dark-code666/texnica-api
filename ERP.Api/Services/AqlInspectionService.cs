using ERP.Api.Data;
using ERP.Api.Domain;
using ERP.Api.Dtos;
using ERP.Api.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ERP.Api.Services;

public class AqlInspectionService : IAqlInspectionService
{
    private readonly ErpDbContext _context;

    public AqlInspectionService(ErpDbContext context)
    {
        _context = context;
    }

    public async Task<PagedResultDto<AqlInspectionDto>> GetAllAsync(int page, int pageSize, string? type, string? search)
    {
        var query = _context.AqlInspections
            .Include(a => a.FGPO)
            .Include(a => a.Inspector)
            .Where(a => a.Active);

        if (!string.IsNullOrWhiteSpace(type))
        {
            query = query.Where(a => a.InspectionType == type);
        }

        if (!string.IsNullOrWhiteSpace(search))
        {
            query = query.Where(a =>
                a.InspectionType.Contains(search) ||
                (a.FGPO != null && a.FGPO.FGPONumber.Contains(search)) ||
                (a.LotShipment != null && a.LotShipment.Contains(search)) ||
                (a.Result != null && a.Result.Contains(search)));
        }

        var totalItems = await query.CountAsync();
        var items = await query
            .OrderByDescending(a => a.InspectionDate)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .Select(a => MapToDto(a))
            .ToListAsync();

        return new PagedResultDto<AqlInspectionDto>
        {
            Items = items,
            TotalCount = totalItems,
            Page = page,
            PageSize = pageSize,
            TotalPages = (int)Math.Ceiling(totalItems / (double)pageSize)
        };
    }

    public async Task<AqlInspectionDto?> GetByIdAsync(int id)
    {
        var entity = await _context.AqlInspections
            .Include(a => a.FGPO)
            .Include(a => a.Inspector)
            .FirstOrDefaultAsync(a => a.ID == id && a.Active);

        return entity == null ? null : MapToDto(entity);
    }

    public async Task<AqlInspectionDto> CreateAsync(CreateAqlInspectionDto dto)
    {
        var entity = new AqlInspection
        {
            InspectionType = dto.InspectionType,
            InspectionDate = dto.InspectionDate,
            FGPOId = dto.FGPOId,
            LotShipment = dto.LotShipment,
            LotSize = dto.LotSize,
            InspectionLevel = dto.InspectionLevel,
            AqlMajor = dto.AqlMajor,
            AqlMinor = dto.AqlMinor,
            SampleSize = dto.SampleSize,
            CriticalDefects = dto.CriticalDefects,
            MajorDefects = dto.MajorDefects,
            MinorDefects = dto.MinorDefects,
            CriticalAc = dto.CriticalAc,
            MajorAc = dto.MajorAc,
            MinorAc = dto.MinorAc,
            CriticalRe = dto.CriticalRe,
            MajorRe = dto.MajorRe,
            MinorRe = dto.MinorRe,
            InspectorId = dto.InspectorId,
            Disposition = dto.Disposition,
            ReportLink = dto.ReportLink,
            Comments = dto.Comments,
            Active = true,
            CreatedAt = DateTime.UtcNow
        };

        _context.AqlInspections.Add(entity);
        await _context.SaveChangesAsync();

        return (await GetByIdAsync(entity.ID))!;
    }

    public async Task<AqlInspectionDto?> UpdateAsync(int id, UpdateAqlInspectionDto dto)
    {
        var entity = await _context.AqlInspections.FirstOrDefaultAsync(a => a.ID == id && a.Active);
        if (entity == null) return null;

        entity.InspectionType = dto.InspectionType;
        entity.InspectionDate = dto.InspectionDate;
        entity.FGPOId = dto.FGPOId;
        entity.LotShipment = dto.LotShipment;
        entity.LotSize = dto.LotSize;
        entity.InspectionLevel = dto.InspectionLevel;
        entity.AqlMajor = dto.AqlMajor;
        entity.AqlMinor = dto.AqlMinor;
        entity.SampleSize = dto.SampleSize;
        entity.CriticalDefects = dto.CriticalDefects;
        entity.MajorDefects = dto.MajorDefects;
        entity.MinorDefects = dto.MinorDefects;
        entity.CriticalAc = dto.CriticalAc;
        entity.MajorAc = dto.MajorAc;
        entity.MinorAc = dto.MinorAc;
        entity.CriticalRe = dto.CriticalRe;
        entity.MajorRe = dto.MajorRe;
        entity.MinorRe = dto.MinorRe;
        entity.InspectorId = dto.InspectorId;
        entity.Disposition = dto.Disposition;
        entity.ReportLink = dto.ReportLink;
        entity.Comments = dto.Comments;
        entity.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync();
        return await GetByIdAsync(id);
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var entity = await _context.AqlInspections.FirstOrDefaultAsync(a => a.ID == id && a.Active);
        if (entity == null) return false;

        entity.Active = false;
        entity.UpdatedAt = DateTime.UtcNow;
        await _context.SaveChangesAsync();
        return true;
    }

    private static AqlInspectionDto MapToDto(AqlInspection a)
    {
        return new AqlInspectionDto
        {
            ID = a.ID,
            InspectionType = a.InspectionType,
            InspectionDate = a.InspectionDate,
            FGPOId = a.FGPOId,
            FgpoNumber = a.FGPO?.FGPONumber,
            LotShipment = a.LotShipment,
            LotSize = a.LotSize,
            InspectionLevel = a.InspectionLevel,
            AqlMajor = a.AqlMajor,
            AqlMinor = a.AqlMinor,
            SampleSize = a.SampleSize,
            CriticalDefects = a.CriticalDefects,
            MajorDefects = a.MajorDefects,
            MinorDefects = a.MinorDefects,
            CriticalAc = a.CriticalAc,
            MajorAc = a.MajorAc,
            MinorAc = a.MinorAc,
            CriticalRe = a.CriticalRe,
            MajorRe = a.MajorRe,
            MinorRe = a.MinorRe,
            Result = a.Result,
            InspectorId = a.InspectorId,
            InspectorName = a.Inspector?.UserName,
            Disposition = a.Disposition,
            ReportLink = a.ReportLink,
            Comments = a.Comments,
            Active = a.Active,
            CreatedAt = a.CreatedAt,
            UpdatedAt = a.UpdatedAt
        };
    }
}
