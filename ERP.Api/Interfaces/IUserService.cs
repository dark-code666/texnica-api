namespace ERP.Api.Interfaces;

using System.Collections.Generic;
using System.Threading.Tasks;
using ERP.Api.Dtos;
using ERP.Api.Dtos.User;

public interface IUserService
{
    Task<AuthResponseDto> RegisterAsync(RegisterUserDto registerDto);
    Task<AuthResponseDto> CreateUserAsync(RegisterUserDto registerDto);
    Task<AuthResponseDto> LoginAsync(LoginUserDto loginDto);
    Task<bool> AssignRoleToUserAsync(int userId, int roleId);
    Task<bool> ChangePasswordAsync(int userId, ChangePasswordDto changePasswordDto);
    Task<List<ERP.Api.Dtos.User.UserDto>> GetAllUsersAsync();
    Task<(string UserType, int? CustomerId, string? CustomerName)> GetLoginProfileAsync(string userName);
    Task<bool> UpdateUserAsync(int userId, UpdateUserDto dto);
    Task<bool> ResetPasswordAsync(int userId, string? newPassword);
    Task<bool> SetUserActiveAsync(int userId, bool active);

}

