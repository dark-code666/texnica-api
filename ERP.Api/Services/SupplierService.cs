using Microsoft.EntityFrameworkCore;
using ERP.Api.Data;
using ERP.Api.Dtos;
using ERP.Api.Domain;
using ERP.Api.Interfaces;

namespace ERP.Api.Services;

public class SupplierService : ISupplierService
{
    private readonly ErpDbContext _context;

    public SupplierService(ErpDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<SupplierDto>> GetAllAsync()
    {
        var suppliers = await _context.Suppliers
            .Where(s => s.Active)
            .OrderBy(s => s.Name)
            .ToListAsync();

        return suppliers.Select(ToDto);
    }

    public async Task<SupplierDto?> GetByIdAsync(int id)
    {
        var supplier = await _context.Suppliers
            .FirstOrDefaultAsync(s => s.ID == id && s.Active);

        return supplier is null ? null : ToDto(supplier);
    }

    public async Task<SupplierDto> CreateAsync(CreateSupplierDto dto)
    {
        Validate(dto.Name);

        // Validar que el Name sea único
        var exists = await _context.Suppliers.AnyAsync(s => s.Name == dto.Name);
        if (exists)
            throw new Exception("El Supplier ya existe.");

        var entity = new Supplier
        {
            Name = dto.Name,
            SupplierCode = dto.SupplierCode,
            Category = dto.Category,
            Contact = dto.Contact,
            Phone = dto.Phone,
            Email = dto.Email,
            Address = dto.Address,
            Remarks = dto.Remarks,
            Active = true,
            CreatedAt = DateTime.UtcNow,
        };

        _context.Suppliers.Add(entity);
        await _context.SaveChangesAsync();

        return ToDto(entity);
    }

    public async Task<bool> UpdateAsync(int id, UpdateSupplierDto dto)
    {
        var entity = await _context.Suppliers
            .FirstOrDefaultAsync(s => s.ID == id && s.Active);

        if (entity is null)
        {
            return false;
        }

        Validate(dto.Name);

        // Validar que el Name sea único (excluyendo el registro actual)
        var exists = await _context.Suppliers.AnyAsync(s => s.Name == dto.Name && s.ID != id);
        if (exists)
            throw new Exception("El Supplier ya existe.");

        entity.Name = dto.Name;
        entity.SupplierCode = dto.SupplierCode;
        entity.Category = dto.Category;
        entity.Contact = dto.Contact;
        entity.Phone = dto.Phone;
        entity.Email = dto.Email;
        entity.Address = dto.Address;
        entity.Remarks = dto.Remarks;
        entity.UpdatedAt = DateTime.UtcNow;

        _context.Suppliers.Update(entity);
        await _context.SaveChangesAsync();

        return true;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var entity = await _context.Suppliers
            .FirstOrDefaultAsync(s => s.ID == id && s.Active);

        if (entity is null)
        {
            return false;
        }

        // Soft Delete: se desactiva el registro en lugar de eliminarlo físicamente
        entity.Active = false;
        entity.UpdatedAt = DateTime.UtcNow;

        _context.Suppliers.Update(entity);
        await _context.SaveChangesAsync();

        return true;
    }

    public async Task<IEnumerable<SupplierDto>> SearchAsync(string? term)
    {
        var query = _context.Suppliers.Where(s => s.Active);

        if (!string.IsNullOrWhiteSpace(term))
        {
            var searchTerm = term.Trim();
            query = query.Where(s =>
                s.Name.Contains(searchTerm) ||
                (s.SupplierCode != null && s.SupplierCode.Contains(searchTerm)) ||
                (s.Category != null && s.Category.Contains(searchTerm)) ||
                (s.Contact != null && s.Contact.Contains(searchTerm)) ||
                (s.Phone != null && s.Phone.Contains(searchTerm)));
        }

        var suppliers = await query
            .OrderBy(s => s.Name)
            .ToListAsync();

        return suppliers.Select(ToDto);
    }

    public async Task<PagedResultDto<SupplierDto>> GetPagedAsync(int page, int pageSize, string? search, string? sortBy, string? sortOrder)
    {
        if (page < 1) page = 1;
        if (pageSize < 1) pageSize = 10;
        if (pageSize > 100) pageSize = 100;

        var query = _context.Suppliers.Where(s => s.Active);

        // Búsqueda
        if (!string.IsNullOrWhiteSpace(search))
        {
            var searchTerm = search.Trim();
            query = query.Where(s =>
                s.Name.Contains(searchTerm) ||
                (s.SupplierCode != null && s.SupplierCode.Contains(searchTerm)) ||
                (s.Category != null && s.Category.Contains(searchTerm)) ||
                (s.Contact != null && s.Contact.Contains(searchTerm)) ||
                (s.Phone != null && s.Phone.Contains(searchTerm)) ||
                (s.Email != null && s.Email.Contains(searchTerm)));
        }

        // Ordenamiento
        var sortByLower = sortBy?.ToLowerInvariant();
        var sortOrderLower = sortOrder?.ToLowerInvariant();
        var descending = sortOrderLower == "desc";

        query = (sortByLower, descending) switch
        {
            ("name", false) => query.OrderBy(s => s.Name),
            ("name", true) => query.OrderByDescending(s => s.Name),
            ("suppliercode", false) => query.OrderBy(s => s.SupplierCode),
            ("suppliercode", true) => query.OrderByDescending(s => s.SupplierCode),
            ("category", false) => query.OrderBy(s => s.Category),
            ("category", true) => query.OrderByDescending(s => s.Category),
            ("createdat", false) => query.OrderBy(s => s.CreatedAt),
            ("createdat", true) => query.OrderByDescending(s => s.CreatedAt),
            _ => query.OrderBy(s => s.Name),
        };

        var totalCount = await query.CountAsync();
        var totalPages = (int)Math.Ceiling(totalCount / (double)pageSize);

        var items = await query
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return new PagedResultDto<SupplierDto>
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

    private static SupplierDto ToDto(Supplier supplier)
    {
        return new SupplierDto
        {
            ID = supplier.ID,
            Name = supplier.Name,
            SupplierCode = supplier.SupplierCode,
            Category = supplier.Category,
            Contact = supplier.Contact,
            Phone = supplier.Phone,
            Email = supplier.Email,
            Address = supplier.Address,
            Remarks = supplier.Remarks,
            Active = supplier.Active,
            CreatedAt = supplier.CreatedAt,
            UpdatedAt = supplier.UpdatedAt,
        };
    }
}
