using Microsoft.AspNetCore.Mvc;
using ERP.Api.Interfaces;

namespace ERP.Api.Controllers;

[ApiController]
[Route("api/catalogs")]
public class CatalogsController : ControllerBase
{
    private readonly ICatalogService _catalogService;

    public CatalogsController(ICatalogService catalogService)
    {
        _catalogService = catalogService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var catalogs = await _catalogService.GetAllAsync();
        return Ok(catalogs);
    }

    [HttpGet("{type}")]
    public async Task<IActionResult> GetByType(string type)
    {
        var items = await _catalogService.GetByTypeAsync(type);
        return Ok(items);
    }
}
