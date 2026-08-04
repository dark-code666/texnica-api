namespace ERP.Api.Dtos;

public class UpdateFgpoDto
{
    public string? FGPONumber { get; set; }
    public string? TemporaryNumber { get; set; }
    public string? Status { get; set; }
    public int CustomerId { get; set; }
    public string? Style { get; set; }
    public string? Color { get; set; }
    public int OrderQuantity { get; set; }
    public DateTime DeliveryDate { get; set; }
    public decimal InTransitQty { get; set; }
    public decimal ReceivedQty { get; set; }
    public decimal TotalShippedQty { get; set; }
    public decimal ProducedQty { get; set; }
    public string? DataOwner { get; set; }
    public string? Remarks { get; set; }
}
