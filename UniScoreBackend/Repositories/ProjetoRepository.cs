using Microsoft.EntityFrameworkCore;
using UniScore.API.Data;
using UniScore.API.Models;

namespace UniScore.API.Repositories
{
    public class ProjetoRepository : IProjetoRepository
    {
        private readonly UniscoreContext _context;

        public ProjetoRepository(UniscoreContext context)
        {
            _context = context;
        }

        public async Task<Projeto?> GetByIdAsync(int id)
        {
            return await _context.Projetos
                .Include(p => p.InformacoesComplementares)
                .Include(p => p.Avaliacoes)
                .ThenInclude(a => a.Pessoa)
                .Include(p => p.Avaliacoes)
                .ThenInclude(a => a.Notas)
                .Include(p => p.EventosProjetos)
                .ThenInclude(ep => ep.Evento)
                .FirstOrDefaultAsync(p => p.IdProjeto == id);
        }

        public async Task<List<Projeto>> GetAllAsync()
        {
            return await _context.Projetos
                .Include(p => p.InformacoesComplementares)
                .Include(p => p.EventosProjetos)
                .ThenInclude(ep => ep.Evento)
                .ToListAsync();
        }

        public async Task<List<Projeto>> GetByEventoIdAsync(int eventoId)
        {
            return await _context.EventosProjetos
                .Where(ep => ep.IdEvento == eventoId)
                .Include(ep => ep.Projeto)
                .ThenInclude(p => p.InformacoesComplementares)
                .Include(ep => ep.Projeto)
                .ThenInclude(p => p.Avaliacoes)
                .ThenInclude(a => a.Pessoa)
                .Select(ep => ep.Projeto)
                .ToListAsync();
        }

        public async Task<List<Projeto>> GetProjetosParaAvaliarAsync(int avaliadorId, int eventoId)
        {
            // Buscar projetos do evento que ainda não foram avaliados por este avaliador
            var projetosJaAvaliados = await _context.Avaliacoes
                .Where(a => a.IdPessoa == avaliadorId)
                .Select(a => a.IdProjeto)
                .ToListAsync();

            return await _context.EventosProjetos
                .Where(ep => ep.IdEvento == eventoId && !projetosJaAvaliados.Contains(ep.IdProjeto))
                .Include(ep => ep.Projeto)
                .ThenInclude(p => p.InformacoesComplementares)
                .Include(ep => ep.Evento)
                .Select(ep => ep.Projeto)
                .ToListAsync();
        }

        public async Task<Projeto> CreateAsync(Projeto projeto)
        {
            _context.Projetos.Add(projeto);
            await _context.SaveChangesAsync();
            return projeto;
        }

        public async Task<Projeto> UpdateAsync(Projeto projeto)
        {
            _context.Projetos.Update(projeto);
            await _context.SaveChangesAsync();
            return projeto;
        }

        public async Task DeleteAsync(int id)
        {
            var projeto = await GetByIdAsync(id);
            if (projeto != null)
            {
                _context.Projetos.Remove(projeto);
                await _context.SaveChangesAsync();
            }
        }

        public async Task AssociarEventoAsync(int projetoId, int eventoId)
        {
            var maxId = await _context.EventosProjetos.MaxAsync(ep => (int?)ep.Id) ?? 0;
            
            var eventoProjeto = new EventoProjeto
            {
                Id = maxId + 1,
                IdEvento = eventoId,
                IdProjeto = projetoId
            };

            _context.EventosProjetos.Add(eventoProjeto);
            await _context.SaveChangesAsync();
        }

        public async Task AssociarAvaliadorAsync(int projetoId, int avaliadorId)
        {
            // Esta funcionalidade pode ser implementada se necessário
            // Por enquanto, qualquer avaliador pode avaliar qualquer projeto do evento
            await Task.CompletedTask;
        }
    }
}