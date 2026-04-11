using Microsoft.EntityFrameworkCore;
using ValhallaBebidas.Domain.Entities;
using ValhallaBebidas.Domain.Interfaces;
using ValhallaBebidas.Infrastructure.Data;

namespace ValhallaBebidas.Infrastructure.Repositories;

public class MovimentacaoRepository : IMovimentacaoRepository
{
    private readonly ValhallaBebidasDbContext _context;

    public MovimentacaoRepository(ValhallaBebidasDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Movimentacao>> ListarTodosAsync()
        => await _context.Movimentacoes
            .Include(m => m.Produto)
            .OrderByDescending(m => m.Data)
            .ToListAsync();

    public async Task<IEnumerable<Movimentacao>> ListarPorProdutoAsync(int produtoId)
        => await _context.Movimentacoes
            .Include(m => m.Produto)
            .Where(m => m.ProdutoId == produtoId)
            .OrderByDescending(m => m.Data)
            .ToListAsync();

    public async Task<Movimentacao?> ObterPorIdAsync(int id)
        => await _context.Movimentacoes
            .Include(m => m.Produto)
            .FirstOrDefaultAsync(m => m.Id == id);

    public async Task AdicionarAsync(Movimentacao movimentacao)
    {
        await _context.Movimentacoes.AddAsync(movimentacao);
    }

    public async Task RemoverAsync(int id)
    {
        var movimentacao = await ObterPorIdAsync(id);
        if (movimentacao != null)
        {
            _context.Movimentacoes.Remove(movimentacao);
        }
    }

    public async Task SaveAsync()
    {
        await _context.SaveChangesAsync();
    }
}