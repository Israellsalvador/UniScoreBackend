using UniScore.API.DTOs;
using UniScore.API.Models;
using UniScore.API.Repositories;

namespace UniScore.API.Services
{
    public class EventoService : IEventoService
    {
        private readonly IEventoRepository _eventoRepository;

        public EventoService(IEventoRepository eventoRepository)
        {
            _eventoRepository = eventoRepository;
        }

        public async Task<EventoDto> CreateEventoAsync(CreateEventoDto request)
        {
            var evento = new Evento
            {
                NomeEvento = request.NomeEvento,
                DescricaoEvento = request.DescricaoEvento,
                Local = request.Local,
                DataInicio = request.DataInicio,
                DataFim = request.DataFim,
                NotaMinima = request.NotaMinima,
                NotaMaxima = request.NotaMaxima,
                Ativo = true
            };

            var createdEvento = await _eventoRepository.CreateAsync(evento);

            // Adicionar critérios
            foreach (var criterioNome in request.Criterios)
            {
                var criterio = new CriterioAvaliacao
                {
                    IdEvento = createdEvento.IdEvento,
                    NomeCriterio = criterioNome
                };
                await _eventoRepository.AddCriterioAsync(criterio);
            }

            return await GetEventoByIdAsync(createdEvento.IdEvento);
        }

        public async Task<EventoDto> GetEventoByIdAsync(int id)
        {
            var evento = await _eventoRepository.GetByIdAsync(id);
            if (evento == null)
            {
                throw new ArgumentException("Evento não encontrado");
            }

            return new EventoDto
            {
                IdEvento = evento.IdEvento,
                NomeEvento = evento.NomeEvento,
                DescricaoEvento = evento.DescricaoEvento,
                Local = evento.Local,
                DataInicio = evento.DataInicio,
                DataFim = evento.DataFim,
                Ativo = evento.Ativo,
                NotaMinima = evento.NotaMinima,
                NotaMaxima = evento.NotaMaxima,
                Criterios = evento.CriteriosAvaliacao.Select(c => new CriterioDto
                {
                    IdCriterio = c.IdCriterio,
                    NomeCriterio = c.NomeCriterio
                }).ToList(),
                TotalProjetos = evento.EventosProjetos.Count,
                TotalAvaliacoes = evento.EventosProjetos.Sum(ep => ep.Projeto.Avaliacoes.Count)
            };
        }

        public async Task<List<EventoDto>> GetAllEventosAsync()
        {
            var eventos = await _eventoRepository.GetAllAsync();
            return eventos.Select(MapToDto).ToList();
        }

        public async Task<List<EventoDto>> GetEventosAtivosAsync()
        {
            var eventos = await _eventoRepository.GetAtivosAsync();
            return eventos.Select(MapToDto).ToList();
        }

        public async Task<EventoDto> UpdateEventoAsync(int id, CreateEventoDto request)
        {
            var evento = await _eventoRepository.GetByIdAsync(id);
            if (evento == null)
            {
                throw new ArgumentException("Evento não encontrado");
            }

            evento.NomeEvento = request.NomeEvento;
            evento.DescricaoEvento = request.DescricaoEvento;
            evento.Local = request.Local;
            evento.DataInicio = request.DataInicio;
            evento.DataFim = request.DataFim;
            evento.NotaMinima = request.NotaMinima;
            evento.NotaMaxima = request.NotaMaxima;

            await _eventoRepository.UpdateAsync(evento);
            return await GetEventoByIdAsync(id);
        }

        public async Task EncerrarEventoAsync(int id)
        {
            var evento = await _eventoRepository.GetByIdAsync(id);
            if (evento == null)
            {
                throw new ArgumentException("Evento não encontrado");
            }

            evento.Ativo = false;
            evento.DataFim = DateTime.Now;
            await _eventoRepository.UpdateAsync(evento);
        }

        public async Task DeleteEventoAsync(int id)
        {
            var evento = await _eventoRepository.GetByIdAsync(id);
            if (evento == null)
            {
                throw new ArgumentException("Evento não encontrado");
            }

            await _eventoRepository.DeleteAsync(id);
        }

        private EventoDto MapToDto(Evento evento)
        {
            return new EventoDto
            {
                IdEvento = evento.IdEvento,
                NomeEvento = evento.NomeEvento,
                DescricaoEvento = evento.DescricaoEvento,
                Local = evento.Local,
                DataInicio = evento.DataInicio,
                DataFim = evento.DataFim,
                Ativo = evento.Ativo,
                NotaMinima = evento.NotaMinima,
                NotaMaxima = evento.NotaMaxima,
                Criterios = evento.CriteriosAvaliacao.Select(c => new CriterioDto
                {
                    IdCriterio = c.IdCriterio,
                    NomeCriterio = c.NomeCriterio
                }).ToList(),
                TotalProjetos = evento.EventosProjetos.Count,
                TotalAvaliacoes = evento.EventosProjetos.Sum(ep => ep.Projeto.Avaliacoes.Count)
            };
        }
    }
}