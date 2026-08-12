namespace ERP.Api.Dtos;

public class CreateTrimsControlDto
{
    public int FGPOId { get; set; }
    public string? TrimType { get; set; }
    public string? Description { get; set; }
    public int? SupplierId { get; set; }
    public string? Uom { get; set; }
    public decimal ConsumptionPerGarment { get; set; }
    public decimal RequiredQty { get; set; }
    public decimal OrderedQty { get; set; }
    public decimal ReceivedQty { get; set; }
    public decimal ApprovedQty { get; set; }
    public decimal RejectedQty { get; set; }
    public decimal ReservedQty { get; set; }
    public decimal IssuedQty { get; set; }
    public DateTime? Eta { get; set; }
    public string? DevelopmentStatus { get; set; }
    public string? ApprovalStatus { get; set; }
    public string? DataOwner { get; set; }
    public string? Comments { get; set; }
}
