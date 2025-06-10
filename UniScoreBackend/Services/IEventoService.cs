using UniScore.API.DTOs;

namespace UniScore.API.Services
{
    public interface IEventoService
    {
        Task<EventoDto> CreateEventoAsync(CreateEventoDto request);
        Task<EventoDto> GetEventoByIdAsync(int id);
        Task<List<EventoDto>> GetAllEventosAsync();
        Task<List<EventoDto>> GetEventosAtivosAsync();
        Task<EventoDto> UpdateEventoAsync(int id, CreateEventoDto request);
        Task EncerrarEventoAsync(int id);
        Task DeleteEventoAsync(int id);
    }
}