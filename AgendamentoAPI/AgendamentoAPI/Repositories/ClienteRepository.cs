using AgendamentoAPI.Interfaces;
using AgendamentoAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace AgendamentoAPI.Repositories;

public class ClienteRepository : IClienteRepository
{
    private readonly AgendamentoDbContext _context;

    public ClienteRepository(AgendamentoDbContext context)
    {
        _context = context;
    }

    public async Task AtualizarAsync(Cliente cliente)
    {
        _context.Clientes.Update(cliente);
        await _context.SaveChangesAsync();
    }

    public async Task CadastrarAsync(Cliente cliente)
    {
        await _context.Clientes.AddAsync(cliente);
        await _context.SaveChangesAsync();
    }

    public async Task DeletarAsync(Guid id)
    {
        var cliente = await _context.Clientes.FindAsync(id);
        if (cliente != null)
        {
            _context.Clientes.Remove(cliente);
            await _context.SaveChangesAsync();
        }
    }

    public async Task<List<Cliente>> ListarAsync()
    {
        return await _context.Clientes.ToListAsync();
    }

    public async Task<Cliente?> ObterPorEmailAsync(string email)
    {
        return await _context.Clientes.FirstOrDefaultAsync(c => c.Email == email);
    }

    public async Task<Cliente?> ObterPorIdAsync(Guid id)
    {
        return await _context.Clientes.FindAsync(id);
    }
}
