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
    public class TiposUsuarioController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        
        public TiposUsuarioController(ApplicationDbContext context)
        {
            _context = context;
        }
        
        /// <summary>
        /// Obtém todos os tipos de usuário
        /// </summary>
        /// <returns>Lista de tipos de usuário</returns>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<TipoUsuarioDto>), 200)]
        public async Task<ActionResult<IEnumerable<TipoUsuarioDto>>> GetTiposUsuario()
        {
            var tipos = await _context.TiposUsuario
                .Select(t => new TipoUsuarioDto
                {
                    IdTipoUsuario = t.IdTipoUsuario,
                    PapelUsuario = t.PapelUsuario
                })
                .ToListAsync();
                
            return Ok(tipos);
        }
        
        /// <summary>
        /// Obtém um tipo de usuário específico por ID
        /// </summary>
        /// <param name="id">ID do tipo de usuário</param>
        /// <returns>Tipo de usuário encontrado</returns>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(TipoUsuarioDto), 200)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<TipoUsuarioDto>> GetTipoUsuario(byte id)
        {
            var tipo = await _context.TiposUsuario.FindAsync(id);
            if (tipo == null)
                return NotFound();
                
            var tipoDto = new TipoUsuarioDto
            {
                IdTipoUsuario = tipo.IdTipoUsuario,
                PapelUsuario = tipo.PapelUsuario
            };
            
            return Ok(tipoDto);
        }
        
        /// <summary>
        /// Cria um novo tipo de usuário
        /// </summary>
        /// <param name="createDto">Dados do tipo de usuário a ser criado</param>
        /// <returns>Tipo de usuário criado</returns>
        [HttpPost]
        [ProducesResponseType(typeof(TipoUsuarioDto), 201)]
        [ProducesResponseType(400)]
        public async Task<ActionResult<TipoUsuarioDto>> CreateTipoUsuario(CreateTipoUsuarioDto createDto)
        {
            var tipo = new TipoUsuario
            {
                PapelUsuario = createDto.PapelUsuario
            };
            
            _context.TiposUsuario.Add(tipo);
            await _context.SaveChangesAsync();
            
            var result = await GetTipoUsuario(tipo.IdTipoUsuario);
            return CreatedAtAction(nameof(GetTipoUsuario), new { id = tipo.IdTipoUsuario }, result.Value);
        }
        
        /// <summary>
        /// Atualiza um tipo de usuário existente
        /// </summary>
        /// <param name="id">ID do tipo de usuário</param>
        /// <param name="updateDto">Dados atualizados do tipo de usuário</param>
        /// <returns>Resultado da operação</returns>
        [HttpPut("{id}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> UpdateTipoUsuario(byte id, UpdateTipoUsuarioDto updateDto)
        {
            var tipo = await _context.TiposUsuario.FindAsync(id);
            if (tipo == null)
                return NotFound();
                
            tipo.PapelUsuario = updateDto.PapelUsuario;
            await _context.SaveChangesAsync();
            
            return NoContent();
        }
        
        /// <summary>
        /// Remove um tipo de usuário
        /// </summary>
        /// <param name="id">ID do tipo de usuário</param>
        /// <returns>Resultado da operação</returns>
        [HttpDelete("{id}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> DeleteTipoUsuario(byte id)
        {
            var tipo = await _context.TiposUsuario.FindAsync(id);
            if (tipo == null)
                return NotFound();
                
            // Check if type is being used
            var hasUsuarios = await _context.PessoaTipoUsuarios.AnyAsync(ptu => ptu.IdTipoUsuario == id);
            if (hasUsuarios)
                return BadRequest("Não é possível excluir o tipo de usuário pois está sendo utilizado");
                
            _context.TiposUsuario.Remove(tipo);
            await _context.SaveChangesAsync();
            
            return NoContent();
        }
    }
}
