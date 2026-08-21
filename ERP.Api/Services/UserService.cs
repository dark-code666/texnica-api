using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using ERP.Api.Data;
using ERP.Api.Domain;
using ERP.Api.Dtos;
using ERP.Api.Dtos.User;
using ERP.Api.Interfaces;
using BCrypt.Net;


namespace ERP.Api.Services;

public class UserService : IUserService
{
    private readonly ErpDbContext _context;
    private readonly IConfiguration _configuration;

    // Contraseña por defecto asignada a los nuevos usuarios
    private const string DefaultPassword = "inicio";

    public UserService(ErpDbContext context, IConfiguration configuration)
    {
        _context = context;
        _configuration = configuration;
    }

    public async Task<AuthResponseDto> RegisterAsync(RegisterUserDto registerDto)
    {
        // Verificar si el correo ya está registrado
        var userExists = await _context.Users.AnyAsync(u => u.UserEmail == registerDto.UserEmail);
        if (userExists)
            throw new Exception("El correo electrónico ya está registrado.");

        // Auto-registro: el usuario crea SU propia contraseña
        if (string.IsNullOrWhiteSpace(registerDto.Password) || registerDto.Password.Length < 6)
            throw new Exception("La contraseña debe tener al menos 6 caracteres.");

        var user = new User
        {
            UserName = registerDto.UserName,
            UserEmail = registerDto.UserEmail,
            Password = BCrypt.Net.BCrypt.HashPassword(registerDto.Password),
            Active = true,
            MustChangePassword = false
        };

        _context.Users.Add(user);
        await _context.SaveChangesAsync();

        // Generar Token
        var token = GenerateJwtToken(user);

        return new AuthResponseDto
        {
            Token = token,
            User = new ERP.Api.Dtos.UserDto
            {
                ID = user.ID,
                UserName = user.UserName,
                UserEmail = user.UserEmail,
                Active = user.Active,
                MustChangePassword = user.MustChangePassword
            }
        };
    }

    /// <summary>
    /// Creación desde el sistema (admin): se asigna la contraseña por defecto (inicio)
    /// y se fuerza el cambio en el primer login.
    /// </summary>
    public async Task<AuthResponseDto> CreateUserAsync(RegisterUserDto registerDto)
    {
        var userExists = await _context.Users.AnyAsync(u => u.UserEmail == registerDto.UserEmail);
        if (userExists)
            throw new Exception("El correo electrónico ya está registrado.");

        var userType = NormalizeUserType(registerDto.UserType);
        var customer = await ResolveAssignedCustomerAsync(userType, registerDto.CustomerId);
        var user = new User
        {
            UserName = registerDto.UserName,
            UserEmail = registerDto.UserEmail,
            Password = BCrypt.Net.BCrypt.HashPassword(DefaultPassword),
            Active = true,
            MustChangePassword = true
            ,UserType = userType
            ,CustomerId = customer?.ID
        };

        _context.Users.Add(user);
        await _context.SaveChangesAsync();

        var token = GenerateJwtToken(user);

        return new AuthResponseDto
        {
            Token = token,
            User = new ERP.Api.Dtos.UserDto
            {
                ID = user.ID,
                UserName = user.UserName,
                UserEmail = user.UserEmail,
                Active = user.Active,
                MustChangePassword = user.MustChangePassword
                ,CustomerId = user.CustomerId
                ,CustomerName = customer?.Name
                ,UserType = user.UserType
            }
        };
    }

    public async Task<AuthResponseDto> LoginAsync(LoginUserDto loginDto)

    {
        // Buscar usuario por nombre de usuario
        var user = await _context.Users.FirstOrDefaultAsync(u => u.UserName == loginDto.UserName);
        if (user == null || !user.Active)
            throw new Exception("Credenciales incorrectas o usuario inactivo.");

        // Verificar la contraseña
        bool isPasswordValid = BCrypt.Net.BCrypt.Verify(loginDto.Password, user.Password);
        if (!isPasswordValid)
            throw new Exception("Credenciales incorrectas o usuario inactivo.");

        var userType = NormalizeUserType(user.UserType);
        var customerId = userType == "Client" ? user.CustomerId : loginDto.CustomerId;
        if (!customerId.HasValue || customerId.Value <= 0)
            throw new Exception(userType == "Client" ? "El usuario cliente no tiene un cliente asignado." : "Selecciona un cliente para continuar.");

        var customer = await _context.Customers
            .FirstOrDefaultAsync(c => c.ID == customerId.Value && c.Active);
        if (customer is null)
            throw new Exception("El cliente seleccionado no es válido.");

        // Generar Token
        var token = GenerateJwtToken(user, customer.ID);

        return new AuthResponseDto
        {
            Token = token,
            User = new ERP.Api.Dtos.UserDto
            {
                ID = user.ID,
                UserName = user.UserName,
                UserEmail = user.UserEmail,
                Active = user.Active,
                MustChangePassword = user.MustChangePassword,
                CustomerId = customer.ID,
                CustomerName = customer.Name
                ,UserType = userType
            }
        };
    }

    public async Task<bool> ChangePasswordAsync(int userId, ChangePasswordDto changePasswordDto)

    {
        var user = await _context.Users.FindAsync(userId);
        if (user == null)
            return false;

        // Si el usuario debe cambiar la contraseña (primer inicio de sesión),
        // no se valida la contraseña actual.
        if (!user.MustChangePassword)
        {
            // Verificar la contraseña actual
            bool isCurrentPasswordValid = BCrypt.Net.BCrypt.Verify(changePasswordDto.CurrentPassword, user.Password);
            if (!isCurrentPasswordValid)
                throw new Exception("La contraseña actual es incorrecta.");
        }

        // Validar que la nueva contraseña no sea la contraseña por defecto
        if (changePasswordDto.NewPassword == DefaultPassword)
            throw new Exception("La nueva contraseña no puede ser la contraseña por defecto.");

        // Actualizar la contraseña
        user.Password = BCrypt.Net.BCrypt.HashPassword(changePasswordDto.NewPassword);
        user.MustChangePassword = false;
        await _context.SaveChangesAsync();
        return true;
    }


