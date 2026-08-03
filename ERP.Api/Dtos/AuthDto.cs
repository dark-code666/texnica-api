namespace ERP.Api.Dtos;

public class RegisterUserDto
{
    public string UserName { get; set; } = null!;
    public string UserEmail { get; set; } = null!;
    public string Password { get; set; } = null!;
}

public class LoginUserDto
{
    public string UserName { get; set; } = null!;
    public string Password { get; set; } = null!;
}

public class UserDto
{
    public int ID { get; set; }
    public string UserName { get; set; } = null!;
    public string UserEmail { get; set; } = null!;
    public bool Active { get; set; }
    public bool MustChangePassword { get; set; }
}

public class AuthResponseDto
{
    public string Token { get; set; } = null!;
    public UserDto User { get; set; } = null!;
}

public class ChangePasswordDto
{
    public string CurrentPassword { get; set; } = null!;
    public string NewPassword { get; set; } = null!;
}
