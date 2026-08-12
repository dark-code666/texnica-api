namespace ERP.Api.Dtos;

public class FgpoLineDto
{
    public int ID { get; set; }
    public int FgpoId { get; set; }
    public string FgpoNumber { get; set; } = null!;
    public string CustomerName { get; set; } = null!;
    public int StyleId { get; set; }
    public string StyleCode { get; set; } = null!;
    public int ColorId { get; set; }
    public string ColorName { get; set; } = null!;
    public int SizeId { get; set; }
    public string SizeCode { get; set; } = null!;
    public int Quantity { get; set; }
    public decimal? UnitPrice { get; set; }
    public decimal TotalValue { get; set; }
    public bool Active { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}
