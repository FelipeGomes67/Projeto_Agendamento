using AgendamentoAPI.Models;

namespace AgendamentoAPI.Interfaces;

public interface IClienteRepository
{
    Task CadastrarAsync(Cliente cliente);
    Task DeletarAsync(Guid id);
    Task<List<Cliente>> ListarAsync();
    Task<Cliente?> ObterPorIdAsync(Guid id);
    Task<Cliente?> ObterPorEmailAsync(string email);
    Task AtualizarAsync(Cliente cliente);
}