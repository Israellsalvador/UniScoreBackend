using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UniScore.API.DTOs;
using UniScore.API.Services;

namespace UniScore.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        /// <summary>
        /// Realiza login no sistema
        /// </summary>
        [HttpPost("login")]
        public async Task<ActionResult<LoginResponseDto>> Login([FromBody] LoginRequestDto request)
        {
            try
            {
                var response = await _authService.LoginAsync(request);
                return Ok(response);
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        /// <summary>
        /// Cria um novo usuário (apenas para Coordenadores e ProfessorAdmin)
        /// </summary>
        [HttpPost("register")]
       // [Authorize(Roles = "Coordenador,ProfessorAdmin")]
        public async Task<ActionResult<UserDto>> Register([FromBody] CreateUserDto request)
        {
            try
            {
                var user = await _authService.CreateUserAsync(request);
                return CreatedAtAction(nameof(GetUser), new { id = user.IdPessoa }, user);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Erro interno do servidor", details = ex.Message });
            }
        }

        /// <summary>
        /// Obtém informações do usuário por ID
        /// </summary>
        [HttpGet("user/{id}")]
        [Authorize]
        public async Task<ActionResult<UserDto>> GetUser(int id)
        {
            try
            {
                var user = await _authService.GetUserByIdAsync(id);
                return Ok(user);
            }
            catch (ArgumentException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Erro interno do servidor", details = ex.Message });
            }
        }

        /// <summary>
        /// Lista todos os usuários (apenas para Coordenadores e ProfessorAdmin)
        /// </summary>
        [HttpGet("users")]
        [Authorize(Roles = "Coordenador,ProfessorAdmin")]
        public async Task<ActionResult<List<UserDto>>> GetAllUsers()
        {
            try
            {
                var users = await _authService.GetAllUsersAsync();
                return Ok(users);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Erro interno do servidor", details = ex.Message });
            }
        }

        /// <summary>
        /// Lista apenas avaliadores
        /// </summary>
        [HttpGet("avaliadores")]
        [Authorize(Roles = "Coordenador,ProfessorAdmin")]
        public async Task<ActionResult<List<UserDto>>> GetAvaliadores()
        {
            try
            {
                var avaliadores = await _authService.GetAvaliadoresAsync();
                return Ok(avaliadores);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Erro interno do servidor", details = ex.Message });
            }
        }

        /// <summary>
        /// Valida se o token ainda é válido
        /// </summary>
        [HttpGet("validate")]
        [Authorize]
        public IActionResult ValidateToken()
        {
            return Ok(new { message = "Token válido", user = User.Identity?.Name });
        }
    }
}