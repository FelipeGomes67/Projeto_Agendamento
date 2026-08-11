using AgendamentoAPI.Interfaces;
using AgendamentoAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace AgendamentoAPI.Repositories
{
    public class ProfissionalRepository : IProfissionalRepository
    {
        private readonly AgendamentoDbContext _context;

        public ProfissionalRepository(AgendamentoDbContext context)
        {
            _context = context;
        }

        public async Task CadastrarAsync(Profissional profissional)
        {
            await _context.Profissionais.AddAsync(profissional);
            await _context.SaveChangesAsync();
        }

        public async Task<List<Profissional>> ListarAsync()
        {
            return await _context.Profissionais.ToListAsync();
        }

        public async Task<Profissional?> ObterPorIdAsync(Guid id)
        {
            return await _context.Profissionais.FindAsync(id);
        }

        public async Task<Profissional?> ObterPorEmailAsync(string email)
        {
            return await _context.Profissionais.FirstOrDefaultAsync(p => p.Email == email);
        }

        public async Task AtualizarAsync(Profissional profissional)
        {
            _context.Profissionais.Update(profissional);
            await _context.SaveChangesAsync();
        }

        public async Task DeletarAsync(Guid id)
        {
            var profissional = await ObterPorIdAsync(id);
            if (profissional != null)
            {
                _context.Profissionais.Remove(profissional);
                await _context.SaveChangesAsync();
            }
        }
    }
}