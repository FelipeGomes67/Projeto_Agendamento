using AgendamentoAPI.Interfaces;
using AgendamentoAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace AgendamentoAPI.Repositories
{
    public class AgendamentoRepository : IAgendamentoRepository
    {
        private readonly AgendamentoDbContext _context;

        public AgendamentoRepository(AgendamentoDbContext context)
        {
            _context = context;
        }

        public async Task CadastrarAsync(Agendamento agendamento)
        {
            await _context.Agendamentos.AddAsync(agendamento);
            await _context.SaveChangesAsync();
        }

        public async Task<List<Agendamento>> ListarAsync()
        {
            return await _context.Agendamentos
                .Include(a => a.IdClienteNavigation)
                .Include(a => a.IdProfissionalNavigation)
                .Include(a => a.IdServicoNavigation)
                .ToListAsync();
        }

        public async Task<Agendamento?> ObterPorIdAsync(Guid id)
        {
            return await _context.Agendamentos
                .Include(a => a.IdClienteNavigation)
                .Include(a => a.IdProfissionalNavigation)
                .Include(a => a.IdServicoNavigation)
                .FirstOrDefaultAsync(a => a.IdAgendamento == id);
        }

        public async Task AtualizarAsync(Agendamento agendamento)
        {
            _context.Agendamentos.Update(agendamento);
            await _context.SaveChangesAsync();
        }

        public async Task DeletarAsync(Guid id)
        {
            var agendamento = await ObterPorIdAsync(id);
            if (agendamento != null)
            {
                _context.Agendamentos.Remove(agendamento);
                await _context.SaveChangesAsync();
            }
        }
    }
}