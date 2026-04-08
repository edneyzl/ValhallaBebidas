using Microsoft.EntityFrameworkCore;
using ValhallaBebidas.Domain.Entities;
using ValhallaBebidas.Domain.Interfaces;
using ValhallaBebidas.Infrastructure.Data;

namespace ValhallaBebidas.Infrastructure.Repositories;

public class PedidoRepository : IPedidoRepository
{
    private readonly ValhallaBebidasDbContext _context;

    public PedidoRepository(ValhallaBebidasDbContext context)
    {
        _context = context;
    }


    public async Task<Pedido?> ObterPorIdAsync(int id)
        => await _context.Pedidos
            .Include(p => p.Cliente)          // Carrega o cliente junto
            .Include(p => p.Itens)            // Carrega os itens junto
            .ThenInclude(i => i.Produto)  // Carrega o produto de cada item
            .FirstOrDefaultAsync(p => p.Id == id);

    public async Task<IEnumerable<Pedido>> ListarTodosAsync()
        => await _context.Pedidos
            .Include(p => p.Cliente)
            .Include(p => p.Itens)
                .ThenInclude(i => i.Produto)
            .ToListAsync();

    public async Task<IEnumerable<Pedido>> ListarPorClienteAsync(int clienteId)
        => await _context.Pedidos
            .Include(p => p.Itens)
            .ThenInclude(i => i.Produto)
            .Where(p => p.ClienteId == clienteId)
            .ToListAsync();

    public async Task AdicionarAsync(Pedido pedido)
    {
        await _context.Pedidos.AddAsync(pedido);
        await _context.SaveChangesAsync();
    }

    public async Task AtualizarAsync(Pedido pedido)
    {
        _context.Pedidos.Update(pedido);
        await _context.SaveChangesAsync();
    }

    public async Task RemoverAsync(int id)
    {
        var pedido = await ObterPorIdAsync(id);
        if (pedido != null)
        {
            _context.Pedidos.Remove(pedido);
            await _context.SaveChangesAsync();
        }
    }
}
