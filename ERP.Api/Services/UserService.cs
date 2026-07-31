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
using ERP.Api.Interfaces;
using BCrypt.Net;

namespace ERP.Api.Services;

public class UserService : IUserService
{
    private readonly ErpDbContext _context;
    private readonly IConfiguration _configuration;

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

        // Crear el nuevo usuario
        var user = new User
        {
            UserName = registerDto.UserName,
            UserEmail = registerDto.UserEmail,
            Password = BCrypt.Net.BCrypt.HashPassword(registerDto.Password),
            Active = true
        };

        _context.Users.Add(user);
        await _context.SaveChangesAsync();

        // Generar Token
        var token = GenerateJwtToken(user);

        return new AuthResponseDto
        {
            Token = token,
            User = new UserDto
            {
                ID = user.ID,
                UserName = user.UserName,
                UserEmail = user.UserEmail,
                Active = user.Active
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

        // Generar Token
        var token = GenerateJwtToken(user);

        return new AuthResponseDto
        {
            Token = token,
            User = new UserDto
            {
                ID = user.ID,
                UserName = user.UserName,
                UserEmail = user.UserEmail,
                Active = user.Active
            }
        };
    }

    private string GenerateJwtToken(User user)
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
            new Claim("active", user.Active.ToString())
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
}
