using Microsoft.EntityFrameworkCore;
using ERP.Api.Data;
using ERP.Api.Dtos;
using ERP.Api.Domain;
using ERP.Api.Interfaces;

namespace ERP.Api.Services;

public class FactoryService : IFactoryService
{
    private readonly ErpDbContext _context;

    public FactoryService(ErpDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<FactoryDto>> GetAllAsync()
    {
        var factories = await _context.Factories
            .Where(f => f.Active)
            .OrderBy(f => f.Name)
            .ToListAsync();

        return factories.Select(ToDto);
    }

    public async Task<FactoryDto?> GetByIdAsync(int id)
    {
        var factory = await _context.Factories
            .FirstOrDefaultAsync(f => f.ID == id && f.Active);

        return factory is null ? null : ToDto(factory);
    }

    public async Task<FactoryDto> CreateAsync(CreateFactoryDto dto)
    {
        Validate(dto.Name);

        // Validar que el Name sea único
        var exists = await _context.Factories.AnyAsync(f => f.Name == dto.Name);
        if (exists)
            throw new Exception("El Factory ya existe.");

        var entity = new Factory
        {
            Name = dto.Name,
            Location = dto.Location,
            Contact = dto.Contact,
            Phone = dto.Phone,
            Email = dto.Email,
            Active = true,
            CreatedAt = DateTime.UtcNow,
        };

        _context.Factories.Add(entity);
        await _context.SaveChangesAsync();

        return ToDto(entity);
    }

    public async Task<bool> UpdateAsync(int id, UpdateFactoryDto dto)
    {
        var entity = await _context.Factories
            .FirstOrDefaultAsync(f => f.ID == id && f.Active);

        if (entity is null)
        {
            return false;
        }

        Validate(dto.Name);

        // Validar que el Name sea único (excluyendo el registro actual)
        var exists = await _context.Factories.AnyAsync(f => f.Name == dto.Name && f.ID != id);
        if (exists)
            throw new Exception("El Factory ya existe.");

        entity.Name = dto.Name;
        entity.Location = dto.Location;
        entity.Contact = dto.Contact;
        entity.Phone = dto.Phone;
        entity.Email = dto.Email;
        entity.UpdatedAt = DateTime.UtcNow;

        _context.Factories.Update(entity);
        await _context.SaveChangesAsync();

        return true;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var entity = await _context.Factories
            .FirstOrDefaultAsync(f => f.ID == id && f.Active);

        if (entity is null)
        {
            return false;
        }

        // Soft Delete: se desactiva el registro en lugar de eliminarlo físicamente
        entity.Active = false;
        entity.UpdatedAt = DateTime.UtcNow;

        _context.Factories.Update(entity);
        await _context.SaveChangesAsync();

        return true;
    }

    public async Task<IEnumerable<FactoryDto>> SearchAsync(string? term)
    {
        var query = _context.Factories.Where(f => f.Active);

        if (!string.IsNullOrWhiteSpace(term))
        {
            var searchTerm = term.Trim();
            query = query.Where(f =>
                f.Name.Contains(searchTerm) ||
                (f.Location != null && f.Location.Contains(searchTerm)) ||
                (f.Contact != null && f.Contact.Contains(searchTerm)) ||
                (f.Phone != null && f.Phone.Contains(searchTerm)));
        }

        var factories = await query
            .OrderBy(f => f.Name)
            .ToListAsync();

        return factories.Select(ToDto);
    }

    public async Task<PagedResultDto<FactoryDto>> GetPagedAsync(int page, int pageSize, string? search, string? sortBy, string? sortOrder)
    {
        if (page < 1) page = 1;
        if (pageSize < 1) pageSize = 10;
        if (pageSize > 100) pageSize = 100;

        var query = _context.Factories.Where(f => f.Active);

        // Búsqueda
        if (!string.IsNullOrWhiteSpace(search))
        {
            var searchTerm = search.Trim();
            query = query.Where(f =>
                f.Name.Contains(searchTerm) ||
                (f.Location != null && f.Location.Contains(searchTerm)) ||
                (f.Contact != null && f.Contact.Contains(searchTerm)) ||
                (f.Phone != null && f.Phone.Contains(searchTerm)));
        }

        // Ordenamiento
        var sortByLower = sortBy?.ToLowerInvariant();
        var sortOrderLower = sortOrder?.ToLowerInvariant();
        var descending = sortOrderLower == "desc";

        query = (sortByLower, descending) switch
        {
            ("name", false) => query.OrderBy(f => f.Name),
            ("name", true) => query.OrderByDescending(f => f.Name),
            ("location", false) => query.OrderBy(f => f.Location),
            ("location", true) => query.OrderByDescending(f => f.Location),
            ("createdat", false) => query.OrderBy(f => f.CreatedAt),
            ("createdat", true) => query.OrderByDescending(f => f.CreatedAt),
            _ => query.OrderBy(f => f.Name),
        };

        var totalCount = await query.CountAsync();
        var totalPages = (int)Math.Ceiling(totalCount / (double)pageSize);

        var items = await query
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return new PagedResultDto<FactoryDto>
        {
            Items = items.Select(ToDto),
            TotalCount = totalCount,
            Page = page,
            PageSize = pageSize,
            TotalPages = totalPages,
        };
    }

    private static void Validate(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new Exception("El Name es obligatorio.");
    }

    private static FactoryDto ToDto(Factory factory)
    {
        return new FactoryDto
        {
            ID = factory.ID,
            Name = factory.Name,
            Location = factory.Location,
            Contact = factory.Contact,
            Phone = factory.Phone,
            Email = factory.Email,
            Active = factory.Active,
            CreatedAt = factory.CreatedAt,
            UpdatedAt = factory.UpdatedAt,
        };
    }
}
