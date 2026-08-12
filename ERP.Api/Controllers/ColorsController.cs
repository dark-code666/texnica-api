using Microsoft.AspNetCore.Mvc;
using ERP.Api.Dtos;
using ERP.Api.Interfaces;

namespace ERP.Api.Controllers;

[ApiController]
[Route("api/colors")]
public class ColorsController : ControllerBase
{
    private readonly IColorService _service;

    public ColorsController(IColorService service) => _service = service;

    [HttpGet]
    public async Task<IActionResult> GetAll() => Ok(await _service.GetAllAsync());

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var item = await _service.GetByIdAsync(id);
        return item is null ? NotFound() : Ok(item);
    }

    [HttpGet("search")]
    public async Task<IActionResult> Search([FromQuery] string? term) => Ok(await _service.SearchAsync(term));

    [HttpPost]
    public async Task<IActionResult> Create(CreateColorDto dto)
    {
        try { var c = await _service.CreateAsync(dto); return CreatedAtAction(nameof(GetById), new { id = c.ID }, c); }
        catch (Exception ex) { return BadRequest(ex.Message); }
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, UpdateColorDto dto)
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
