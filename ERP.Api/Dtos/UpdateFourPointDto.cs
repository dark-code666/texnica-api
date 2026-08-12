namespace ERP.Api.Dtos;

public class UpdateFourPointDto
{
    public DateTime InspectionDate { get; set; }
    public int? ReceivingId { get; set; }
    public int FabricPOId { get; set; }
    public int FGPOId { get; set; }
    public string? LotNumber { get; set; }
    public string? RollNumber { get; set; }
    public decimal Width { get; set; }
    public decimal InspectedLength { get; set; }
    public int Points1 { get; set; }
    public int Points2 { get; set; }
    public int Points3 { get; set; }
    public int Points4 { get; set; }
    public decimal MaxAllowed { get; set; }
    public int AcceptedQty { get; set; }
    public int RejectedQty { get; set; }
    public int HoldQty { get; set; }
    public int? InspectorId { get; set; }
    public string? ReportLink { get; set; }
    public string? Comments { get; set; }
}
