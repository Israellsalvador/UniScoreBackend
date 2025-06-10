using UniScore.API.DTOs;
using UniScore.API.Repositories;

namespace UniScore.API.Services
{
    public class RelatorioService : IRelatorioService
    {
        private readonly IProjetoRepository _projetoRepository;
        private readonly IEventoRepository _eventoRepository;
        private readonly IEventoService _eventoService;

        public RelatorioService(
            IProjetoRepository projetoRepository,
            IEventoRepository eventoRepository,
            IEventoService eventoService)
        {
            _projetoRepository = projetoRepository;
            _eventoRepository = eventoRepository;
            _eventoService = eventoService;
        }

        public async Task<List<RankingDto>> GetRankingEventoAsync(int eventoId)
        {
            var projetos = await _projetoRepository.GetByEventoIdAsync(eventoId);
            
            var ranking = projetos
                .Where(p => p.NotaFinal.HasValue)
                .Select(p => new RankingDto
                {
                    IdProjeto = p.IdProjeto,
                    NomeProjeto = p.NomeProjeto,
                    Integrantes = p.Integrantes,
                    Turma = p.Turma,
                    NotaFinal = p.NotaFinal ?? 0,
                    TotalAvaliacoes = p.Avaliacoes.Count
                })
                .OrderByDescending(r => r.NotaFinal)
                .ToList();

            // Adicionar posições
            for (int i = 0; i < ranking.Count; i++)
            {
                ranking[i].Posicao = i + 1;
            }

            return ranking;
        }

        public async Task<RelatorioEventoDto> GetRelatorioEventoAsync(int eventoId)
        {
            var evento = await _eventoService.GetEventoByIdAsync(eventoId);
            var ranking = await GetRankingEventoAsync(eventoId);
            var estatisticas = await GetEstatisticasEventoAsync(eventoId);

            return new RelatorioEventoDto
            {
                Evento = evento,
                Ranking = ranking,
                Estatisticas = estatisticas
            };
        }

        public async Task<EstatisticasEventoDto> GetEstatisticasEventoAsync(int eventoId)
        {
            var projetos = await _projetoRepository.GetByEventoIdAsync(eventoId);
            
            var totalProjetos = projetos.Count;
            var totalAvaliacoes = projetos.Sum(p => p.Avaliacoes.Count);
            var totalAvaliadores = projetos
                .SelectMany(p => p.Avaliacoes)
                .Select(a => a.IdPessoa)
                .Distinct()
                .Count();

            var todasNotas = projetos
                .Where(p => p.NotaFinal.HasValue)
                .Select(p => p.NotaFinal!.Value)
                .ToList();

            var mediaGeral = todasNotas.Any() ? todasNotas.Average() : 0;
            var notaMaisAlta = todasNotas.Any() ? todasNotas.Max() : 0;
            var notaMaisBaixa = todasNotas.Any() ? todasNotas.Min() : 0;

            return new EstatisticasEventoDto
            {
                TotalProjetos = totalProjetos,
                TotalAvaliacoes = totalAvaliacoes,
                TotalAvaliadores = totalAvaliadores,
                MediaGeralNotas = mediaGeral,
                NotaMaisAlta = notaMaisAlta,
                NotaMaisBaixa = notaMaisBaixa
            };
        }
    }
}