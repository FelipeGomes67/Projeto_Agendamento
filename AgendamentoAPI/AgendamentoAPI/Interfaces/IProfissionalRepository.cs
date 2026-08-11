using AgendamentoAPI.Models;

namespace AgendamentoAPI.Interfaces;

public interface IProfissionalRepository
{
    Task CadastrarAsync(Profissional profissional);
    Task DeletarAsync(Guid id);
    Task<List<Profissional>> ListarAsync();
    Task<Profissional?> ObterPorIdAsync(Guid id);
    Task<Profissional?> ObterPorEmailAsync(string email);
    Task AtualizarAsync(Profissional profissional);
}