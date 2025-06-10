using UniScore.API.DTOs;
using UniScore.API.Models;
using UniScore.API.Repositories;

namespace UniScore.API.Services
{
    public class AvaliacaoService : IAvaliacaoService
    {
        private readonly IAvaliacaoRepository _avaliacaoRepository;
        private readonly IEventoRepository _eventoRepository;
        private readonly IProjetoRepository _projetoRepository;

        public AvaliacaoService(
            IAvaliacaoRepository avaliacaoRepository,
            IEventoRepository eventoRepository,
            IProjetoRepository projetoRepository)
        {
            _avaliacaoRepository = avaliacaoRepository;
            _eventoRepository = eventoRepository;
            _projetoRepository = projetoRepository;
        }

        public async Task<AvaliacaoDto> CreateAvaliacaoAsync(int avaliadorId, CreateAvaliacaoDto request)
        {
            // Verificar se já avaliou
            var jaAvaliou = await _avaliacaoRepository.JaAvaliouAsync(avaliadorId, request.IdProjeto);
            if (jaAvaliou)
            {
                throw new ArgumentException("Você já avaliou este projeto");
            }

            var avaliacao = new Avaliacao
            {
                IdPessoa = avaliadorId,
                IdProjeto = request.IdProjeto,
                DataAvaliacao = DateTime.Now,
                Finalizada = true
            };

            var createdAvaliacao = await _avaliacaoRepository.CreateAsync(avaliacao);

            // Adicionar notas
            var maxId = 0;
            foreach (var notaDto in request.Notas)
            {
                var nota = new Nota
                {
                    Id = ++maxId,
                    IdCriterio = notaDto.IdCriterio,
                    IdAvaliacao = createdAvaliacao.IdAvaliacao,
                    NotaProjeto = notaDto.NotaProjeto
                };
                await _avaliacaoRepository.AddNotaAsync(nota);
            }

            // Atualizar nota final do projeto
            await AtualizarNotaFinalProjeto(request.IdProjeto);

            return await GetAvaliacaoByIdAsync(createdAvaliacao.IdAvaliacao);
        }

        public async Task<List<AvaliacaoDto>> GetMinhasAvaliacoesAsync(int avaliadorId)
        {
            var avaliacoes = await _avaliacaoRepository.GetByAvaliadorIdAsync(avaliadorId);
            return avaliacoes.Select(MapToDto).ToList();
        }

        public async Task<List<AvaliacaoDto>> GetAvaliacoesByProjetoAsync(int projetoId)
        {
            var avaliacoes = await _avaliacaoRepository.GetByProjetoIdAsync(projetoId);
            return avaliacoes.Select(MapToDto).ToList();
        }

        public async Task<CriteriosEventoDto> GetCriteriosEventoAsync(int eventoId)
        {
            var evento = await _eventoRepository.GetByIdAsync(eventoId);
            if (evento == null)
            {
                throw new ArgumentException("Evento não encontrado");
            }

            return new CriteriosEventoDto
            {
                IdEvento = evento.IdEvento,
                NomeEvento = evento.NomeEvento,
                NotaMinima = evento.NotaMinima,
                NotaMaxima = evento.NotaMaxima,
                Criterios = evento.CriteriosAvaliacao.Select(c => new CriterioDto
                {
                    IdCriterio = c.IdCriterio,
                    NomeCriterio = c.NomeCriterio
                }).ToList()
            };
        }

        public async Task<bool> JaAvaliouAsync(int avaliadorId, int projetoId)
        {
            return await _avaliacaoRepository.JaAvaliouAsync(avaliadorId, projetoId);
        }

        private async Task<AvaliacaoDto> GetAvaliacaoByIdAsync(int id)
        {
            var avaliacao = await _avaliacaoRepository.GetByIdAsync(id);
            if (avaliacao == null)
            {
                throw new ArgumentException("Avaliação não encontrada");
            }

            return MapToDto(avaliacao);
        }

        private async Task AtualizarNotaFinalProjeto(int projetoId)
        {
            var projeto = await _projetoRepository.GetByIdAsync(projetoId);
            if (projeto == null) return;

            var todasNotas = projeto.Avaliacoes.SelectMany(a => a.Notas).Select(n => n.NotaProjeto);
            if (todasNotas.Any())
            {
                projeto.NotaFinal = todasNotas.Sum(); // Soma de todas as notas
                await _projetoRepository.UpdateAsync(projeto);
            }
        }

        private AvaliacaoDto MapToDto(Avaliacao avaliacao)
        {
            return new AvaliacaoDto
            {
                IdAvaliacao = avaliacao.IdAvaliacao,
                IdProjeto = avaliacao.IdProjeto,
                NomeProjeto = avaliacao.Projeto.NomeProjeto,
                NomeAvaliador = avaliacao.Pessoa.NomePessoa,
                DataAvaliacao = avaliacao.DataAvaliacao,
                Finalizada = avaliacao.Finalizada,
                Notas = avaliacao.Notas.Select(n => new NotaDetalheDto
                {
                    IdCriterio = n.IdCriterio,
                    NomeCriterio = n.CriterioAvaliacao.NomeCriterio,
                    NotaProjeto = n.NotaProjeto
                }).ToList(),
                NotaTotal = avaliacao.Notas.Sum(n => n.NotaProjeto)
            };
        }
    }
}