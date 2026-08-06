using Microsoft.AspNetCore.Mvc;
using ERP.Api.Dtos;
using ERP.Api.Interfaces;

namespace ERP.Api.Controllers;

[ApiController]
[Route("api/fabric-receivings")]
public class FabricReceivingsController : ControllerBase
{
    private readonly IFabricReceivingService _fabricReceivingService;

    public FabricReceivingsController(IFabricReceivingService fabricReceivingService)
    {
        _fabricReceivingService = fabricReceivingService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var items = await _fabricReceivingService.GetAllAsync();
        return Ok(items);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var item = await _fabricReceivingService.GetByIdAsync(id);
        if (item is null)
            return NotFound();

        return Ok(item);
    }

    [HttpGet("fabric-po/{fabricPOId}")]
    public async Task<IActionResult> GetByFabricPO(int fabricPOId)
    {
        var items = await _fabricReceivingService.GetByFabricPOAsync(fabricPOId);
        return Ok(items);
    }

    [HttpGet("fgpo/{fgpoId}")]
    public async Task<IActionResult> GetByFgpo(int fgpoId)
    {
        var items = await _fabricReceivingService.GetByFgpoAsync(fgpoId);
        return Ok(items);
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreateFabricReceivingDto dto)
    {
        try
        {
            var created = await _fabricReceivingService.CreateAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = created.ID }, created);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, UpdateFabricReceivingDto dto)
    {
        try
        {
            var updated = await _fabricReceivingService.UpdateAsync(id, dto);
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
        var deleted = await _fabricReceivingService.DeleteAsync(id);
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
        [FromQuery] string? status = null)
    {
        var result = await _fabricReceivingService.GetPagedAsync(page, pageSize, search, sortBy, sortOrder, fabricPO, fgpo, status);
        return Ok(result);
    }
}
