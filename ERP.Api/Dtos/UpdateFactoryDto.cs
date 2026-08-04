namespace ERP.Api.Dtos;

public class UpdateFactoryDto
{
    public string Name { get; set; } = null!;
    public string? Location { get; set; }
    public string? Contact { get; set; }
    public string? Phone { get; set; }
    public string? Email { get; set; }
}
