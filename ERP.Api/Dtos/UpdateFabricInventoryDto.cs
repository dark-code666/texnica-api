namespace ERP.Api.Dtos;

public class UpdateFabricInventoryDto
{
    public int FabricPOId { get; set; }
    public int FGPOId { get; set; }
    public int? LotId { get; set; }
    public decimal ReceivedQuantity { get; set; }
    public decimal ApprovedQuantity { get; set; }
    public decimal RejectedQuantity { get; set; }
    public decimal HoldQuantity { get; set; }
    public decimal ReservedQuantity { get; set; }
    public decimal IssuedQuantity { get; set; }
    public decimal ReturnedQuantity { get; set; }
    public decimal ShortageQuantity { get; set; }
    public string? WarehouseLocation { get; set; }
    public string? InventoryStatus { get; set; }
    public int? DataOwnerId { get; set; }
    public DateTime? LastUpdated { get; set; }
    public string? Remarks { get; set; }
}
