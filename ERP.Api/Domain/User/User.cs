namespace ERP.Api.Domain;

public class User : BaseEntity
{
    public string UserName { get; set; } = null!;
    public string UserEmail { get; set; } = null!;
    public string Password { get; set; } = null!;
}
