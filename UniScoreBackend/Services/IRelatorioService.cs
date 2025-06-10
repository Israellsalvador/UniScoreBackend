using UniScore.API.DTOs;

namespace UniScore.API.Services
{
    public interface IRelatorioService
    {
        Task<List<RankingDto>> GetRankingEventoAsync(int eventoId);
        Task<RelatorioEventoDto> GetRelatorioEventoAsync(int eventoId);
        Task<EstatisticasEventoDto> GetEstatisticasEventoAsync(int eventoId);
    }

    public class RankingDto
    {
        public int Posicao { get; set; }
        public int IdProjeto { get; set; }
        public string NomeProjeto { get; set; } = string.Empty;
        public string Integrantes { get; set; } = string.Empty;
        public string Turma { get; set; } = string.Empty;
        public float NotaFinal { get; set; }
        public int TotalAvaliacoes { get; set; }
    }

    public class RelatorioEventoDto
    {
        public EventoDto Evento { get; set; } = new();
        public List<RankingDto> Ranking { get; set; } = new();
        public EstatisticasEventoDto Estatisticas { get; set; } = new();
    }

    public class EstatisticasEventoDto
    {
        public int TotalProjetos { get; set; }
        public int TotalAvaliacoes { get; set; }
        public int TotalAvaliadores { get; set; }
        public float MediaGeralNotas { get; set; }
        public float NotaMaisAlta { get; set; }
        public float NotaMaisBaixa { get; set; }
    }
}