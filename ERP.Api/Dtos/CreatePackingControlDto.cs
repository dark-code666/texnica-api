namespace ERP.Api.Dtos;

public class CreatePackingControlDto
{
    public DateTime PackingDate { get; set; }
    public int FGPOId { get; set; }
    public decimal QcPassedQty { get; set; }
    public decimal ReceivedByPackingQty { get; set; }
    public decimal FoldedQty { get; set; }
    public decimal PolybaggedQty { get; set; }
    public decimal PackedQty { get; set; }
    public int FullCartons { get; set; }
    public int PartialCartons { get; set; }
    public int PcsPerCarton { get; set; }
    public int? ResponsiblePersonId { get; set; }
    public DateTime? LastUpdated { get; set; }
    public string? Remarks { get; set; }
}
