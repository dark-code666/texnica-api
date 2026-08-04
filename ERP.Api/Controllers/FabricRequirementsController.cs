using Microsoft.AspNetCore.Mvc;
using ERP.Api.Dtos;
using ERP.Api.Interfaces;

namespace ERP.Api.Controllers;

[ApiController]
[Route("api/fabric-requirements")]
public class FabricRequirementsController : ControllerBase
{
    private readonly IFabricRequirementService _fabricRequirementService;

    public FabricRequirementsController(IFabricRequirementService fabricRequirementService)
    {
        _fabricRequirementService = fabricRequirementService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var items = await _fabricRequirementService.GetAllAsync();
        return Ok(items);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var item = await _fabricRequirementService.GetByIdAsync(id);
        if (item is null)
        {
            return NotFound();
        }

        return Ok(item);
    }

    [HttpGet("fgpo/{fgpoId}")]
    public async Task<IActionResult> GetByFgpo(int fgpoId)
    {
        var items = await _fabricRequirementService.GetByFgpoAsync(fgpoId);
        return Ok(items);
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreateFabricRequirementDto dto)
    {
        try
        {
            var created = await _fabricRequirementService.CreateAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = created.ID }, created);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, UpdateFabricRequirementDto dto)
    {
        try
        {
            var updated = await _fabricRequirementService.UpdateAsync(id, dto);
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
        var deleted = await _fabricRequirementService.DeleteAsync(id);
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
        [FromQuery] string? customer = null,
        [FromQuery] string? style = null,
        [FromQuery] string? fabricComponent = null,
        [FromQuery] string? status = null)
    {
        var result = await _fabricRequirementService.GetPagedAsync(page, pageSize, search, sortBy, sortOrder, fgpo, customer, style, fabricComponent, status);
        return Ok(result);
    }
}
