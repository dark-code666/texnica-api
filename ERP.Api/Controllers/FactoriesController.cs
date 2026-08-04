using Microsoft.AspNetCore.Mvc;
using ERP.Api.Dtos;
using ERP.Api.Interfaces;

namespace ERP.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class FactoriesController : ControllerBase
{
    private readonly IFactoryService _factoryService;

    public FactoriesController(IFactoryService factoryService)
    {
        _factoryService = factoryService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var factories = await _factoryService.GetAllAsync();
        return Ok(factories);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var factory = await _factoryService.GetByIdAsync(id);
        if (factory is null)
        {
            return NotFound();
        }

        return Ok(factory);
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreateFactoryDto dto)
    {
        try
        {
            var created = await _factoryService.CreateAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = created.ID }, created);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, UpdateFactoryDto dto)
    {
        try
        {
            var updated = await _factoryService.UpdateAsync(id, dto);
            if (!updated)
            {
                return NotFound();
            }

            return NoContent();
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var deleted = await _factoryService.DeleteAsync(id);
        if (!deleted)
        {
            return NotFound();
        }

        return NoContent();
    }

    [HttpGet("search")]
    public async Task<IActionResult> Search([FromQuery] string? term)
    {
        var factories = await _factoryService.SearchAsync(term);
        return Ok(factories);
    }

    [HttpGet("paged")]
    public async Task<IActionResult> GetPaged(
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 10,
        [FromQuery] string? search = null,
        [FromQuery] string? sortBy = null,
        [FromQuery] string? sortOrder = null)
    {
        var result = await _factoryService.GetPagedAsync(page, pageSize, search, sortBy, sortOrder);
        return Ok(result);
    }
}
