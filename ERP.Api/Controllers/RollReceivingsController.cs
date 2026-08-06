using Microsoft.AspNetCore.Mvc;
using ERP.Api.Dtos;
using ERP.Api.Interfaces;

namespace ERP.Api.Controllers;

[ApiController]
[Route("api/roll-receivings")]
public class RollReceivingsController : ControllerBase
{
    private readonly IRollReceivingService _rollReceivingService;

    public RollReceivingsController(IRollReceivingService rollReceivingService)
    {
        _rollReceivingService = rollReceivingService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var items = await _rollReceivingService.GetAllAsync();
        return Ok(items);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var item = await _rollReceivingService.GetByIdAsync(id);
        if (item is null)
            return NotFound();

        return Ok(item);
    }

    [HttpGet("receiving/{receivingId}")]
    public async Task<IActionResult> GetByReceiving(int receivingId)
    {
        var items = await _rollReceivingService.GetByReceivingAsync(receivingId);
        return Ok(items);
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreateRollReceivingDto dto)
    {
        try
        {
            var created = await _rollReceivingService.CreateAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = created.ID }, created);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, UpdateRollReceivingDto dto)
    {
        try
        {
            var updated = await _rollReceivingService.UpdateAsync(id, dto);
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
        var deleted = await _rollReceivingService.DeleteAsync(id);
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
        [FromQuery] string? receiving = null,
        [FromQuery] string? fabricPO = null,
        [FromQuery] string? lotNumber = null)
    {
        var result = await _rollReceivingService.GetPagedAsync(page, pageSize, search, sortBy, sortOrder, receiving, fabricPO, lotNumber);
        return Ok(result);
    }
}
