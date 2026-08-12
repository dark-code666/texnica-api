using Microsoft.AspNetCore.Mvc;
using ERP.Api.Dtos;
using ERP.Api.Interfaces;

namespace ERP.Api.Controllers;

[ApiController]
[Route("api/fgpo-lines")]
public class FgpoLinesController : ControllerBase
{
    private readonly IFgpoLineService _service;

    public FgpoLinesController(IFgpoLineService service) => _service = service;

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
    public async Task<IActionResult> Create(CreateFgpoLineDto dto)
    {
        try { var c = await _service.CreateAsync(dto); return CreatedAtAction(nameof(GetById), new { id = c.ID }, c); }
        catch (Exception ex) { return BadRequest(ex.Message); }
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, UpdateFgpoLineDto dto)
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
}
