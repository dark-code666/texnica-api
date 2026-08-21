namespace ERP.Api.Domain;

public class User : BaseEntity
{
    public string UserName { get; set; } = null!;
    public string UserEmail { get; set; } = null!;
    public string Password { get; set; } = null!;
    public int? RoleId { get; set; }
    public string UserType { get; set; } = "Employee";
    public int? CustomerId { get; set; }
    public bool MustChangePassword { get; set; } = true;
    
    // Navigation properties
    public Role? Role { get; set; }
    public Customer? Customer { get; set; }
}
