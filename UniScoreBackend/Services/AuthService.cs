using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using UniScore.API.DTOs;
using UniScore.API.Models;
using UniScore.API.Repositories;

namespace UniScore.API.Services
{
    public class AuthService : IAuthService
    {
        private readonly IPessoaRepository _pessoaRepository;
        private readonly IConfiguration _configuration;

        public AuthService(IPessoaRepository pessoaRepository, IConfiguration configuration)
        {
            _pessoaRepository = pessoaRepository;
            _configuration = configuration;
        }

        public async Task<LoginResponseDto> LoginAsync(LoginRequestDto request)
        {
            var pessoa = await _pessoaRepository.GetByUsuarioAsync(request.Usuario);
            
            if (pessoa == null || !VerifyPassword(request.Senha, pessoa.Senha))
            {
                throw new UnauthorizedAccessException("Usuário ou senha inválidos");
            }

            var papeis = await _pessoaRepository.GetPapeisByPessoaIdAsync(pessoa.IdPessoa);
            var token = GenerateJwtToken(pessoa, papeis);
            
            return new LoginResponseDto
            {
                Token = token,
                NomePessoa = pessoa.NomePessoa,
                Usuario = pessoa.Usuario,
                Papeis = papeis,
                ExpiresAt = DateTime.UtcNow.AddHours(8)
            };
        }

        public async Task<UserDto> CreateUserAsync(CreateUserDto request)
        {
            // Verificar se usuário já existe
            var existingUser = await _pessoaRepository.GetByUsuarioAsync(request.Usuario);
            if (existingUser != null)
            {
                throw new ArgumentException("Usuário já existe");
            }

            var pessoa = new Pessoa
            {
                NomePessoa = request.NomePessoa,
                Usuario = request.Usuario,
                Senha = HashPassword(request.Senha)
            };

            var createdPessoa = await _pessoaRepository.CreateAsync(pessoa);
            
            // Associar tipo de usuário
            await _pessoaRepository.AssociateTipoUsuarioAsync(createdPessoa.IdPessoa, request.IdTipoUsuario);

            var papeis = await _pessoaRepository.GetPapeisByPessoaIdAsync(createdPessoa.IdPessoa);

            return new UserDto
            {
                IdPessoa = createdPessoa.IdPessoa,
                NomePessoa = createdPessoa.NomePessoa,
                Usuario = createdPessoa.Usuario,
                Papeis = papeis
            };
        }

        public async Task<UserDto> GetUserByIdAsync(int id)
        {
            var pessoa = await _pessoaRepository.GetByIdAsync(id);
            if (pessoa == null)
            {
                throw new ArgumentException("Usuário não encontrado");
            }

            var papeis = await _pessoaRepository.GetPapeisByPessoaIdAsync(pessoa.IdPessoa);

            return new UserDto
            {
                IdPessoa = pessoa.IdPessoa,
                NomePessoa = pessoa.NomePessoa,
                Usuario = pessoa.Usuario,
                Papeis = papeis
            };
        }

        public async Task<List<UserDto>> GetAllUsersAsync()
        {
            var pessoas = await _pessoaRepository.GetAllAsync();
            var users = new List<UserDto>();

            foreach (var pessoa in pessoas)
            {
                var papeis = await _pessoaRepository.GetPapeisByPessoaIdAsync(pessoa.IdPessoa);
                users.Add(new UserDto
                {
                    IdPessoa = pessoa.IdPessoa,
                    NomePessoa = pessoa.NomePessoa,
                    Usuario = pessoa.Usuario,
                    Papeis = papeis
                });
            }

            return users;
        }

        public async Task<List<UserDto>> GetAvaliadoresAsync()
        {
            var avaliadores = await _pessoaRepository.GetAvaliadoresAsync();
            var users = new List<UserDto>();

            foreach (var pessoa in avaliadores)
            {
                var papeis = await _pessoaRepository.GetPapeisByPessoaIdAsync(pessoa.IdPessoa);
                users.Add(new UserDto
                {
                    IdPessoa = pessoa.IdPessoa,
                    NomePessoa = pessoa.NomePessoa,
                    Usuario = pessoa.Usuario,
                    Papeis = papeis
                });
            }

            return users;
        }

        public async Task<bool> ValidateUserAsync(string usuario, string senha)
        {
            var pessoa = await _pessoaRepository.GetByUsuarioAsync(usuario);
            return pessoa != null && VerifyPassword(senha, pessoa.Senha);
        }

        private string GenerateJwtToken(Pessoa pessoa, List<string> papeis)
        {
            var jwtSettings = _configuration.GetSection("JwtSettings");
            var secretKey = jwtSettings["SecretKey"];
            var issuer = jwtSettings["Issuer"];
            var audience = jwtSettings["Audience"];

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, pessoa.IdPessoa.ToString()),
                new Claim(ClaimTypes.Name, pessoa.Usuario),
                new Claim("NomePessoa", pessoa.NomePessoa)
            };

            foreach (var papel in papeis)
            {
                claims.Add(new Claim(ClaimTypes.Role, papel));
            }

            var token = new JwtSecurityToken(
                issuer: issuer,
                audience: audience,
                claims: claims,
                expires: DateTime.UtcNow.AddHours(8),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        private string HashPassword(string password)
        {
            using var sha256 = SHA256.Create();
            var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
            return Convert.ToBase64String(hashedBytes);
        }

        private bool VerifyPassword(string password, string hashedPassword)
        {
            var hashedInput = HashPassword(password);
            return hashedInput == hashedPassword;
        }
    }
}