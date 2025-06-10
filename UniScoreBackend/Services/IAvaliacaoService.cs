using UniScore.API.DTOs;

namespace UniScore.API.Services
{
    public interface IAvaliacaoService
    {
        Task<AvaliacaoDto> CreateAvaliacaoAsync(int avaliadorId, CreateAvaliacaoDto request);
        Task<List<AvaliacaoDto>> GetMinhasAvaliacoesAsync(int avaliadorId);
        Task<List<AvaliacaoDto>> GetAvaliacoesByProjetoAsync(int projetoId);
        Task<CriteriosEventoDto> GetCriteriosEventoAsync(int eventoId);
        Task<bool> JaAvaliouAsync(int avaliadorId, int projetoId);
    }
}