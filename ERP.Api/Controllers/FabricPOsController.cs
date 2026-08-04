using Microsoft.AspNetCore.Mvc;
using ERP.Api.Dtos;
using ERP.Api.Interfaces;

namespace ERP.Api.Controllers;

[ApiController]
[Route("api/fabric-pos")]
public class FabricPOsController : ControllerBase
{
    private readonly IFabricPOService _fabricPOService;

    public FabricPOsController(IFabricPOService fabricPOService)
    {
        _fabricPOService = fabricPOService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var items = await _fabricPOService.GetAllAsync();
        return Ok(items);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var item = await _fabricPOService.GetByIdAsync(id);
        if (item is null)
        {
            return NotFound();
        }

        return Ok(item);
    }

    [HttpGet("fgpo/{fgpoId}")]
    public async Task<IActionResult> GetByFgpo(int fgpoId)
    {
        var items = await _fabricPOService.GetByFgpoAsync(fgpoId);
        return Ok(items);
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreateFabricPODto dto)
    {
        try
        {
            var created = await _fabricPOService.CreateAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = created.ID }, created);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, UpdateFabricPODto dto)
    {
        try
        {
            var updated = await _fabricPOService.UpdateAsync(id, dto);
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
        var deleted = await _fabricPOService.DeleteAsync(id);
        if (!deleted)
        {
            return NotFound();
        }

        return NoContent();
    }

    [HttpGet("paged")]
    public async Task<IActionResult> GetPaged(
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 10,
        [FromQuery] string? search = null,
        [FromQuery] string? sortBy = null,
        [FromQuery] string? sortOrder = null,
        [FromQuery] string? fgpo = null,
        [FromQuery] string? supplier = null,
        [FromQuery] string? fabricMill = null,
        [FromQuery] string? fabricComponent = null,
        [FromQuery] string? poStatus = null)
    {
        var result = await _fabricPOService.GetPagedAsync(page, pageSize, search, sortBy, sortOrder, fgpo, supplier, fabricMill, fabricComponent, poStatus);
        return Ok(result);
    }
}
