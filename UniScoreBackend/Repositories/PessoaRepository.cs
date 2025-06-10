using Microsoft.EntityFrameworkCore;
using UniScore.API.Data;
using UniScore.API.Models;

namespace UniScore.API.Repositories
{
    public class PessoaRepository : IPessoaRepository
    {
        private readonly UniscoreContext _context;

        public PessoaRepository(UniscoreContext context)
        {
            _context = context;
        }

        public async Task<Pessoa?> GetByIdAsync(int id)
        {
            return await _context.Pessoas
                .Include(p => p.PessoasTiposUsuario)
                .ThenInclude(pt => pt.TipoUsuario)
                .FirstOrDefaultAsync(p => p.IdPessoa == id);
        }

        public async Task<Pessoa?> GetByUsuarioAsync(string usuario)
        {
            return await _context.Pessoas
                .Include(p => p.PessoasTiposUsuario)
                .ThenInclude(pt => pt.TipoUsuario)
                .FirstOrDefaultAsync(p => p.Usuario == usuario);
        }

        public async Task<List<Pessoa>> GetAllAsync()
        {
            return await _context.Pessoas
                .Include(p => p.PessoasTiposUsuario)
                .ThenInclude(pt => pt.TipoUsuario)
                .ToListAsync();
        }

        public async Task<List<Pessoa>> GetAvaliadoresAsync()
        {
            return await _context.Pessoas
                .Include(p => p.PessoasTiposUsuario)
                .ThenInclude(pt => pt.TipoUsuario)
                .Where(p => p.PessoasTiposUsuario.Any(pt => pt.TipoUsuario.PapelUsuario == "Avaliador"))
                .ToListAsync();
        }

        public async Task<Pessoa> CreateAsync(Pessoa pessoa)
        {
            _context.Pessoas.Add(pessoa);
            await _context.SaveChangesAsync();
            return pessoa;
        }

        public async Task<Pessoa> UpdateAsync(Pessoa pessoa)
        {
            _context.Pessoas.Update(pessoa);
            await _context.SaveChangesAsync();
            return pessoa;
        }

        public async Task DeleteAsync(int id)
        {
            var pessoa = await GetByIdAsync(id);
            if (pessoa != null)
            {
                _context.Pessoas.Remove(pessoa);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<List<string>> GetPapeisByPessoaIdAsync(int pessoaId)
        {
            return await _context.PessoasTiposUsuario
                .Where(pt => pt.IdPessoa == pessoaId)
                .Include(pt => pt.TipoUsuario)
                .Select(pt => pt.TipoUsuario.PapelUsuario)
                .ToListAsync();
        }

        public async Task AssociateTipoUsuarioAsync(int pessoaId, byte tipoUsuarioId)
        {
            var maxId = await _context.PessoasTiposUsuario.MaxAsync(pt => (int?)pt.Id) ?? 0;
            
            var pessoaTipoUsuario = new PessoaTipoUsuario
            {
                Id = maxId + 1,
                IdPessoa = pessoaId,
                IdTipoUsuario = tipoUsuarioId
            };

            _context.PessoasTiposUsuario.Add(pessoaTipoUsuario);
            await _context.SaveChangesAsync();
        }
    }
}