    private string GenerateJwtToken(User user, int? customerId = null)
    {

        try
        {
            var jwtSecret = Environment.GetEnvironmentVariable("JWT_SECRET")
                ?? _configuration["Jwt:Secret"]
                ?? "DefaultSuperSecretKey2026!ForJwtTokens";

            var expirationHoursStr = Environment.GetEnvironmentVariable("JWT_EXPIRATION_HOURS")
                                     ?? _configuration["Jwt:ExpirationHours"]
                                     ?? "1";

            if (!double.TryParse(expirationHoursStr, out var expirationHours))
            {
                expirationHours = 1;
            }

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSecret));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
            new Claim(JwtRegisteredClaimNames.Sub, user.ID.ToString()),
            new Claim(JwtRegisteredClaimNames.Email, user.UserEmail),
            new Claim(ClaimTypes.Name, user.UserName),
            new Claim("active", user.Active.ToString()),
            new Claim("customer_id", customerId?.ToString() ?? string.Empty)
        };

            var token = new JwtSecurityToken(
                issuer: "ERP.Api",
                audience: "ERP.Client",
                claims: claims,
                expires: DateTime.UtcNow.AddHours(expirationHours),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);



        }
        catch (Exception ex)
        {
            throw new Exception("Error generating JWT token: " + ex.Message);

        }
    }

    public async Task<bool> AssignRoleToUserAsync(int userId, int roleId)
    {
        var user = await _context.Users.FindAsync(userId);
        if (user == null)
            return false;

        var role = await _context.Roles.FindAsync(roleId);
        if (role == null || !role.Active)
            return false;

        user.RoleId = roleId;
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<List<ERP.Api.Dtos.User.UserDto>> GetAllUsersAsync()
    {
        var users = await _context.Users
            .Include(u => u.Role)
            .Include(u => u.Customer)
            .OrderBy(u => u.UserName)
            .ToListAsync();

        return users.Select(u => new ERP.Api.Dtos.User.UserDto
        {
            ID = u.ID,
            UserName = u.UserName,
            UserEmail = u.UserEmail,
            Active = u.Active,
            MustChangePassword = u.MustChangePassword,
            RoleId = u.RoleId,
            RoleName = u.Role?.Name
            ,CustomerId = u.CustomerId
            ,CustomerName = u.Customer?.Name
            ,UserType = u.UserType
        }).ToList();
    }

    public async Task<bool> UpdateUserAsync(int userId, UpdateUserDto dto)
    {
        var user = await _context.Users.FirstOrDefaultAsync(u => u.ID == userId);
        if (user is null) return false;
        if (string.IsNullOrWhiteSpace(dto.UserName) || string.IsNullOrWhiteSpace(dto.UserEmail))
            throw new Exception("El nombre de usuario y el correo son obligatorios.");
        if (await _context.Users.AnyAsync(u => u.ID != userId && (u.UserName == dto.UserName || u.UserEmail == dto.UserEmail)))
            throw new Exception("El usuario o correo ya está registrado.");

        var userType = NormalizeUserType(dto.UserType);
        var customer = await ResolveAssignedCustomerAsync(userType, dto.CustomerId);
        user.UserName = dto.UserName.Trim();
        user.UserEmail = dto.UserEmail.Trim();
        user.UserType = userType;
        user.CustomerId = customer?.ID;
        user.RoleId = dto.RoleId;
        user.UpdatedAt = DateTime.UtcNow;
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> ResetPasswordAsync(int userId, string? newPassword)
    {
        var user = await _context.Users.FirstOrDefaultAsync(u => u.ID == userId);
        if (user is null) return false;
        var password = string.IsNullOrWhiteSpace(newPassword) ? DefaultPassword : newPassword;
        if (password.Length < 6) throw new Exception("La contraseña debe tener al menos 6 caracteres.");
        user.Password = BCrypt.Net.BCrypt.HashPassword(password);
        user.MustChangePassword = true;
        user.UpdatedAt = DateTime.UtcNow;
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> SetUserActiveAsync(int userId, bool active)
    {
        var user = await _context.Users.FirstOrDefaultAsync(u => u.ID == userId);
        if (user is null) return false;
        user.Active = active;
        user.UpdatedAt = DateTime.UtcNow;
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<(string UserType, int? CustomerId, string? CustomerName)> GetLoginProfileAsync(string userName)
    {
        var user = await _context.Users.Include(u => u.Customer)
            .FirstOrDefaultAsync(u => u.UserName == userName && u.Active);
        if (user is null) throw new Exception("Usuario no encontrado.");
        return (NormalizeUserType(user.UserType), user.CustomerId, user.Customer?.Name);
    }

    private static string NormalizeUserType(string? userType) =>
        string.Equals(userType, "Client", StringComparison.OrdinalIgnoreCase) ? "Client" : "Employee";

    private async Task<Customer?> ResolveAssignedCustomerAsync(string userType, int? customerId)
    {
        if (userType == "Employee")
        {
            if (customerId.HasValue)
                throw new Exception("Los usuarios Employee no deben tener un cliente asignado.");
            return null;
        }

        if (!customerId.HasValue)
            throw new Exception("Los usuarios Client requieren un cliente asignado.");
        var customer = await _context.Customers.FirstOrDefaultAsync(c => c.ID == customerId.Value && c.Active);
        if (customer is null) throw new Exception("El cliente asignado no es válido.");
        return customer;
    }

}

