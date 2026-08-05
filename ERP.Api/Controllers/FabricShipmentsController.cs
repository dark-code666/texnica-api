using Microsoft.AspNetCore.Mvc;
using ERP.Api.Dtos;
using ERP.Api.Interfaces;

namespace ERP.Api.Controllers;

[ApiController]
[Route("api/fabric-shipments")]
public class FabricShipmentsController : ControllerBase
{
    private readonly IFabricShipmentService _fabricShipmentService;

    public FabricShipmentsController(IFabricShipmentService fabricShipmentService)
    {
        _fabricShipmentService = fabricShipmentService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var items = await _fabricShipmentService.GetAllAsync();
        return Ok(items);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var item = await _fabricShipmentService.GetByIdAsync(id);
        if (item is null)
            return NotFound();

        return Ok(item);
    }

    [HttpGet("fabric-po/{fabricPOId}")]
    public async Task<IActionResult> GetByFabricPO(int fabricPOId)
    {
        var items = await _fabricShipmentService.GetByFabricPOAsync(fabricPOId);
        return Ok(items);
    }

    [HttpGet("fgpo/{fgpoId}")]
    public async Task<IActionResult> GetByFgpo(int fgpoId)
    {
        var items = await _fabricShipmentService.GetByFgpoAsync(fgpoId);
        return Ok(items);
    }

    [HttpGet("lot/{lotNumber}")]
    public async Task<IActionResult> GetByLot(string lotNumber)
    {
        var items = await _fabricShipmentService.GetByLotAsync(lotNumber);
        return Ok(items);
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreateFabricShipmentDto dto)
    {
        try
        {
            var created = await _fabricShipmentService.CreateAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = created.ID }, created);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, UpdateFabricShipmentDto dto)
    {
        try
        {
            var updated = await _fabricShipmentService.UpdateAsync(id, dto);
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
        var deleted = await _fabricShipmentService.DeleteAsync(id);
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
        [FromQuery] string? lotNumber = null,
        [FromQuery] string? status = null)
    {
        var result = await _fabricShipmentService.GetPagedAsync(page, pageSize, search, sortBy, sortOrder, fabricPO, fgpo, lotNumber, status);
        return Ok(result);
    }
}
