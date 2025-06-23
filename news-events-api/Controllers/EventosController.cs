using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProjectEvaluationApi.Data;
using ProjectEvaluationApi.DTOs;
using ProjectEvaluationApi.Models;

namespace NewsEventsApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    [Produces("application/json")]
    public class EventosController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        
        public EventosController(ApplicationDbContext context)
        {
            _context = context;
        }
        
        /// <summary>
        /// Obtém todos os eventos
        /// </summary>
        /// <returns>Lista de eventos</returns>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<EventoDto>), 200)]
        public async Task<ActionResult<IEnumerable<EventoDto>>> GetEventos()
        {
            var eventos = await _context.Eventos
                .Include(e => e.CriteriosAvaliacao)
                .Include(e => e.EventoProjetos)
                .Select(e => new EventoDto
                {
                    IdEvento = e.IdEvento,
                    NomeEvento = e.NomeEvento,
                    DescricaoEvento = e.DescricaoEvento,
                    Local = e.Local,
                    DataInicio = e.DataInicio,
                    DataFim = e.DataFim,
                    CriteriosAvaliacao = e.CriteriosAvaliacao.Select(ca => new CriterioAvaliacaoDto
                    {
                        IdCriterio = ca.IdCriterio,
                        IdEvento = ca.IdEvento,
                        NomeCriterio = ca.NomeCriterio
                    }).ToList(),
                    ProjetosIds = e.EventoProjetos.Select(ep => ep.IdProjeto).ToList()
                })
                .ToListAsync();
                
            return Ok(eventos);
        }
        
        /// <summary>
        /// Obtém um evento específico por ID
        /// </summary>
        /// <param name="id">ID do evento</param>
        /// <returns>Evento encontrado</returns>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(EventoDto), 200)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<EventoDto>> GetEvento(int id)
        {
            var evento = await _context.Eventos
                .Include(e => e.CriteriosAvaliacao)
                .Include(e => e.EventoProjetos)
                .Where(e => e.IdEvento == id)
                .Select(e => new EventoDto
                {
                    IdEvento = e.IdEvento,
                    NomeEvento = e.NomeEvento,
                    DescricaoEvento = e.DescricaoEvento,
                    Local = e.Local,
                    DataInicio = e.DataInicio,
                    DataFim = e.DataFim,
                    CriteriosAvaliacao = e.CriteriosAvaliacao.Select(ca => new CriterioAvaliacaoDto
                    {
                        IdCriterio = ca.IdCriterio,
                        IdEvento = ca.IdEvento,
                        NomeCriterio = ca.NomeCriterio
                    }).ToList(),
                    ProjetosIds = e.EventoProjetos.Select(ep => ep.IdProjeto).ToList()
                })
                .FirstOrDefaultAsync();
                
            if (evento == null)
                return NotFound();
                
            return Ok(evento);
        }
        
        /// <summary>
        /// Cria um novo evento
        /// </summary>
        /// <param name="createDto">Dados do evento a ser criado</param>
        /// <returns>Evento criado</returns>
        [HttpPost]
        [ProducesResponseType(typeof(EventoDto), 201)]
        [ProducesResponseType(400)]
        public async Task<ActionResult<EventoDto>> CreateEvento(CreateEventoDto createDto)
        {
            var evento = new Evento
            {
                NomeEvento = createDto.NomeEvento,
                DescricaoEvento = createDto.DescricaoEvento,
                Local = createDto.Local,
                DataInicio = createDto.DataInicio,
                DataFim = createDto.DataFim
            };
            
            _context.Eventos.Add(evento);
            await _context.SaveChangesAsync();
            
            
            
            // Add evaluation criteria
            foreach (var criterioNome in createDto.CriteriosAvaliacao)
            {
                var criterio = new CriterioAvaliacao
                {
                    IdEvento = evento.IdEvento,
                    NomeCriterio = criterioNome
                };
                _context.CriteriosAvaliacao.Add(criterio);
            }
            
            await _context.SaveChangesAsync();
            
            var result = await GetEvento(evento.IdEvento);
            return CreatedAtAction(nameof(GetEvento), new { id = evento.IdEvento }, result.Value);
        }
        
        /// <summary>
        /// Atualiza um evento existente
        /// </summary>
        /// <param name="id">ID do evento</param>
        /// <param name="updateDto">Dados atualizados do evento</param>
        /// <returns>Resultado da operação</returns>
        [HttpPut("{id}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> UpdateEvento(int id, UpdateEventoDto updateDto)
        {
            var evento = await _context.Eventos.FindAsync(id);
            if (evento == null)
                return NotFound();
                
            evento.NomeEvento = updateDto.NomeEvento;
            evento.DescricaoEvento = updateDto.DescricaoEvento;
            evento.Local = updateDto.Local;
            evento.DataInicio = updateDto.DataInicio;
            evento.DataFim = updateDto.DataFim;
            
            
            // Update evaluation criteria
            var existingCriterios = await _context.CriteriosAvaliacao
                .Where(ca => ca.IdEvento == id)
                .ToListAsync();
            _context.CriteriosAvaliacao.RemoveRange(existingCriterios);
            
            foreach (var criterioNome in updateDto.CriteriosAvaliacao)
            {
                var criterio = new CriterioAvaliacao
                {
                    IdEvento = id,
                    NomeCriterio = criterioNome
                };
                _context.CriteriosAvaliacao.Add(criterio);
            }
            
            await _context.SaveChangesAsync();
            return NoContent();
        }
        
        /// <summary>
        /// Remove um evento
        /// </summary>
        /// <param name="id">ID do evento</param>
        /// <returns>Resultado da operação</returns>
        [HttpDelete("{id}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> DeleteEvento(int id)
        {
            var evento = await _context.Eventos.FindAsync(id);
            if (evento == null)
                return NotFound();
                
            // Check if event has projects
            var hasProjetos = await _context.EventoProjetos.AnyAsync(ep => ep.IdEvento == id);
            if (hasProjetos)
                return BadRequest("Não é possível excluir o evento pois possui projetos associados");
                
           
            // Remove related criteria and their evaluations
            var criterios = await _context.CriteriosAvaliacao
                .Where(ca => ca.IdEvento == id)
                .ToListAsync();
                
            foreach (var criterio in criterios)
            {
                var notas = await _context.Notas
                    .Where(n => n.IdCriterio == criterio.IdCriterio)
                    .ToListAsync();
                _context.Notas.RemoveRange(notas);
            }
            
            _context.CriteriosAvaliacao.RemoveRange(criterios);
            _context.Eventos.Remove(evento);
            await _context.SaveChangesAsync();
            
            return NoContent();
        }
        
       
      
        
        /// <summary>
        /// Adiciona um projeto ao evento
        /// </summary>
        /// <param name="eventoId">ID do evento</param>
        /// <param name="projetoId">ID do projeto</param>
        /// <returns>Resultado da operação</returns>
        [HttpPost("{eventoId}/projetos/{projetoId}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> AddProjetoToEvento(int eventoId, int projetoId)
        {
            var evento = await _context.Eventos.FindAsync(eventoId);
            var projeto = await _context.Projetos.FindAsync(projetoId);
            
            if (evento == null || projeto == null)
                return NotFound();
                
            var existing = await _context.EventoProjetos
                .FirstOrDefaultAsync(ep => ep.IdEvento == eventoId && ep.IdProjeto == projetoId);
                
            if (existing != null)
                return BadRequest("Projeto já está associado ao evento");
                
            var eventoProjeto = new EventoProjeto
            {
                IdEvento = eventoId,
                IdProjeto = projetoId
            };
            
            _context.EventoProjetos.Add(eventoProjeto);
            await _context.SaveChangesAsync();
            
            return NoContent();
        }
        
        /// <summary>
        /// Remove um projeto do evento
        /// </summary>
        /// <param name="eventoId">ID do evento</param>
        /// <param name="projetoId">ID do projeto</param>
        /// <returns>Resultado da operação</returns>
        [HttpDelete("{eventoId}/projetos/{projetoId}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> RemoveProjetoFromEvento(int eventoId, int projetoId)
        {
            var eventoProjeto = await _context.EventoProjetos
                .FirstOrDefaultAsync(ep => ep.IdEvento == eventoId && ep.IdProjeto == projetoId);
                
            if (eventoProjeto == null)
                return NotFound();
                
            _context.EventoProjetos.Remove(eventoProjeto);
            await _context.SaveChangesAsync();
            
            return NoContent();
        }
    }
}
