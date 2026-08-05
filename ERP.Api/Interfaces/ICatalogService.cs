using ERP.Api.Dtos;

namespace ERP.Api.Interfaces;

public interface ICatalogService
{
    Task<Dictionary<string, List<string>>> GetAllAsync();
    Task<IEnumerable<string>> GetByTypeAsync(string type);
}
