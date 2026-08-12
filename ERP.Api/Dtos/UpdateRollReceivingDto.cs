namespace ERP.Api.Dtos;

public class UpdateRollReceivingDto
{
    public int ReceivingId { get; set; }
    public string? LotNumber { get; set; }
    public string? RollNumber { get; set; }
    public string? SupplierRollNumber { get; set; }
    public decimal GrossWeight { get; set; }
    public decimal NetWeight { get; set; }
    public decimal ActualYardage { get; set; }
    public decimal ActualWidth { get; set; }
    public decimal ActualGSM { get; set; }
    public string? ShadeGroup { get; set; }
    public decimal DamagedQty { get; set; }
    public string? Condition { get; set; }
    public string? WarehouseLocation { get; set; }
    public DateTime ReceivedDate { get; set; }
    public int? DataOwnerId { get; set; }
    public string? Comments { get; set; }
}
