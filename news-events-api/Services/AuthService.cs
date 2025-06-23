using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using ProjectEvaluationApi.Data;
using ProjectEvaluationApi.DTOs;
using ProjectEvaluationApi.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace ProjectEvaluationApi.Services
{
    public class AuthService : IAuthService
    {
        private readonly ApplicationDbContext _context;
        private readonly IConfiguration _configuration;
        
        public AuthService(ApplicationDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }
        
        public async Task<LoginResponseDto?> LoginAsync(LoginDto loginDto)
        {
            var pessoa = await _context.Pessoas
                .Include(p => p.PessoaTipoUsuarios)
                .ThenInclude(ptu => ptu.TipoUsuario)
                .FirstOrDefaultAsync(p => p.Usuario == loginDto.Usuario);
                
            if (pessoa == null || !VerifyPassword(loginDto.Senha, pessoa.Senha))
                return null;
                
            var papeis = pessoa.PessoaTipoUsuarios
                .Select(ptu => ptu.TipoUsuario!.PapelUsuario)
                .ToList();
                
            var token = GenerateJwtToken(pessoa.IdPessoa, pessoa.NomePessoa, papeis);
            
            return new LoginResponseDto
            {
                Token = token,
                IdPessoa = pessoa.IdPessoa,
                NomePessoa = pessoa.NomePessoa,
                Papeis = papeis
            };
        }
        
        public async Task<bool> RegisterAsync(RegisterDto registerDto)
        {
            // Check if user already exists
            if (await _context.Pessoas.AnyAsync(p => p.Usuario == registerDto.Usuario))
                return false;
                
            var pessoa = new Pessoa
            {
                NomePessoa = registerDto.NomePessoa,
                Usuario = registerDto.Usuario,
                Senha = HashPassword(registerDto.Senha)
            };
            
            _context.Pessoas.Add(pessoa);
            await _context.SaveChangesAsync();
            
            // Add user types
            foreach (var tipoUsuarioId in registerDto.TiposUsuario)
            {
                var pessoaTipoUsuario = new PessoaTipoUsuario
                {
                    IdPessoa = pessoa.IdPessoa,
                    IdTipoUsuario = tipoUsuarioId
                };
                _context.PessoaTipoUsuarios.Add(pessoaTipoUsuario);
            }
            
            await _context.SaveChangesAsync();
            return true;
        }
        
        public string GenerateJwtToken(int idPessoa, string nomePessoa, List<string> papeis)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]!));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, idPessoa.ToString()),
                new Claim(ClaimTypes.Name, nomePessoa),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };
            
            foreach (var papel in papeis)
            {
                claims.Add(new Claim(ClaimTypes.Role, papel));
            }
            
            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.Now.AddHours(24),
                signingCredentials: credentials
            );
            
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
        
        public string HashPassword(string password)
        {
            using var sha256 = SHA256.Create();
            var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
            return Convert.ToBase64String(hashedBytes);
        }
        
        public bool VerifyPassword(string password, string hashedPassword)
        {
            var hashedInput = HashPassword(password);
            return hashedInput == hashedPassword;
        }
    }
}
