using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ERP.Api.Data;
using ERP.Api.Dtos;

namespace ERP.Api.Controllers;

[ApiController]
[Route("api/lots")]
public class LotsController : ControllerBase
{
    private readonly ErpDbContext _context;

    public LotsController(ErpDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var lots = await _context.Lots
            .Include(l => l.FabricPO)
            .Include(l => l.FGPO)
            .Where(l => l.Active)
            .OrderBy(l => l.LotNumber)
            .ToListAsync();
        return Ok(lots.Select(ToDto));
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var lot = await _context.Lots
            .Include(l => l.FabricPO)
            .Include(l => l.FGPO)
            .FirstOrDefaultAsync(l => l.ID == id && l.Active);
        return lot is null ? NotFound() : Ok(ToDto(lot));
    }

    private static LotDto ToDto(Domain.Lot l) => new()
    {
        ID = l.ID,
        LotNumber = l.LotNumber,
        FabricPOId = l.FabricPOId,
        FabricPONumber = l.FabricPO?.FabricPONumber ?? string.Empty,
        FGPOId = l.FGPOId,
        FGPONumber = l.FGPO?.FGPONumber ?? string.Empty,
        ProducedQuantity = l.ProducedQuantity,
        Active = l.Active,
        CreatedAt = l.CreatedAt,
        UpdatedAt = l.UpdatedAt,
    };
}
