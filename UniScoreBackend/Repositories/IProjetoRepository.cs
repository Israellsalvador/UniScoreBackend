using UniScore.API.Models;

namespace UniScore.API.Repositories
{
    public interface IProjetoRepository
    {
        Task<Projeto?> GetByIdAsync(int id);
        Task<List<Projeto>> GetAllAsync();
        Task<List<Projeto>> GetByEventoIdAsync(int eventoId);
        Task<List<Projeto>> GetProjetosParaAvaliarAsync(int avaliadoreId, int eventoId);
        Task<Projeto> CreateAsync(Projeto projeto);
        Task<Projeto> UpdateAsync(Projeto projeto);
        Task DeleteAsync(int id);
        Task AssociarEventoAsync(int projetoId, int eventoId);
        Task AssociarAvaliadorAsync(int projetoId, int avaliadoreId);
    }
}