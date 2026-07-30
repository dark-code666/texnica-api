namespace ERP.Api.Dtos;

public class CreateProductDto
{
    public string Name { get; set; } = null!;
    public string? Description { get; set; }
    public decimal Price { get; set; }
}
