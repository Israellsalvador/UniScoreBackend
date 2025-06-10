using Microsoft.EntityFrameworkCore;
using UniScore.API.Data;
using UniScore.API.Models;

namespace UniScore.API.Repositories
{
    public class EventoRepository : IEventoRepository
    {
        private readonly UniscoreContext _context;

        public EventoRepository(UniscoreContext context)
        {
            _context = context;
        }

        public async Task<Evento?> GetByIdAsync(int id)
        {
            return await _context.Eventos
                .Include(e => e.CriteriosAvaliacao)
                .Include(e => e.EventosProjetos)
                .ThenInclude(ep => ep.Projeto)
                .FirstOrDefaultAsync(e => e.IdEvento == id);
        }

        public async Task<List<Evento>> GetAllAsync()
        {
            return await _context.Eventos
                .Include(e => e.CriteriosAvaliacao)
                .Include(e => e.EventosProjetos)
                .OrderByDescending(e => e.DataInicio)
                .ToListAsync();
        }

        public async Task<List<Evento>> GetAtivosAsync()
        {
            return await _context.Eventos
                .Include(e => e.CriteriosAvaliacao)
                .Include(e => e.EventosProjetos)
                .Where(e => e.Ativo)
                .OrderByDescending(e => e.DataInicio)
                .ToListAsync();
        }

        public async Task<Evento> CreateAsync(Evento evento)
        {
            _context.Eventos.Add(evento);
            await _context.SaveChangesAsync();
            return evento;
        }

        public async Task<Evento> UpdateAsync(Evento evento)
        {
            _context.Eventos.Update(evento);
            await _context.SaveChangesAsync();
            return evento;
        }

        public async Task DeleteAsync(int id)
        {
            var evento = await GetByIdAsync(id);
            if (evento != null)
            {
                _context.Eventos.Remove(evento);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<List<CriterioAvaliacao>> GetCriteriosByEventoIdAsync(int eventoId)
        {
            return await _context.CriteriosAvaliacao
                .Where(c => c.IdEvento == eventoId)
                .ToListAsync();
        }

        public async Task AddCriterioAsync(CriterioAvaliacao criterio)
        {
            _context.CriteriosAvaliacao.Add(criterio);
            await _context.SaveChangesAsync();
        }
    }
}