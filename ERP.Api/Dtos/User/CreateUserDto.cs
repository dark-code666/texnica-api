namespace ERP.Api.Dtos.User
{
    public class CreateUserDto
    {
        public string UserName { get; set; } = null!;
        public string UserEmail { get; set; } = null!;
        public string Password { get; set; } = null!;

    }
}
