namespace ERP.Api.Dtos;

public class RegisterUserDto
{
    public string UserName { get; set; } = null!;
    public string UserEmail { get; set; } = null!;
    // Password en claro (compatibilidad) o el descifrado de EncryptedPassword.
    // Nullable: cuando el cliente cifra, este campo no se envía.
    public string? Password { get; set; }
    // Password cifrado en el navegador (RSA-OAEP). Si viene, se descifra en el servidor.
    public string? EncryptedPassword { get; set; }
}

public class LoginUserDto
{
    public string UserName { get; set; } = null!;
    public int CustomerId { get; set; }
    // Password en claro (compatibilidad) o el descifrado de EncryptedPassword.
    public string? Password { get; set; }
    // Password cifrado en el navegador (RSA-OAEP). Si viene, se descifra en el servidor.
    public string? EncryptedPassword { get; set; }
}

public class UserDto
{
    public int ID { get; set; }
    public string UserName { get; set; } = null!;
    public string UserEmail { get; set; } = null!;
    public bool Active { get; set; }
    public bool MustChangePassword { get; set; }
    public int? CustomerId { get; set; }
    public string? CustomerName { get; set; }
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
