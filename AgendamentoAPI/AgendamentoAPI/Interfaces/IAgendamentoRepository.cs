using AgendamentoAPI.Models;

namespace AgendamentoAPI.Interfaces;

public interface IAgendamentoRepository
{
    Task CadastrarAsync(Agendamento agendamento);
    Task DeletarAsync(Guid id);
    Task<List<Agendamento>> ListarAsync();
    Task<Agendamento?> ObterPorIdAsync(Guid id);
    Task AtualizarAsync(Agendamento agendamento);
}