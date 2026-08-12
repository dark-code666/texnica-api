namespace ERP.Api.Dtos;

public class UpdateFgpoLineDto
{
    public int FgpoId { get; set; }
    public int StyleId { get; set; }
    public int ColorId { get; set; }
    public int SizeId { get; set; }
    public int Quantity { get; set; }
    public decimal? UnitPrice { get; set; }
}
