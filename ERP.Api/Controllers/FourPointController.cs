using Microsoft.AspNetCore.Mvc;
using ERP.Api.Dtos;
using ERP.Api.Interfaces;

namespace ERP.Api.Controllers;

[ApiController]
[Route("api/four-point")]
public class FourPointController : ControllerBase
{
    private readonly IFourPointService _service;

    public FourPointController(IFourPointService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        return Ok(await _service.GetAllAsync());
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var item = await _service.GetByIdAsync(id);
        return item is null ? NotFound() : Ok(item);
    }

    [HttpGet("fabric-po/{fabricPOId}")]
    public async Task<IActionResult> GetByFabricPO(int fabricPOId)
    {
        return Ok(await _service.GetByFabricPOAsync(fabricPOId));
    }

    [HttpGet("fgpo/{fgpoId}")]
    public async Task<IActionResult> GetByFgpo(int fgpoId)
    {
        return Ok(await _service.GetByFgpoAsync(fgpoId));
    }

    [HttpGet("receiving/{receivingId}")]
    public async Task<IActionResult> GetByReceiving(int receivingId)
    {
        return Ok(await _service.GetByReceivingAsync(receivingId));
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreateFourPointDto dto)
    {
        try
        {
            var created = await _service.CreateAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = created.ID }, created);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, UpdateFourPointDto dto)
    {
        try
        {
            var updated = await _service.UpdateAsync(id, dto);
            return updated ? NoContent() : NotFound();
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var deleted = await _service.DeleteAsync(id);
        return deleted ? NoContent() : NotFound();
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
        [FromQuery] string? result = null)
    {
        return Ok(await _service.GetPagedAsync(page, pageSize, search, sortBy, sortOrder, fabricPO, fgpo, result));
    }
}
