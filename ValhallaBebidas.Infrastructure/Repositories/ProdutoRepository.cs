using System.Linq;
using Microsoft.EntityFrameworkCore;
using ValhallaBebidas.Domain.Entities;
using ValhallaBebidas.Domain.Interfaces;
using ValhallaBebidas.Infrastructure.Data;

namespace ValhallaBebidas.Infrastructure.Repositories;

public class ProdutoRepository : IProdutoRepository
{
    private readonly ValhallaBebidasDbContext _context;

    public ProdutoRepository(ValhallaBebidasDbContext context)
    {
        _context = context;
    }

    public async Task<Produto?> ObterPorIdAsync(int id)
        => await _context.Produtos.FindAsync(id);

    public async Task<Produto?> ObterPorEanAsync(string ean)
    => await _context.Produtos.FirstOrDefaultAsync(p => p.Ean == ean);

    public async Task<IEnumerable<Produto>> ObterPorIdsAsync(IEnumerable<int> ids)
    {
        var idSet = ids.ToHashSet();
        return await _context.Produtos
            .Where(p => idSet.Contains(p.Id))
            .ToListAsync();
    }

    public async Task<IEnumerable<Produto>> ListarTodosAsync()
        => await _context.Produtos.ToListAsync();

    public async Task<IEnumerable<Produto>> ListarPorCategoriaAsync(int categoriaId)
        => await _context.Produtos
            .Where(p => p.CategoriaId == categoriaId)
            .ToListAsync();

    public async Task<IEnumerable<Produto>> ObterEstoqueBaixoAsync()
        => await _context.Produtos
            .Where(p => p.QuantidadeEstoque <= p.QuantidadeMinimo)
            .OrderBy(p => p.QuantidadeEstoque)
            .ToListAsync();

    public async Task<IEnumerable<Produto>> ListarAtivosAsync()
        => await _context.Produtos
            .Where(p => p.Status)
            .ToListAsync();

    public async Task AdicionarAsync(Produto produto)
    {
        await _context.Produtos.AddAsync(produto);
        await _context.SaveChangesAsync();
    }

    public async Task AtualizarAsync(Produto produto)
    {
        _context.Produtos.Update(produto);
        await _context.SaveChangesAsync();
    }

    public async Task RemoverAsync(int id)
    {
        var produto = await ObterPorIdAsync(id);
        if (produto != null)
        {
            _context.Produtos.Remove(produto);
            await _context.SaveChangesAsync();
        }
    }
}
