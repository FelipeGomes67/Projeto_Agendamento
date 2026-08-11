using AgendamentoAPI.Models;

namespace AgendamentoAPI.Interfaces;

public interface IServicoRepository
{
    Task CadastrarAsync(Servico servico);
    Task DeletarAsync(Guid id);
    Task<List<Servico>> ListarAsync();
    Task<Servico?> ObterPorIdAsync(Guid id);
    Task AtualizarAsync(Servico servico);
}