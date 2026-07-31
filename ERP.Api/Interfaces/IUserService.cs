namespace ERP.Api.Interfaces;

using System.Threading.Tasks;
using ERP.Api.Dtos;

public interface IUserService
{
    Task<AuthResponseDto> RegisterAsync(RegisterUserDto registerDto);
    Task<AuthResponseDto> LoginAsync(LoginUserDto loginDto);
}
