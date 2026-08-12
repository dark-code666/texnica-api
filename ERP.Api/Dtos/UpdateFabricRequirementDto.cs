namespace ERP.Api.Dtos;

public class UpdateFabricRequirementDto
{
    public int FGPOId { get; set; }
    // Style/Color se mantienen como texto descriptivo de especificaciones técnicas de la tela
    public string? Style { get; set; }
    public string? Color { get; set; }
    // FabricComponent normalizado: FK a Components
    public int? ComponentId { get; set; }
    public string? FabricDescription { get; set; }
    public string? Composition { get; set; }
    public decimal GSM { get; set; }
    public string? RequiredWidth { get; set; }
    public string? UOM { get; set; }
    public decimal OrderQuantity { get; set; }
    public decimal ApprovedYield { get; set; }
    public decimal AllowancePercentage { get; set; }
    public decimal AvailableInventory { get; set; }
    public DateTime RequiredDate { get; set; }
    public string? Status { get; set; }
    public int? DataOwnerId { get; set; }
    public string? Remarks { get; set; }
}
