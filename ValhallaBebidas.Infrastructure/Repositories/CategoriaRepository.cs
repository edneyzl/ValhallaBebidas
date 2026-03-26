using ValhallaBebidas.Domain.Entities;
using ValhallaBebidas.Infrastructure.Data;

namespace ValhallaBebidas.Infrastructure.Repositories;

public class CategoriaRepository
{
    private readonly ValhallaBebidasDbContext _context;

    public CategoriaRepository(ValhallaBebidasDbContext context)
    {
        _context = context;
    }

    public async Task<Categoria?> ObterPorIdAsync(int id)
        => await _context.Categorias.FindAsync(id);

    public async Task<Categoria?> ObterPorNomeAsync(string nome)
        => await _context.Categorias.FindAsync(nome);

    public async Task<IEnumerable<Categoria>> ListarTodosAsync()
        => await _context.Categorias.ToListAsync();/*CORRIGIR essa merda*/

    public async Task AdicionarAsync(Categoria categoria)
    {
        await _context.Categorias.AddAsync(categoria);
        await _context.SaveChangesAsync();
    }

    public async Task AtualizarAsync(Categoria categoria)
    {
        _context.Categorias.Update(categoria);
        await _context.SaveChangesAsync();
    }

    public async Task RemoverAsync(int id)
    {
        var categoria = await ObterPorIdAsync(id);
        if (categoria != null)
        {
            _context.Categorias.Remove(categoria);
            await _context.SaveChangesAsync();
        }
    }
}
