using Microsoft.AspNetCore.Mvc;
using ProjectEvaluationApi.DTOs;
using ProjectEvaluationApi.Services;

namespace ProjectEvaluationApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Produces("application/json")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        
        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }
        
        /// <summary>
        /// Realiza login do usuário
        /// </summary>
        /// <param name="loginDto">Credenciais de login</param>
        /// <returns>Token JWT e informações do usuário</returns>
        [HttpPost("login")]
        [ProducesResponseType(typeof(LoginResponseDto), 200)]
        [ProducesResponseType(401)]
        public async Task<ActionResult<LoginResponseDto>> Login(LoginDto loginDto)
        {
            var result = await _authService.LoginAsync(loginDto);
            if (result == null)
                return Unauthorized("Credenciais inválidas");
                
            return Ok(result);
        }
        
        /// <summary>
        /// Registra um novo usuário
        /// </summary>
        /// <param name="registerDto">Dados do usuário a ser registrado</param>
        /// <returns>Resultado do registro</returns>
        [HttpPost("register")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> Register(RegisterDto registerDto)
        {
            var result = await _authService.RegisterAsync(registerDto);
            if (!result)
                return BadRequest("Usuário já existe ou dados inválidos");
                
            return Ok(new { message = "Usuário registrado com sucesso" });
        }
    }
}
