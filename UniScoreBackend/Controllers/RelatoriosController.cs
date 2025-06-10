using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UniScore.API.Services;

namespace UniScore.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "Coordenador,ProfessorAdmin")]
    public class RelatoriosController : ControllerBase
    {
        private readonly IRelatorioService _relatorioService;

        public RelatoriosController(IRelatorioService relatorioService)
        {
            _relatorioService = relatorioService;
        }

        /// <summary>
        /// Obtém ranking de um evento
        /// </summary>
        [HttpGet("ranking/{eventoId}")]
        public async Task<ActionResult<List<RankingDto>>> GetRanking(int eventoId)
        {
            try
            {
                var ranking = await _relatorioService.GetRankingEventoAsync(eventoId);
                return Ok(ranking);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Erro interno do servidor", details = ex.Message });
            }
        }

        /// <summary>
        /// Obtém relatório completo de um evento
        /// </summary>
        [HttpGet("evento/{eventoId}")]
        public async Task<ActionResult<RelatorioEventoDto>> GetRelatorioEvento(int eventoId)
        {
            try
            {
                var relatorio = await _relatorioService.GetRelatorioEventoAsync(eventoId);
                return Ok(relatorio);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Erro interno do servidor", details = ex.Message });
            }
        }

        /// <summary>
        /// Obtém estatísticas de um evento
        /// </summary>
        [HttpGet("estatisticas/{eventoId}")]
        public async Task<ActionResult<EstatisticasEventoDto>> GetEstatisticas(int eventoId)
        {
            try
            {
                var estatisticas = await _relatorioService.GetEstatisticasEventoAsync(eventoId);
                return Ok(estatisticas);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Erro interno do servidor", details = ex.Message });
            }
        }
    }
}