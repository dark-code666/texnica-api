using Microsoft.AspNetCore.Mvc;
using ERP.Api.Dtos;
using ERP.Api.Interfaces;

namespace ERP.Api.Controllers;

[ApiController]
[Route("api/mill-productions")]
public class MillProductionsController : ControllerBase
{
    private readonly IMillProductionService _millProductionService;

    public MillProductionsController(IMillProductionService millProductionService)
    {
        _millProductionService = millProductionService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var items = await _millProductionService.GetAllAsync();
        return Ok(items);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var item = await _millProductionService.GetByIdAsync(id);
        if (item is null)
            return NotFound();

        return Ok(item);
    }

    [HttpGet("fabric-po/{fabricPOId}")]
    public async Task<IActionResult> GetByFabricPO(int fabricPOId)
    {
        var items = await _millProductionService.GetByFabricPOAsync(fabricPOId);
        return Ok(items);
    }

    [HttpGet("fgpo/{fgpoId}")]
    public async Task<IActionResult> GetByFgpo(int fgpoId)
    {
        var items = await _millProductionService.GetByFgpoAsync(fgpoId);
        return Ok(items);
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreateMillProductionDto dto)
    {
        try
        {
            var created = await _millProductionService.CreateAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = created.ID }, created);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, UpdateMillProductionDto dto)
    {
        try
        {
            var updated = await _millProductionService.UpdateAsync(id, dto);
            if (!updated)
                return NotFound();

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
        var deleted = await _millProductionService.DeleteAsync(id);
        if (!deleted)
            return NotFound();

        return NoContent();
    }

    [HttpGet("paged")]
    public async Task<IActionResult> GetPaged(
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 10,
        [FromQuery] string? search = null,
        [FromQuery] string? sortBy = null,
        [FromQuery] string? sortOrder = null,
        [FromQuery] string? fabricPO = null,
        [FromQuery] string? fgpo = null,
        [FromQuery] string? supplier = null,
        [FromQuery] string? status = null)
    {
        var result = await _millProductionService.GetPagedAsync(page, pageSize, search, sortBy, sortOrder, fabricPO, fgpo, supplier, status);
        return Ok(result);
    }
}
