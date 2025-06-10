using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UniScore.API.DTOs;
using UniScore.API.Services;

namespace UniScore.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class EventosController : ControllerBase
    {
        private readonly IEventoService _eventoService;

        public EventosController(IEventoService eventoService)
        {
            _eventoService = eventoService;
        }

        /// <summary>
        /// Cria um novo evento (apenas Coordenadores)
        /// </summary>
        [HttpPost]
        [Authorize(Roles = "Coordenador,ProfessorAdmin")]
        public async Task<ActionResult<EventoDto>> CreateEvento([FromBody] CreateEventoDto request)
        {
            try
            {
                var evento = await _eventoService.CreateEventoAsync(request);
                return CreatedAtAction(nameof(GetEvento), new { id = evento.IdEvento }, evento);
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
        /// Obtém evento por ID
        /// </summary>
        [HttpGet("{id}")]
        public async Task<ActionResult<EventoDto>> GetEvento(int id)
        {
            try
            {
                var evento = await _eventoService.GetEventoByIdAsync(id);
                return Ok(evento);
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
        /// Lista todos os eventos
        /// </summary>
        [HttpGet]
        public async Task<ActionResult<List<EventoDto>>> GetAllEventos()
        {
            try
            {
                var eventos = await _eventoService.GetAllEventosAsync();
                return Ok(eventos);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Erro interno do servidor", details = ex.Message });
            }
        }

        /// <summary>
        /// Lista apenas eventos ativos
        /// </summary>
        [HttpGet("ativos")]
        public async Task<ActionResult<List<EventoDto>>> GetEventosAtivos()
        {
            try
            {
                var eventos = await _eventoService.GetEventosAtivosAsync();
                return Ok(eventos);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Erro interno do servidor", details = ex.Message });
            }
        }

        /// <summary>
        /// Atualiza um evento (apenas Coordenadores)
        /// </summary>
        [HttpPut("{id}")]
        [Authorize(Roles = "Coordenador,ProfessorAdmin")]
        public async Task<ActionResult<EventoDto>> UpdateEvento(int id, [FromBody] CreateEventoDto request)
        {
            try
            {
                var evento = await _eventoService.UpdateEventoAsync(id, request);
                return Ok(evento);
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
        /// Encerra um evento (apenas Coordenadores)
        /// </summary>
        [HttpPost("{id}/encerrar")]
        [Authorize(Roles = "Coordenador,ProfessorAdmin")]
        public async Task<ActionResult> EncerrarEvento(int id)
        {
            try
            {
                await _eventoService.EncerrarEventoAsync(id);
                return Ok(new { message = "Evento encerrado com sucesso" });
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
        /// Exclui um evento (apenas Coordenadores)
        /// </summary>
        [HttpDelete("{id}")]
        [Authorize(Roles = "Coordenador,ProfessorAdmin")]
        public async Task<ActionResult> DeleteEvento(int id)
        {
            try
            {
                await _eventoService.DeleteEventoAsync(id);
                return Ok(new { message = "Evento excluído com sucesso" });
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