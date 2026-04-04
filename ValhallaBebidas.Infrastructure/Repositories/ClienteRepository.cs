using Microsoft.EntityFrameworkCore;
using ValhallaBebidas.Domain.Entities;
using ValhallaBebidas.Domain.Interfaces;
using ValhallaBebidas.Infrastructure.Data;

namespace ValhallaBebidas.Infrastructure.Repositories;

public class ClienteRepository : IClienteRepository
{
    private readonly ValhallaBebidasDbContext _context;

    public ClienteRepository(ValhallaBebidasDbContext context)
    {
        _context = context;
    }


    public async Task<Cliente?> ObterPorIdAsync(int id)
    => await _context.Clientes.FindAsync(id);

    public async Task<Cliente?> ObterPorDocumentoAsync(string doc)
    => await _context.Clientes.FirstOrDefaultAsync(c => c.Documento == doc);

    public async Task<Cliente?> ObterPorEmailAsync(string email)
    => await _context.Clientes.FirstOrDefaultAsync(c => c.Email == email);

    public async Task<IEnumerable<Cliente>> ListarTodosAsync()
    => await _context.Clientes.ToListAsync();

    public async Task AdicionarAsync(Cliente cliente)
    {
        await _context.Clientes.AddAsync(cliente);
    }

    public async Task AtualizarAsync(Cliente cliente)
    {
        _context.Clientes.Update(cliente);
    }

    public async Task RemoverAsync(int id)
    {
        var cliente = await ObterPorIdAsync(id);
        if (cliente != null)
        {
            _context.Clientes.Remove(cliente);
        }
    }
}
