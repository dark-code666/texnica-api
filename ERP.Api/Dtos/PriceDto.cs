namespace ERP.Api.Dtos;

public class PriceDto
{
    public int ID { get; set; }
    public int StyleId { get; set; }
    public string StyleCode { get; set; } = null!;
    public int ColorId { get; set; }
    public string ColorName { get; set; } = null!;
    public int SizeId { get; set; }
    public string SizeCode { get; set; } = null!;
    public string? Sku { get; set; }
    public decimal UnitPrice { get; set; }
    public string? Comments { get; set; }
    public bool Active { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}
