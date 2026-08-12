using Microsoft.EntityFrameworkCore;
using ERP.Api.Data;
using ERP.Api.Dtos;
using ERP.Api.Domain;
using ERP.Api.Interfaces;

namespace ERP.Api.Services;

public class ComponentService : IComponentService
{
    private readonly ErpDbContext _context;

    public ComponentService(ErpDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<ComponentDto>> GetAllAsync()
    {
        var items = await _context.Components
            .Where(c => c.Active)
            .OrderBy(c => c.ComponentCode)
            .ToListAsync();
        return items.Select(ToDto);
    }

    public async Task<ComponentDto?> GetByIdAsync(int id)
    {
        var item = await _context.Components.FirstOrDefaultAsync(c => c.ID == id && c.Active);
        return item is null ? null : ToDto(item);
    }

    public async Task<ComponentDto> CreateAsync(CreateComponentDto dto)
    {
        if (string.IsNullOrWhiteSpace(dto.ComponentCode))
            throw new Exception("El Component Code es obligatorio.");

        var exists = await _context.Components.AnyAsync(c => c.ComponentCode == dto.ComponentCode);
        if (exists)
            throw new Exception("El Component ya existe.");

        var entity = new Component
        {
            ComponentCode = dto.ComponentCode.Trim(),
            Description = dto.Description,
            Active = true,
            CreatedAt = DateTime.UtcNow,
        };

        _context.Components.Add(entity);
        await _context.SaveChangesAsync();
        return ToDto(entity);
    }

    public async Task<bool> UpdateAsync(int id, UpdateComponentDto dto)
    {
        var entity = await _context.Components.FirstOrDefaultAsync(c => c.ID == id && c.Active);
        if (entity is null)
            return false;

        if (string.IsNullOrWhiteSpace(dto.ComponentCode))
            throw new Exception("El Component Code es obligatorio.");

        var exists = await _context.Components.AnyAsync(c => c.ComponentCode == dto.ComponentCode && c.ID != id);
        if (exists)
            throw new Exception("El Component ya existe.");

        entity.ComponentCode = dto.ComponentCode.Trim();
        entity.Description = dto.Description;
        entity.UpdatedAt = DateTime.UtcNow;

        _context.Components.Update(entity);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var entity = await _context.Components.FirstOrDefaultAsync(c => c.ID == id && c.Active);
        if (entity is null)
            return false;

        entity.Active = false;
        entity.UpdatedAt = DateTime.UtcNow;
        _context.Components.Update(entity);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<IEnumerable<ComponentDto>> SearchAsync(string? term)
    {
        var query = _context.Components.Where(c => c.Active);
        if (!string.IsNullOrWhiteSpace(term))
        {
            var t = term.Trim();
            query = query.Where(c => c.ComponentCode.Contains(t) || (c.Description != null && c.Description.Contains(t)));
        }
        var items = await query.OrderBy(c => c.ComponentCode).ToListAsync();
        return items.Select(ToDto);
    }

    private static ComponentDto ToDto(Component item) => new()
    {
        ID = item.ID,
        ComponentCode = item.ComponentCode,
        Description = item.Description,
        Active = item.Active,
        CreatedAt = item.CreatedAt,
        UpdatedAt = item.UpdatedAt,
    };
}
