using UniScore.API.Models;

namespace UniScore.API.Repositories
{
    public interface IEventoRepository
    {
        Task<Evento?> GetByIdAsync(int id);
        Task<List<Evento>> GetAllAsync();
        Task<List<Evento>> GetAtivosAsync();
        Task<Evento> CreateAsync(Evento evento);
        Task<Evento> UpdateAsync(Evento evento);
        Task DeleteAsync(int id);
        Task<List<CriterioAvaliacao>> GetCriteriosByEventoIdAsync(int eventoId);
        Task AddCriterioAsync(CriterioAvaliacao criterio);
    }
}