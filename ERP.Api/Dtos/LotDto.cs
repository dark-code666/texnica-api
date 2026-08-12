namespace ERP.Api.Dtos;

public class LotDto
{
    public int ID { get; set; }
    public string LotNumber { get; set; } = null!;
    public int FabricPOId { get; set; }
    public string FabricPONumber { get; set; } = string.Empty;
    public int FGPOId { get; set; }
    public string FGPONumber { get; set; } = string.Empty;
    public decimal ProducedQuantity { get; set; }
    public bool Active { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}
