using Microsoft.AspNetCore.Mvc;
using ERP.Api.Dtos;
using ERP.Api.Interfaces;

namespace ERP.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class FgpoController : ControllerBase
{
    private readonly IFgpoService _fgpoService;

    public FgpoController(IFgpoService fgpoService)
    {
        _fgpoService = fgpoService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var fgpos = await _fgpoService.GetAllAsync();
        return Ok(fgpos);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var fgpo = await _fgpoService.GetByIdAsync(id);
        if (fgpo is null)
        {
            return NotFound();
        }

        return Ok(fgpo);
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreateFgpoDto dto)
    {
        try
        {
            var created = await _fgpoService.CreateAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = created.ID }, created);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, UpdateFgpoDto dto)
    {
        try
        {
            var updated = await _fgpoService.UpdateAsync(id, dto);
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
        var deleted = await _fgpoService.DeleteAsync(id);
        if (!deleted)
        {
            return NotFound();
        }

        return NoContent();
    }

    [HttpGet("search")]
    public async Task<IActionResult> Search([FromQuery] string? term)
    {
        var fgpos = await _fgpoService.SearchAsync(term);
        return Ok(fgpos);
    }

    [HttpGet("paged")]
    public async Task<IActionResult> GetPaged(
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 10,
        [FromQuery] string? search = null,
        [FromQuery] string? sortBy = null,
        [FromQuery] string? sortOrder = null,
        [FromQuery] string? status = null,
        [FromQuery] string? customer = null)
    {
        var result = await _fgpoService.GetPagedAsync(page, pageSize, search, sortBy, sortOrder, status, customer);
        return Ok(result);
    }
}
