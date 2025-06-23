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
    public class AvaliacoesController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        
        public AvaliacoesController(ApplicationDbContext context)
        {
            _context = context;
        }
        
        /// <summary>
        /// Obtém todas as avaliações
        /// </summary>
        /// <returns>Lista de avaliações</returns>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<AvaliacaoDto>), 200)]
        public async Task<ActionResult<IEnumerable<AvaliacaoDto>>> GetAvaliacoes()
        {
            var avaliacoes = await _context.Avaliacoes
                .Include(a => a.Pessoa)
                .Include(a => a.Projeto)
                .Include(a => a.Notas)
                .ThenInclude(n => n.CriterioAvaliacao)
                .Select(a => new AvaliacaoDto
                {
                    IdAvaliacao = a.IdAvaliacao,
                    IdPessoa = a.IdPessoa,
                    NomePessoa = a.Pessoa!.NomePessoa,
                    IdProjeto = a.IdProjeto,
                    NomeProjeto = a.Projeto!.NomeProjeto,
                    Notas = a.Notas.Select(n => new NotaDto
                    {
                        Id = n.Id,
                        IdCriterio = n.IdCriterio,
                        NomeCriterio = n.CriterioAvaliacao!.NomeCriterio,
                        IdAvaliacao = n.IdAvaliacao,
                        NotaProjeto = n.NotaProjeto
                    }).ToList(),
                    MediaFinal = a.Notas.Any() ? a.Notas.Average(n => n.NotaProjeto) : null
                })
                .ToListAsync();
                
            return Ok(avaliacoes);
        }
        
        /// <summary>
        /// Obtém uma avaliação específica por ID
        /// </summary>
        /// <param name="id">ID da avaliação</param>
        /// <returns>Avaliação encontrada</returns>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(AvaliacaoDto), 200)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<AvaliacaoDto>> GetAvaliacao(int id)
        {
            var avaliacao = await _context.Avaliacoes
                .Include(a => a.Pessoa)
                .Include(a => a.Projeto)
                .Include(a => a.Notas)
                .ThenInclude(n => n.CriterioAvaliacao)
                .Where(a => a.IdAvaliacao == id)
                .Select(a => new AvaliacaoDto
                {
                    IdAvaliacao = a.IdAvaliacao,
                    IdPessoa = a.IdPessoa,
                    NomePessoa = a.Pessoa!.NomePessoa,
                    IdProjeto = a.IdProjeto,
                    NomeProjeto = a.Projeto!.NomeProjeto,
                    Notas = a.Notas.Select(n => new NotaDto
                    {
                        Id = n.Id,
                        IdCriterio = n.IdCriterio,
                        NomeCriterio = n.CriterioAvaliacao!.NomeCriterio,
                        IdAvaliacao = n.IdAvaliacao,
                        NotaProjeto = n.NotaProjeto
                    }).ToList(),
                    MediaFinal = a.Notas.Any() ? a.Notas.Average(n => n.NotaProjeto) : null
                })
                .FirstOrDefaultAsync();
                
            if (avaliacao == null)
                return NotFound();
                
            return Ok(avaliacao);
        }
        
        /// <summary>
        /// Cria uma nova avaliação
        /// </summary>
        /// <param name="createDto">Dados da avaliação a ser criada</param>
        /// <returns>Avaliação criada</returns>
        [HttpPost]
        [ProducesResponseType(typeof(AvaliacaoDto), 201)]
        [ProducesResponseType(400)]
        public async Task<ActionResult<AvaliacaoDto>> CreateAvaliacao(CreateAvaliacaoDto createDto)
        {
            // Check if person and project exist
            var pessoa = await _context.Pessoas.FindAsync(createDto.IdPessoa);
            var projeto = await _context.Projetos.FindAsync(createDto.IdProjeto);
            
            if (pessoa == null || projeto == null)
                return BadRequest("Pessoa ou projeto não encontrado");
                
            // Check if evaluation already exists
            var existingAvaliacao = await _context.Avaliacoes
                .FirstOrDefaultAsync(a => a.IdPessoa == createDto.IdPessoa && a.IdProjeto == createDto.IdProjeto);
                
            if (existingAvaliacao != null)
                return BadRequest("Avaliação já existe para esta pessoa e projeto");
                
            var avaliacao = new Avaliacao
            {
                IdPessoa = createDto.IdPessoa,
                IdProjeto = createDto.IdProjeto
            };
            
            _context.Avaliacoes.Add(avaliacao);
            await _context.SaveChangesAsync();
            
            // Add grades
            foreach (var notaDto in createDto.Notas)
            {
                var nota = new Nota
                {
                    IdCriterio = notaDto.IdCriterio,
                    IdAvaliacao = avaliacao.IdAvaliacao,
                    NotaProjeto = notaDto.NotaProjeto
                };
                _context.Notas.Add(nota);
            }
            
            await _context.SaveChangesAsync();
            
            var result = await GetAvaliacao(avaliacao.IdAvaliacao);
            return CreatedAtAction(nameof(GetAvaliacao), new { id = avaliacao.IdAvaliacao }, result.Value);
        }
        
        /// <summary>
        /// Atualiza uma avaliação existente
        /// </summary>
        /// <param name="id">ID da avaliação</param>
        /// <param name="updateDto">Dados atualizados da avaliação</param>
        /// <returns>Resultado da operação</returns>
        [HttpPut("{id}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> UpdateAvaliacao(int id, UpdateAvaliacaoDto updateDto)
        {
            var avaliacao = await _context.Avaliacoes.FindAsync(id);
            if (avaliacao == null)
                return NotFound();
                
            // Update grades
            var existingNotas = await _context.Notas
                .Where(n => n.IdAvaliacao == id)
                .ToListAsync();
            _context.Notas.RemoveRange(existingNotas);
            
            foreach (var notaDto in updateDto.Notas)
            {
                var nota = new Nota
                {
                    IdCriterio = notaDto.IdCriterio,
                    IdAvaliacao = id,
                    NotaProjeto = notaDto.NotaProjeto
                };
                _context.Notas.Add(nota);
            }
            
            await _context.SaveChangesAsync();
            return NoContent();
        }
        
        /// <summary>
        /// Remove uma avaliação
        /// </summary>
        /// <param name="id">ID da avaliação</param>
        /// <returns>Resultado da operação</returns>
        [HttpDelete("{id}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> DeleteAvaliacao(int id)
        {
            var avaliacao = await _context.Avaliacoes.FindAsync(id);
            if (avaliacao == null)
                return NotFound();
                
            // Remove related grades
            var notas = await _context.Notas
                .Where(n => n.IdAvaliacao == id)
                .ToListAsync();
            _context.Notas.RemoveRange(notas);
            
            _context.Avaliacoes.Remove(avaliacao);
            await _context.SaveChangesAsync();
            
            return NoContent();
        }
        
        /// <summary>
        /// Obtém avaliações por pessoa
        /// </summary>
        /// <param name="pessoaId">ID da pessoa</param>
        /// <returns>Lista de avaliações da pessoa</returns>
        [HttpGet("pessoa/{pessoaId}")]
        [ProducesResponseType(typeof(IEnumerable<AvaliacaoDto>), 200)]
        public async Task<ActionResult<IEnumerable<AvaliacaoDto>>> GetAvaliacoesByPessoa(int pessoaId)
        {
            var avaliacoes = await _context.Avaliacoes
                .Include(a => a.Pessoa)
                .Include(a => a.Projeto)
                .Include(a => a.Notas)
                .ThenInclude(n => n.CriterioAvaliacao)
                .Where(a => a.IdPessoa == pessoaId)
                .Select(a => new AvaliacaoDto
                {
                    IdAvaliacao = a.IdAvaliacao,
                    IdPessoa = a.IdPessoa,
                    NomePessoa = a.Pessoa!.NomePessoa,
                    IdProjeto = a.IdProjeto,
                    NomeProjeto = a.Projeto!.NomeProjeto,
                    Notas = a.Notas.Select(n => new NotaDto
                    {
                        Id = n.Id,
                        IdCriterio = n.IdCriterio,
                        NomeCriterio = n.CriterioAvaliacao!.NomeCriterio,
                        IdAvaliacao = n.IdAvaliacao,
                        NotaProjeto = n.NotaProjeto
                    }).ToList(),
                    MediaFinal = a.Notas.Any() ? a.Notas.Average(n => n.NotaProjeto) : null
                })
                .ToListAsync();
                
            return Ok(avaliacoes);
        }
        
        /// <summary>
        /// Obtém avaliações por projeto
        /// </summary>
        /// <param name="projetoId">ID do projeto</param>
        /// <returns>Lista de avaliações do projeto</returns>
        [HttpGet("projeto/{projetoId}")]
        [ProducesResponseType(typeof(IEnumerable<AvaliacaoDto>), 200)]
        public async Task<ActionResult<IEnumerable<AvaliacaoDto>>> GetAvaliacoesByProjeto(int projetoId)
        {
            var avaliacoes = await _context.Avaliacoes
                .Include(a => a.Pessoa)
                .Include(a => a.Projeto)
                .Include(a => a.Notas)
                .ThenInclude(n => n.CriterioAvaliacao)
                .Where(a => a.IdProjeto == projetoId)
                .Select(a => new AvaliacaoDto
                {
                    IdAvaliacao = a.IdAvaliacao,
                    IdPessoa = a.IdPessoa,
                    NomePessoa = a.Pessoa!.NomePessoa,
                    IdProjeto = a.IdProjeto,
                    NomeProjeto = a.Projeto!.NomeProjeto,
                    Notas = a.Notas.Select(n => new NotaDto
                    {
                        Id = n.Id,
                        IdCriterio = n.IdCriterio,
                        NomeCriterio = n.CriterioAvaliacao!.NomeCriterio,
                        IdAvaliacao = n.IdAvaliacao,
                        NotaProjeto = n.NotaProjeto
                    }).ToList(),
                    MediaFinal = a.Notas.Any() ? a.Notas.Average(n => n.NotaProjeto) : null
                })
                .ToListAsync();
                
            return Ok(avaliacoes);
        }
    }
}
