using UniScore.API.Models;

namespace UniScore.API.Repositories
{
    public interface IAvaliacaoRepository
    {
        Task<Avaliacao?> GetByIdAsync(int id);
        Task<List<Avaliacao>> GetByAvaliadorIdAsync(int avaliadorId);
        Task<List<Avaliacao>> GetByProjetoIdAsync(int projetoId);
        Task<Avaliacao> CreateAsync(Avaliacao avaliacao);
        Task<Avaliacao> UpdateAsync(Avaliacao avaliacao);
        Task<bool> JaAvaliouAsync(int avaliadorId, int projetoId);
        Task AddNotaAsync(Nota nota);
    }
}