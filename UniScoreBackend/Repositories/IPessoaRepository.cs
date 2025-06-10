using UniScore.API.Models;

namespace UniScore.API.Repositories
{
    public interface IPessoaRepository
    {
        Task<Pessoa?> GetByIdAsync(int id);
        Task<Pessoa?> GetByUsuarioAsync(string usuario);
        Task<List<Pessoa>> GetAllAsync();
        Task<List<Pessoa>> GetAvaliadoresAsync();
        Task<Pessoa> CreateAsync(Pessoa pessoa);
        Task<Pessoa> UpdateAsync(Pessoa pessoa);
        Task DeleteAsync(int id);
        Task<List<string>> GetPapeisByPessoaIdAsync(int pessoaId);
        Task AssociateTipoUsuarioAsync(int pessoaId, byte tipoUsuarioId);
    }
}