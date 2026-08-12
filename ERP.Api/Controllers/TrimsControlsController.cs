using Microsoft.AspNetCore.Mvc;
using ERP.Api.Dtos;
using ERP.Api.Interfaces;

namespace ERP.Api.Controllers;

[ApiController]
[Route("api/trims-controls")]
public class TrimsControlsController : ControllerBase
{
    private readonly ITrimsControlService _service;

    public TrimsControlsController(ITrimsControlService service) => _service = service;

    [HttpGet]
    public async Task<IActionResult> GetAll() => Ok(await _service.GetAllAsync());

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var item = await _service.GetByIdAsync(id);
        return item is null ? NotFound() : Ok(item);
    }

    [HttpGet("fgpo/{fgpoId}")]
    public async Task<IActionResult> GetByFgpo(int fgpoId) => Ok(await _service.GetByFgpoAsync(fgpoId));

    [HttpPost]
    public async Task<IActionResult> Create(CreateTrimsControlDto dto)
    {
        try { var c = await _service.CreateAsync(dto); return CreatedAtAction(nameof(GetById), new { id = c.ID }, c); }
        catch (Exception ex) { return BadRequest(ex.Message); }
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, UpdateTrimsControlDto dto)
    {
        try { var ok = await _service.UpdateAsync(id, dto); return ok ? NoContent() : NotFound(); }
        catch (Exception ex) { return BadRequest(ex.Message); }
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var ok = await _service.DeleteAsync(id);
        return ok ? NoContent() : NotFound();
    }

    [HttpGet("paged")]
    public async Task<IActionResult> GetPaged(
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 10,
        [FromQuery] string? search = null,
        [FromQuery] string? sortBy = null,
        [FromQuery] string? sortOrder = null,
        [FromQuery] string? fgpo = null,
        [FromQuery] string? status = null)
    {
        return Ok(await _service.GetPagedAsync(page, pageSize, search, sortBy, sortOrder, fgpo, status));
    }
}
