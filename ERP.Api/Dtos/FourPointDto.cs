namespace ERP.Api.Dtos;

public class FourPointDto
{
    public int ID { get; set; }
    public DateTime InspectionDate { get; set; }
    public int? ReceivingId { get; set; }
    public string ReceivingNumber { get; set; } = string.Empty;
    public int FabricPOId { get; set; }
    public string FabricPONumber { get; set; } = null!;
    public int FGPOId { get; set; }
    public string FGPONumber { get; set; } = null!;
    public string CustomerName { get; set; } = null!;
    public string? Supplier { get; set; }
    public string? LotNumber { get; set; }
    public int? LotId { get; set; }
    public string? RollNumber { get; set; }
    public decimal Width { get; set; }
    public decimal InspectedLength { get; set; }
    public int Points1 { get; set; }
    public int Points2 { get; set; }
    public int Points3 { get; set; }
    public int Points4 { get; set; }
    public int TotalPoints { get; set; }
    public decimal PointsPer100SqYd { get; set; }
    public decimal MaxAllowed { get; set; }
    public int AcceptedQty { get; set; }
    public int RejectedQty { get; set; }
    public int HoldQty { get; set; }
    public string? Result { get; set; }
    public string? Inspector { get; set; }
    public string? ReportLink { get; set; }
    public string? Comments { get; set; }
    public bool Active { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}
