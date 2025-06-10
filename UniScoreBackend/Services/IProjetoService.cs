using UniScore.API.DTOs;

namespace UniScore.API.Services
{
    public interface IProjetoService
    {
        Task<ProjetoDto> CreateProjetoAsync(CreateProjetoDto request);
        Task<ProjetoDto> GetProjetoByIdAsync(int id);
        Task<List<ProjetoDto>> GetProjetosByEventoAsync(int eventoId);
        Task<List<ProjetoParaAvaliarDto>> GetProjetosParaAvaliarAsync(int avaliadorId, int eventoId);
        Task<ProjetoDto> UpdateProjetoAsync(int id, CreateProjetoDto request);
        Task DeleteProjetoAsync(int id);
    }
}