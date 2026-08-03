namespace ERP.Api.Dtos.User
{
    public class UserDto
    {
        public int ID { get; set; }
        public string UserName { get; set; } = null!;
        public string UserEmail { get; set; } = null!;
        public bool Active { get; set; }
        public bool MustChangePassword { get; set; }
        public int? RoleId { get; set; }
        public string? RoleName { get; set; }
    }
}
