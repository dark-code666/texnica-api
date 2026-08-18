using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ERP.Api.Dtos;
using ERP.Api.Interfaces;
using ERP.Api.Services;

namespace ERP.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IUserService _userService;
    private readonly CryptoService _crypto;

    public AuthController(IUserService userService, CryptoService crypto)
    {
        _userService = userService;
        _crypto = crypto;
    }

    // Clave pública RSA para que el navegador cifre el password antes de enviarlo
    [AllowAnonymous]
    [HttpGet("public-key")]
    public IActionResult GetPublicKey() => Ok(new { publicKey = _crypto.PublicKeyPem });

    // Descifra el password si el cliente lo envió cifrado (EncryptedPassword)
    private void ResolvePassword(LoginUserDto dto)
    {
        if (!string.IsNullOrWhiteSpace(dto.EncryptedPassword))
            dto.Password = _crypto.DecryptPassword(dto.EncryptedPassword);
    }

    private void ResolvePassword(RegisterUserDto dto)
    {
        if (!string.IsNullOrWhiteSpace(dto.EncryptedPassword))
            dto.Password = _crypto.DecryptPassword(dto.EncryptedPassword);
    }

    [AllowAnonymous]
    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterUserDto dto)
    {
        try
        {
            ResolvePassword(dto);
            var response = await _userService.RegisterAsync(dto);
            return Ok(response);
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    // Creación desde el sistema (admin): asigna la contraseña por defecto (inicio)
    // y fuerza el cambio en el primer login.
    [HttpPost("users")]
    public async Task<IActionResult> CreateUser([FromBody] RegisterUserDto dto)
    {
        try
        {
            ResolvePassword(dto);
            var response = await _userService.CreateUserAsync(dto);
            return Ok(response);
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [AllowAnonymous]
    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginUserDto dto)
    {
        try
        {
            ResolvePassword(dto);
            var response = await _userService.LoginAsync(dto);
            return Ok(response);
        }
        catch (Exception ex)
        {
            return Unauthorized(new { message = ex.Message });
        }
    }

    [HttpPost("assign-role")]
    public async Task<IActionResult> AssignRole([FromBody] AssignRoleDto dto)
    {
        try
        {
            var result = await _userService.AssignRoleToUserAsync(dto.UserId, dto.RoleId);
            if (!result)
                return BadRequest(new { message = "Failed to assign role. User or role not found." });
            return Ok(new { message = "Role assigned successfully" });
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpPost("change-password")]
    public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordDto dto)
    {
        try
        {
            // Obtener el ID del usuario autenticado desde el token JWT
            var userIdClaim = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)
                              ?? User.FindFirst("sub");
            if (userIdClaim == null || !int.TryParse(userIdClaim.Value, out var userId))
                return Unauthorized(new { message = "Usuario no autenticado." });

            var result = await _userService.ChangePasswordAsync(userId, dto);
            if (!result)
                return BadRequest(new { message = "No se pudo cambiar la contraseña." });

            return Ok(new { message = "Contraseña actualizada correctamente." });
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpGet("users")]
    public async Task<IActionResult> GetAllUsers()
    {
        try
        {
            var users = await _userService.GetAllUsersAsync();
            return Ok(users);
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }
}


