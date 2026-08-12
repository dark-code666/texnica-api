namespace ERP.Api.Dtos;

public class UpdateStyleYieldDto
{
    public int StyleId { get; set; }
    public int ComponentId { get; set; }
    public decimal? YieldQuoted { get; set; }
    public decimal? YieldReal { get; set; }
    public string? Notes { get; set; }
}
