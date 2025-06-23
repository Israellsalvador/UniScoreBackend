using ProjectEvaluationApi.DTOs;

namespace ProjectEvaluationApi.Services
{
    public interface IAuthService
    {
        Task<LoginResponseDto?> LoginAsync(LoginDto loginDto);
        Task<bool> RegisterAsync(RegisterDto registerDto);
        string GenerateJwtToken(int idPessoa, string nomePessoa, List<string> papeis);
        string HashPassword(string password);
        bool VerifyPassword(string password, string hashedPassword);
    }
}
