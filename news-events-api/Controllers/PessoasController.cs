using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProjectEvaluationApi.Data;
using ProjectEvaluationApi.DTOs;
using ProjectEvaluationApi.Models;
using ProjectEvaluationApi.Services;

namespace ProjectEvaluationApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    [Produces("application/json")]
    public class PessoasController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IAuthService _authService;
        
        public PessoasController(ApplicationDbContext context, IAuthService authService)
        {
            _context = context;
            _authService = authService;
        }
        
        /// <summary>
        /// Obtém todas as pessoas
        /// </summary>
        /// <returns>Lista de pessoas</returns>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<PessoaDto>), 200)]
        public async Task<ActionResult<IEnumerable<PessoaDto>>> GetPessoas()
        {
            var pessoas = await _context.Pessoas
                .Include(p => p.PessoaTipoUsuarios)
                .ThenInclude(ptu => ptu.TipoUsuario)
                .Select(p => new PessoaDto
                {
                    IdPessoa = p.IdPessoa,
                    NomePessoa = p.NomePessoa,
                    Usuario = p.Usuario,
                    TiposUsuario = p.PessoaTipoUsuarios.Select(ptu => new TipoUsuarioDto
                    {
                        IdTipoUsuario = ptu.TipoUsuario!.IdTipoUsuario,
                        PapelUsuario = ptu.TipoUsuario.PapelUsuario
                    }).ToList()
                })
                .ToListAsync();
                
            return Ok(pessoas);
        }
        
        /// <summary>
        /// Obtém uma pessoa específica por ID
        /// </summary>
        /// <param name="id">ID da pessoa</param>
        /// <returns>Pessoa encontrada</returns>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(PessoaDto), 200)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<PessoaDto>> GetPessoa(int id)
        {
            var pessoa = await _context.Pessoas
                .Include(p => p.PessoaTipoUsuarios)
                .ThenInclude(ptu => ptu.TipoUsuario)
                .Where(p => p.IdPessoa == id)
                .Select(p => new PessoaDto
                {
                    IdPessoa = p.IdPessoa,
                    NomePessoa = p.NomePessoa,
                    Usuario = p.Usuario,
                    TiposUsuario = p.PessoaTipoUsuarios.Select(ptu => new TipoUsuarioDto
                    {
                        IdTipoUsuario = ptu.TipoUsuario!.IdTipoUsuario,
                        PapelUsuario = ptu.TipoUsuario.PapelUsuario
                    }).ToList()
                })
                .FirstOrDefaultAsync();
                
            if (pessoa == null)
                return NotFound();
                
            return Ok(pessoa);
        }
        
        /// <summary>
        /// Cria uma nova pessoa
        /// </summary>
        /// <param name="createDto">Dados da pessoa a ser criada</param>
        /// <returns>Pessoa criada</returns>
        [HttpPost]
        [ProducesResponseType(typeof(PessoaDto), 201)]
        [ProducesResponseType(400)]
        public async Task<ActionResult<PessoaDto>> CreatePessoa(CreatePessoaDto createDto)
        {
            // Check if user already exists
            if (await _context.Pessoas.AnyAsync(p => p.Usuario == createDto.Usuario))
                return BadRequest("Usuário já existe");
                
            var pessoa = new Pessoa
            {
                NomePessoa = createDto.NomePessoa,
                Usuario = createDto.Usuario,
                Senha = _authService.HashPassword(createDto.Senha)
            };
            
            _context.Pessoas.Add(pessoa);
            await _context.SaveChangesAsync();
            
            // Add user types
            foreach (var tipoUsuarioId in createDto.TiposUsuario)
            {
                var pessoaTipoUsuario = new PessoaTipoUsuario
                {
                    IdPessoa = pessoa.IdPessoa,
                    IdTipoUsuario = tipoUsuarioId
                };
                _context.PessoaTipoUsuarios.Add(pessoaTipoUsuario);
            }
            
            await _context.SaveChangesAsync();
            
            var result = await GetPessoa(pessoa.IdPessoa);
            return CreatedAtAction(nameof(GetPessoa), new { id = pessoa.IdPessoa }, result.Value);
        }
        
        /// <summary>
        /// Atualiza uma pessoa existente
        /// </summary>
        /// <param name="id">ID da pessoa</param>
        /// <param name="updateDto">Dados atualizados da pessoa</param>
        /// <returns>Resultado da operação</returns>
        [HttpPut("{id}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> UpdatePessoa(int id, UpdatePessoaDto updateDto)
        {
            var pessoa = await _context.Pessoas.FindAsync(id);
            if (pessoa == null)
                return NotFound();
                
            // Check if new username is already taken by another user
            if (await _context.Pessoas.AnyAsync(p => p.Usuario == updateDto.Usuario && p.IdPessoa != id))
                return BadRequest("Usuário já existe");
                
            pessoa.NomePessoa = updateDto.NomePessoa;
            pessoa.Usuario = updateDto.Usuario;
            
            if (!string.IsNullOrEmpty(updateDto.Senha))
            {
                pessoa.Senha = _authService.HashPassword(updateDto.Senha);
            }
            
            // Update user types
            var existingTypes = await _context.PessoaTipoUsuarios
                .Where(ptu => ptu.IdPessoa == id)
                .ToListAsync();
            _context.PessoaTipoUsuarios.RemoveRange(existingTypes);
            
            foreach (var tipoUsuarioId in updateDto.TiposUsuario)
            {
                var pessoaTipoUsuario = new PessoaTipoUsuario
                {
                    IdPessoa = id,
                    IdTipoUsuario = tipoUsuarioId
                };
                _context.PessoaTipoUsuarios.Add(pessoaTipoUsuario);
            }
            
            await _context.SaveChangesAsync();
            return NoContent();
        }
        
        /// <summary>
        /// Remove uma pessoa
        /// </summary>
        /// <param name="id">ID da pessoa</param>
        /// <returns>Resultado da operação</returns>
        [HttpDelete("{id}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> DeletePessoa(int id)
        {
            var pessoa = await _context.Pessoas.FindAsync(id);
            if (pessoa == null)
                return NotFound();
                
            // Check if person has evaluations
            var hasAvaliacoes = await _context.Avaliacoes.AnyAsync(a => a.IdPessoa == id);
            if (hasAvaliacoes)
                return BadRequest("Não é possível excluir a pessoa pois possui avaliações associadas");
                
            // Remove related user types
            var tiposUsuario = await _context.PessoaTipoUsuarios
                .Where(ptu => ptu.IdPessoa == id)
                .ToListAsync();
            _context.PessoaTipoUsuarios.RemoveRange(tiposUsuario);
            
            _context.Pessoas.Remove(pessoa);
            await _context.SaveChangesAsync();
            
            return NoContent();
        }
    }
}
