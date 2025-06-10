using UniScore.API.DTOs;
using UniScore.API.Models;
using UniScore.API.Repositories;

namespace UniScore.API.Services
{
    public class ProjetoService : IProjetoService
    {
        private readonly IProjetoRepository _projetoRepository;
        private readonly IEventoRepository _eventoRepository;

        public ProjetoService(IProjetoRepository projetoRepository, IEventoRepository eventoRepository)
        {
            _projetoRepository = projetoRepository;
            _eventoRepository = eventoRepository;
        }

        public async Task<ProjetoDto> CreateProjetoAsync(CreateProjetoDto request)
        {
            var projeto = new Projeto
            {
                NomeProjeto = request.NomeProjeto,
                DescricaoProjeto = request.DescricaoProjeto,
                Integrantes = request.Integrantes,
                Turma = request.Turma
            };

            var createdProjeto = await _projetoRepository.CreateAsync(projeto);

            // Associar ao evento
            await _projetoRepository.AssociarEventoAsync(createdProjeto.IdProjeto, request.IdEvento);

            // Adicionar informações complementares
            foreach (var info in request.InformacoesComplementares)
            {
                var infoComplementar = new InformacoesComplementares
                {
                    InformacaoComplementar = info,
                    IdProjeto = createdProjeto.IdProjeto
                };
                // Adicionar ao contexto (seria necessário um repository para isso)
            }

            return await GetProjetoByIdAsync(createdProjeto.IdProjeto);
        }

        public async Task<ProjetoDto> GetProjetoByIdAsync(int id)
        {
            var projeto = await _projetoRepository.GetByIdAsync(id);
            if (projeto == null)
            {
                throw new ArgumentException("Projeto não encontrado");
            }

            return MapToDto(projeto);
        }

        public async Task<List<ProjetoDto>> GetProjetosByEventoAsync(int eventoId)
        {
            var projetos = await _projetoRepository.GetByEventoIdAsync(eventoId);
            return projetos.Select(MapToDto).ToList();
        }

        public async Task<List<ProjetoParaAvaliarDto>> GetProjetosParaAvaliarAsync(int avaliadorId, int eventoId)
        {
            var projetos = await _projetoRepository.GetProjetosParaAvaliarAsync(avaliadorId, eventoId);
            var evento = await _eventoRepository.GetByIdAsync(eventoId);

            return projetos.Select(p => new ProjetoParaAvaliarDto
            {
                IdProjeto = p.IdProjeto,
                NomeProjeto = p.NomeProjeto,
                DescricaoProjeto = p.DescricaoProjeto,
                Integrantes = p.Integrantes,
                Turma = p.Turma,
                NomeEvento = evento?.NomeEvento ?? "",
                InformacoesComplementares = p.InformacoesComplementares.Select(ic => ic.InformacaoComplementar).ToList(),
                JaAvaliado = p.Avaliacoes.Any(a => a.IdPessoa == avaliadorId)
            }).ToList();
        }

        public async Task<ProjetoDto> UpdateProjetoAsync(int id, CreateProjetoDto request)
        {
            var projeto = await _projetoRepository.GetByIdAsync(id);
            if (projeto == null)
            {
                throw new ArgumentException("Projeto não encontrado");
            }

            projeto.NomeProjeto = request.NomeProjeto;
            projeto.DescricaoProjeto = request.DescricaoProjeto;
            projeto.Integrantes = request.Integrantes;
            projeto.Turma = request.Turma;

            await _projetoRepository.UpdateAsync(projeto);
            return await GetProjetoByIdAsync(id);
        }

        public async Task DeleteProjetoAsync(int id)
        {
            var projeto = await _projetoRepository.GetByIdAsync(id);
            if (projeto == null)
            {
                throw new ArgumentException("Projeto não encontrado");
            }

            await _projetoRepository.DeleteAsync(id);
        }

        private ProjetoDto MapToDto(Projeto projeto)
        {
            var eventoNome = projeto.EventosProjetos.FirstOrDefault()?.Evento?.NomeEvento ?? "";
            var eventoId = projeto.EventosProjetos.FirstOrDefault()?.IdEvento ?? 0;

            return new ProjetoDto
            {
                IdProjeto = projeto.IdProjeto,
                NomeProjeto = projeto.NomeProjeto,
                DescricaoProjeto = projeto.DescricaoProjeto,
                Integrantes = projeto.Integrantes,
                Turma = projeto.Turma,
                NotaFinal = projeto.NotaFinal,
                IdEvento = eventoId,
                NomeEvento = eventoNome,
                InformacoesComplementares = projeto.InformacoesComplementares.Select(ic => ic.InformacaoComplementar).ToList(),
                Avaliacoes = projeto.Avaliacoes.Select(a => new AvaliacaoResumoDto
                {
                    IdAvaliacao = a.IdAvaliacao,
                    NomeAvaliador = a.Pessoa.NomePessoa,
                    DataAvaliacao = a.DataAvaliacao,
                    Finalizada = a.Finalizada,
                    NotaTotal = a.Notas.Sum(n => n.NotaProjeto)
                }).ToList()
            };
        }
    }
}