using Microsoft.EntityFrameworkCore;
using ERP.Api.Data;
using ERP.Api.Interfaces;

namespace ERP.Api.Services;

public class CatalogService : ICatalogService
{
    private readonly ErpDbContext _context;

    public CatalogService(ErpDbContext context)
    {
        _context = context;
    }

    public async Task<Dictionary<string, List<string>>> GetAllAsync()
    {
        var items = await _context.CatalogValues
            .Where(c => c.Active)
            .OrderBy(c => c.Type).ThenBy(c => c.Value)
            .ToListAsync();

        return items
            .GroupBy(c => c.Type)
            .ToDictionary(g => g.Key, g => g.Select(c => c.Value).ToList());
    }

    public async Task<IEnumerable<string>> GetByTypeAsync(string type)
    {
        return await _context.CatalogValues
            .Where(c => c.Active && c.Type == type)
            .OrderBy(c => c.Value)
            .Select(c => c.Value)
            .ToListAsync();
    }
}
