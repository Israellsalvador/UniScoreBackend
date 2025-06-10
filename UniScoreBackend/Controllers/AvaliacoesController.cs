using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using UniScore.API.DTOs;
using UniScore.API.Services;

namespace UniScore.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class AvaliacoesController : ControllerBase
    {
        private readonly IAvaliacaoService _avaliacaoService;

        public AvaliacoesController(IAvaliacaoService avaliacaoService)
        {
            _avaliacaoService = avaliacaoService;
        }

        /// <summary>
        /// Cria uma nova avaliação (apenas Avaliadores)
        /// </summary>
        [HttpPost]
        [Authorize(Roles = "Avaliador")]
        public async Task<ActionResult<AvaliacaoDto>> CreateAvaliacao([FromBody] CreateAvaliacaoDto request)
        {
            try
            {
                var avaliadorId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");
                var avaliacao = await _avaliacaoService.CreateAvaliacaoAsync(avaliadorId, request);
                return CreatedAtAction(nameof(GetMinhasAvaliacoes), null, avaliacao);
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
        /// Obtém critérios de avaliação de um evento
        /// </summary>
        [HttpGet("criterios/{eventoId}")]
        public async Task<ActionResult<CriteriosEventoDto>> GetCriteriosEvento(int eventoId)
        {
            try
            {
                var criterios = await _avaliacaoService.GetCriteriosEventoAsync(eventoId);
                return Ok(criterios);
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
        /// Obtém minhas avaliações (apenas Avaliadores)
        /// </summary>
        [HttpGet("minhas")]
        [Authorize(Roles = "Avaliador")]
        public async Task<ActionResult<List<AvaliacaoDto>>> GetMinhasAvaliacoes()
        {
            try
            {
                var avaliadorId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");
                var avaliacoes = await _avaliacaoService.GetMinhasAvaliacoesAsync(avaliadorId);
                return Ok(avaliacoes);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Erro interno do servidor", details = ex.Message });
            }
        }

        /// <summary>
        /// Obtém avaliações de um projeto específico (apenas Coordenadores)
        /// </summary>
        [HttpGet("projeto/{projetoId}")]
        [Authorize(Roles = "Coordenador,ProfessorAdmin")]
        public async Task<ActionResult<List<AvaliacaoDto>>> GetAvaliacoesByProjeto(int projetoId)
        {
            try
            {
                var avaliacoes = await _avaliacaoService.GetAvaliacoesByProjetoAsync(projetoId);
                return Ok(avaliacoes);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Erro interno do servidor", details = ex.Message });
            }
        }

        /// <summary>
        /// Verifica se já avaliou um projeto
        /// </summary>
        [HttpGet("ja-avaliou/{projetoId}")]
        [Authorize(Roles = "Avaliador")]
        public async Task<ActionResult<bool>> JaAvaliou(int projetoId)
        {
            try
            {
                var avaliadorId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");
                var jaAvaliou = await _avaliacaoService.JaAvaliouAsync(avaliadorId, projetoId);
                return Ok(jaAvaliou);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Erro interno do servidor", details = ex.Message });
            }
        }
    }
}