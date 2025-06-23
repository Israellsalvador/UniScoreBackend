using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProjectEvaluationApi.Data;
using ProjectEvaluationApi.DTOs;
using ProjectEvaluationApi.Models;

namespace ProjectEvaluationApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    [Produces("application/json")]
    public class ProjetosController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        
        public ProjetosController(ApplicationDbContext context)
        {
            _context = context;
        }
        
        /// <summary>
        /// Obtém todos os projetos
        /// </summary>
        /// <returns>Lista de projetos</returns>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<ProjetoDto>), 200)]
        public async Task<ActionResult<IEnumerable<ProjetoDto>>> GetProjetos()
        {
            var projetos = await _context.Projetos
                .Include(p => p.InformacoesComplementares)
                .Include(p => p.EventoProjetos)
                .Select(p => new ProjetoDto
                {
                    IdProjeto = p.IdProjeto,
                    NomeProjeto = p.NomeProjeto,
                    DescricaoProjeto = p.DescricaoProjeto,
                    Integrantes = p.Integrantes,
                    Turma = p.Turma,
                    NotaFinal = p.NotaFinal,
                    InformacoesComplementares = p.InformacoesComplementares.Select(ic => new InformacoesComplementaresDto
                    {
                        IdComplemento = ic.IdComplemento,
                        InformacaoComplementar = ic.InformacaoComplementar,
                        IdProjeto = ic.IdProjeto
                    }).ToList(),
                    EventosIds = p.EventoProjetos.Select(ep => ep.IdEvento).ToList()
                })
                .ToListAsync();
                
            return Ok(projetos);
        }
        
        /// <summary>
        /// Obtém um projeto específico por ID
        /// </summary>
        /// <param name="id">ID do projeto</param>
        /// <returns>Projeto encontrado</returns>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(ProjetoDto), 200)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<ProjetoDto>> GetProjeto(int id)
        {
            var projeto = await _context.Projetos
                .Include(p => p.InformacoesComplementares)
                .Include(p => p.EventoProjetos)
                .Where(p => p.IdProjeto == id)
                .Select(p => new ProjetoDto
                {
                    IdProjeto = p.IdProjeto,
                    NomeProjeto = p.NomeProjeto,
                    DescricaoProjeto = p.DescricaoProjeto,
                    Integrantes = p.Integrantes,
                    Turma = p.Turma,
                    NotaFinal = p.NotaFinal,
                    InformacoesComplementares = p.InformacoesComplementares.Select(ic => new InformacoesComplementaresDto
                    {
                        IdComplemento = ic.IdComplemento,
                        InformacaoComplementar = ic.InformacaoComplementar,
                        IdProjeto = ic.IdProjeto
                    }).ToList(),
                    EventosIds = p.EventoProjetos.Select(ep => ep.IdEvento).ToList()
                })
                .FirstOrDefaultAsync();
                
            if (projeto == null)
                return NotFound();
                
            return Ok(projeto);
        }
        
        /// <summary>
        /// Cria um novo projeto
        /// </summary>
        /// <param name="createDto">Dados do projeto a ser criado</param>
        /// <returns>Projeto criado</returns>
        [HttpPost]
        [ProducesResponseType(typeof(ProjetoDto), 201)]
        [ProducesResponseType(400)]
        public async Task<ActionResult<ProjetoDto>> CreateProjeto(CreateProjetoDto createDto)
        {
            var projeto = new Projeto
            {
                NomeProjeto = createDto.NomeProjeto,
                DescricaoProjeto = createDto.DescricaoProjeto,
                Integrantes = createDto.Integrantes,
                Turma = createDto.Turma
            };
            
            _context.Projetos.Add(projeto);
            await _context.SaveChangesAsync();
            
            // Add complementary information
            foreach (var info in createDto.InformacoesComplementares)
            {
                var informacao = new InformacoesComplementares
                {
                    IdProjeto = projeto.IdProjeto,
                    InformacaoComplementar = info
                };
                _context.InformacoesComplementares.Add(informacao);
            }
            
            await _context.SaveChangesAsync();
            
            var result = await GetProjeto(projeto.IdProjeto);
            return CreatedAtAction(nameof(GetProjeto), new { id = projeto.IdProjeto }, result.Value);
        }
        
        /// <summary>
        /// Atualiza um projeto existente
        /// </summary>
        /// <param name="id">ID do projeto</param>
        /// <param name="updateDto">Dados atualizados do projeto</param>
        /// <returns>Resultado da operação</returns>
        [HttpPut("{id}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> UpdateProjeto(int id, UpdateProjetoDto updateDto)
        {
            var projeto = await _context.Projetos.FindAsync(id);
            if (projeto == null)
                return NotFound();
                
            projeto.NomeProjeto = updateDto.NomeProjeto;
            projeto.DescricaoProjeto = updateDto.DescricaoProjeto;
            projeto.Integrantes = updateDto.Integrantes;
            projeto.Turma = updateDto.Turma;
            projeto.NotaFinal = updateDto.NotaFinal;
            
            // Update complementary information
            var existingInfos = await _context.InformacoesComplementares
                .Where(ic => ic.IdProjeto == id)
                .ToListAsync();
            _context.InformacoesComplementares.RemoveRange(existingInfos);
            
            foreach (var info in updateDto.InformacoesComplementares)
            {
                var informacao = new InformacoesComplementares
                {
                    IdProjeto = id,
                    InformacaoComplementar = info
                };
                _context.InformacoesComplementares.Add(informacao);
            }
            
            await _context.SaveChangesAsync();
            return NoContent();
        }
        
        /// <summary>
        /// Remove um projeto
        /// </summary>
        /// <param name="id">ID do projeto</param>
        /// <returns>Resultado da operação</returns>
        [HttpDelete("{id}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> DeleteProjeto(int id)
        {
            var projeto = await _context.Projetos.FindAsync(id);
            if (projeto == null)
                return NotFound();
                
            // Check if project has evaluations
            var hasAvaliacoes = await _context.Avaliacoes.AnyAsync(a => a.IdProjeto == id);
            if (hasAvaliacoes)
                return BadRequest("Não é possível excluir o projeto pois possui avaliações associadas");
                
            // Remove related information
            var infos = await _context.InformacoesComplementares
                .Where(ic => ic.IdProjeto == id)
                .ToListAsync();
            _context.InformacoesComplementares.RemoveRange(infos);
            
            var eventoProjetos = await _context.EventoProjetos
                .Where(ep => ep.IdProjeto == id)
                .ToListAsync();
            _context.EventoProjetos.RemoveRange(eventoProjetos);
            
            _context.Projetos.Remove(projeto);
            await _context.SaveChangesAsync();
            
            return NoContent();
        }
        
        /// <summary>
        /// Calcula a nota final do projeto baseada nas avaliações
        /// </summary>
        /// <param name="id">ID do projeto</param>
        /// <returns>Resultado da operação</returns>
        [HttpPost("{id}/calcular-nota")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> CalcularNotaFinal(int id)
        {
            var projeto = await _context.Projetos.FindAsync(id);
            if (projeto == null)
                return NotFound();
                
            var avaliacoes = await _context.Avaliacoes
                .Include(a => a.Notas)
                .Where(a => a.IdProjeto == id)
                .ToListAsync();
                
            if (!avaliacoes.Any())
                return BadRequest("Projeto não possui avaliações");
                
            var todasNotas = avaliacoes.SelectMany(a => a.Notas).ToList();
            if (!todasNotas.Any())
                return BadRequest("Projeto não possui notas atribuídas");
                
            var notaFinal = todasNotas.Average(n => n.NotaProjeto);
            projeto.NotaFinal = (float)notaFinal;
            
            await _context.SaveChangesAsync();
            
            return Ok(new { NotaFinal = notaFinal });
        }
    }
}
