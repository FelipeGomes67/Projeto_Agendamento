using AgendamentoAPI.Interfaces;
using AgendamentoAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace AgendamentoAPI.Repositories
{
    public class ServicoRepository : IServicoRepository
    {
        private readonly AgendamentoDbContext _context;

        public ServicoRepository(AgendamentoDbContext context)
        {
            _context = context;
        }

        public async Task CadastrarAsync(Servico servico)
        {
            await _context.Servicos.AddAsync(servico);
            await _context.SaveChangesAsync();
        }

        public async Task<List<Servico>> ListarAsync()
        {
            return await _context.Servicos.ToListAsync();
        }

        public async Task<Servico?> ObterPorIdAsync(Guid id)
        {
            return await _context.Servicos.FindAsync(id);
        }

        public async Task AtualizarAsync(Servico servico)
        {
            _context.Servicos.Update(servico);
            await _context.SaveChangesAsync();
        }

        public async Task DeletarAsync(Guid id)
        {
            var servico = await ObterPorIdAsync(id);
            if (servico != null)
            {
                _context.Servicos.Remove(servico);
                await _context.SaveChangesAsync();
            }
        }
    }
}