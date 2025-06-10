using Microsoft.EntityFrameworkCore;
using UniScore.API.Data;
using UniScore.API.Models;

namespace UniScore.API.Repositories
{
    public class AvaliacaoRepository : IAvaliacaoRepository
    {
        private readonly UniscoreContext _context;

        public AvaliacaoRepository(UniscoreContext context)
        {
            _context = context;
        }

        public async Task<Avaliacao?> GetByIdAsync(int id)
        {
            return await _context.Avaliacoes
                .Include(a => a.Pessoa)
                .Include(a => a.Projeto)
                .Include(a => a.Notas)
                .ThenInclude(n => n.CriterioAvaliacao)
                .FirstOrDefaultAsync(a => a.IdAvaliacao == id);
        }

        public async Task<List<Avaliacao>> GetByAvaliadorIdAsync(int avaliadorId)
        {
            return await _context.Avaliacoes
                .Include(a => a.Pessoa)
                .Include(a => a.Projeto)
                .Include(a => a.Notas)
                .ThenInclude(n => n.CriterioAvaliacao)
                .Where(a => a.IdPessoa == avaliadorId)
                .ToListAsync();
        }

        public async Task<List<Avaliacao>> GetByProjetoIdAsync(int projetoId)
        {
            return await _context.Avaliacoes
                .Include(a => a.Pessoa)
                .Include(a => a.Projeto)
                .Include(a => a.Notas)
                .ThenInclude(n => n.CriterioAvaliacao)
                .Where(a => a.IdProjeto == projetoId)
                .ToListAsync();
        }

        public async Task<Avaliacao> CreateAsync(Avaliacao avaliacao)
        {
            _context.Avaliacoes.Add(avaliacao);
            await _context.SaveChangesAsync();
            return avaliacao;
        }

        public async Task<Avaliacao> UpdateAsync(Avaliacao avaliacao)
        {
            _context.Avaliacoes.Update(avaliacao);
            await _context.SaveChangesAsync();
            return avaliacao;
        }

        public async Task<bool> JaAvaliouAsync(int avaliadorId, int projetoId)
        {
            return await _context.Avaliacoes
                .AnyAsync(a => a.IdPessoa == avaliadorId && a.IdProjeto == projetoId);
        }

        public async Task AddNotaAsync(Nota nota)
        {
            _context.Notas.Add(nota);
            await _context.SaveChangesAsync();
        }
    }
}