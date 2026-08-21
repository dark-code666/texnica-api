namespace ERP.Api.Dtos.User;

public class UpdateUserDto
{
    public string UserName { get; set; } = null!;
    public string UserEmail { get; set; } = null!;
    public string UserType { get; set; } = "Employee";
    public int? CustomerId { get; set; }
    public int? RoleId { get; set; }
}
