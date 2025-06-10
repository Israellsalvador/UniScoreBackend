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
    public class ProjetosController : ControllerBase
    {
        private readonly IProjetoService _projetoService;

        public ProjetosController(IProjetoService projetoService)
        {
            _projetoService = projetoService;
        }

        /// <summary>
        /// Cria um novo projeto (apenas Coordenadores)
        /// </summary>
        [HttpPost]
        [Authorize(Roles = "Coordenador,ProfessorAdmin")]
        public async Task<ActionResult<ProjetoDto>> CreateProjeto([FromBody] CreateProjetoDto request)
        {
            try
            {
                var projeto = await _projetoService.CreateProjetoAsync(request);
                return CreatedAtAction(nameof(GetProjeto), new { id = projeto.IdProjeto }, projeto);
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
        /// Obtém projeto por ID
        /// </summary>
        [HttpGet("{id}")]
        public async Task<ActionResult<ProjetoDto>> GetProjeto(int id)
        {
            try
            {
                var projeto = await _projetoService.GetProjetoByIdAsync(id);
                return Ok(projeto);
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
        /// Lista projetos por evento
        /// </summary>
        [HttpGet("evento/{eventoId}")]
        public async Task<ActionResult<List<ProjetoDto>>> GetProjetosByEvento(int eventoId)
        {
            try
            {
                var projetos = await _projetoService.GetProjetosByEventoAsync(eventoId);
                return Ok(projetos);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Erro interno do servidor", details = ex.Message });
            }
        }

        /// <summary>
        /// Lista projetos para avaliar (Mobile - apenas para Avaliadores)
        /// </summary>
        [HttpGet("para-avaliar")]
        [Authorize(Roles = "Avaliador")]
        public async Task<ActionResult<List<ProjetoParaAvaliarDto>>> GetProjetosParaAvaliar([FromQuery] int eventoId)
        {
            try
            {
                var avaliadorId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");
                var projetos = await _projetoService.GetProjetosParaAvaliarAsync(avaliadorId, eventoId);
                return Ok(projetos);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Erro interno do servidor", details = ex.Message });
            }
        }

        /// <summary>
        /// Atualiza um projeto (apenas Coordenadores)
        /// </summary>
        [HttpPut("{id}")]
        [Authorize(Roles = "Coordenador,ProfessorAdmin")]
        public async Task<ActionResult<ProjetoDto>> UpdateProjeto(int id, [FromBody] CreateProjetoDto request)
        {
            try
            {
                var projeto = await _projetoService.UpdateProjetoAsync(id, request);
                return Ok(projeto);
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
        /// Exclui um projeto (apenas Coordenadores)
        /// </summary>
        [HttpDelete("{id}")]
        [Authorize(Roles = "Coordenador,ProfessorAdmin")]
        public async Task<ActionResult> DeleteProjeto(int id)
        {
            try
            {
                await _projetoService.DeleteProjetoAsync(id);
                return Ok(new { message = "Projeto excluído com sucesso" });
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
    }
}