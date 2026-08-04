using Microsoft.EntityFrameworkCore;
using ERP.Api.Data;
using ERP.Api.Dtos;
using ERP.Api.Domain;
using ERP.Api.Interfaces;

namespace ERP.Api.Services;

public class CustomerService : ICustomerService
{
    private readonly ErpDbContext _context;

    public CustomerService(ErpDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<CustomerDto>> GetAllAsync()
    {
        var customers = await _context.Customers
            .Where(c => c.Active)
            .OrderBy(c => c.Name)
            .ToListAsync();

        return customers.Select(ToDto);
    }

    public async Task<CustomerDto?> GetByIdAsync(int id)
    {
        var customer = await _context.Customers
            .FirstOrDefaultAsync(c => c.ID == id && c.Active);

        return customer is null ? null : ToDto(customer);
    }

    public async Task<CustomerDto> CreateAsync(CreateCustomerDto dto)
    {
        Validate(dto.Name);

        // Validar que el Name sea único
        var exists = await _context.Customers.AnyAsync(c => c.Name == dto.Name);
        if (exists)
            throw new Exception("El Customer ya existe.");

        var entity = new Customer
        {
            Name = dto.Name,
            Contact = dto.Contact,
            Phone = dto.Phone,
            Email = dto.Email,
            Address = dto.Address,
            Active = true,
            CreatedAt = DateTime.UtcNow,
        };

        _context.Customers.Add(entity);
        await _context.SaveChangesAsync();

        return ToDto(entity);
    }

    public async Task<bool> UpdateAsync(int id, UpdateCustomerDto dto)
    {
        var entity = await _context.Customers
            .FirstOrDefaultAsync(c => c.ID == id && c.Active);

        if (entity is null)
        {
            return false;
        }

        Validate(dto.Name);

        // Validar que el Name sea único (excluyendo el registro actual)
        var exists = await _context.Customers.AnyAsync(c => c.Name == dto.Name && c.ID != id);
        if (exists)
            throw new Exception("El Customer ya existe.");

        entity.Name = dto.Name;
        entity.Contact = dto.Contact;
        entity.Phone = dto.Phone;
        entity.Email = dto.Email;
        entity.Address = dto.Address;
        entity.UpdatedAt = DateTime.UtcNow;

        _context.Customers.Update(entity);
        await _context.SaveChangesAsync();

        return true;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var entity = await _context.Customers
            .FirstOrDefaultAsync(c => c.ID == id && c.Active);

        if (entity is null)
        {
            return false;
        }

        // Soft Delete: se desactiva el registro en lugar de eliminarlo físicamente
        entity.Active = false;
        entity.UpdatedAt = DateTime.UtcNow;

        _context.Customers.Update(entity);
        await _context.SaveChangesAsync();

        return true;
    }

    public async Task<IEnumerable<CustomerDto>> SearchAsync(string? term)
    {
        var query = _context.Customers.Where(c => c.Active);

        if (!string.IsNullOrWhiteSpace(term))
        {
            var searchTerm = term.Trim();
            query = query.Where(c =>
                c.Name.Contains(searchTerm) ||
                (c.Contact != null && c.Contact.Contains(searchTerm)) ||
                (c.Phone != null && c.Phone.Contains(searchTerm)) ||
                (c.Email != null && c.Email.Contains(searchTerm)));
        }

        var customers = await query
            .OrderBy(c => c.Name)
            .ToListAsync();

        return customers.Select(ToDto);
    }

    public async Task<PagedResultDto<CustomerDto>> GetPagedAsync(int page, int pageSize, string? search, string? sortBy, string? sortOrder)
    {
        if (page < 1) page = 1;
        if (pageSize < 1) pageSize = 10;
        if (pageSize > 100) pageSize = 100;

        var query = _context.Customers.Where(c => c.Active);

        // Búsqueda
        if (!string.IsNullOrWhiteSpace(search))
        {
            var searchTerm = search.Trim();
            query = query.Where(c =>
                c.Name.Contains(searchTerm) ||
                (c.Contact != null && c.Contact.Contains(searchTerm)) ||
                (c.Phone != null && c.Phone.Contains(searchTerm)) ||
                (c.Email != null && c.Email.Contains(searchTerm)));
        }

        // Ordenamiento
        var sortByLower = sortBy?.ToLowerInvariant();
        var sortOrderLower = sortOrder?.ToLowerInvariant();
        var descending = sortOrderLower == "desc";

        query = (sortByLower, descending) switch
        {
            ("name", false) => query.OrderBy(c => c.Name),
            ("name", true) => query.OrderByDescending(c => c.Name),
            ("contact", false) => query.OrderBy(c => c.Contact),
            ("contact", true) => query.OrderByDescending(c => c.Contact),
            ("createdat", false) => query.OrderBy(c => c.CreatedAt),
            ("createdat", true) => query.OrderByDescending(c => c.CreatedAt),
            _ => query.OrderBy(c => c.Name),
        };

        var totalCount = await query.CountAsync();
        var totalPages = (int)Math.Ceiling(totalCount / (double)pageSize);

        var items = await query
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return new PagedResultDto<CustomerDto>
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

    private static CustomerDto ToDto(Customer customer)
    {
        return new CustomerDto
        {
            ID = customer.ID,
            Name = customer.Name,
            Contact = customer.Contact,
            Phone = customer.Phone,
            Email = customer.Email,
            Address = customer.Address,
            Active = customer.Active,
            CreatedAt = customer.CreatedAt,
            UpdatedAt = customer.UpdatedAt,
        };
    }
}
