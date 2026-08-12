namespace ERP.Api.Dtos;

public class CreatePriceDto
{
    public int StyleId { get; set; }
    public int ColorId { get; set; }
    public int SizeId { get; set; }
    public string? Sku { get; set; }
    public decimal UnitPrice { get; set; }
    public string? Comments { get; set; }
}
