using UniScore.API.DTOs;

namespace UniScore.API.Services
{
    public interface IAuthService
    {
        Task<LoginResponseDto> LoginAsync(LoginRequestDto request);
        Task<UserDto> CreateUserAsync(CreateUserDto request);
        Task<UserDto> GetUserByIdAsync(int id);
        Task<List<UserDto>> GetAllUsersAsync();
        Task<List<UserDto>> GetAvaliadoresAsync();
        Task<bool> ValidateUserAsync(string usuario, string senha);
    }
}