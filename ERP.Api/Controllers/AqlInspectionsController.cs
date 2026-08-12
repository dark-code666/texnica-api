using ERP.Api.Dtos;
using ERP.Api.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ERP.Api.Controllers;

[ApiController]
[Route("api/aql-inspections")]
public class AqlInspectionsController : ControllerBase
{
    private readonly IAqlInspectionService _service;

    public AqlInspectionsController(IAqlInspectionService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<ActionResult<PagedResultDto<AqlInspectionDto>>> GetAll(
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 10,
        [FromQuery] string? type = null,
        [FromQuery] string? search = null)
    {
        var result = await _service.GetAllAsync(page, pageSize, type, search);
        return Ok(result);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<AqlInspectionDto>> GetById(int id)
    {
        var result = await _service.GetByIdAsync(id);
        if (result == null) return NotFound();
        return Ok(result);
    }

    [HttpPost]
    public async Task<ActionResult<AqlInspectionDto>> Create([FromBody] CreateAqlInspectionDto dto)
    {
        var result = await _service.CreateAsync(dto);
        return CreatedAtAction(nameof(GetById), new { id = result.ID }, result);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<AqlInspectionDto>> Update(int id, [FromBody] UpdateAqlInspectionDto dto)
    {
        var result = await _service.UpdateAsync(id, dto);
        if (result == null) return NotFound();
        return Ok(result);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var result = await _service.DeleteAsync(id);
        if (!result) return NotFound();
        return NoContent();
    }
}
