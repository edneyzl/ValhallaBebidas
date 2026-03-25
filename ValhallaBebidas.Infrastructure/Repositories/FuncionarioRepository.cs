using Microsoft.EntityFrameworkCore;
using ValhallaBebidas.Domain.Entities;
using ValhallaBebidas.Domain.Interfaces;
using ValhallaBebidas.Infrastructure.Data;

namespace ValhallaBebidas.Infrastructure.Repositories;

public class FuncionarioRepository : IFuncionarioRepository
{


    private readonly ValhallaBebidasDbContext _context;

    public FuncionarioRepository(ValhallaBebidasDbContext context)
    {
        _context = context;
    }


    public async Task<Funcionario?> ObterPorIdAsync(int id)
        => await _context.Funcionarios.FindAsync(id);

    public async Task<Funcionario?> ObterPorCpfAsync(string cpf)
        => await _context.Funcionarios.FindAsync(cpf);

    public async Task<IEnumerable<Funcionario>> ListarTodosAsync()
        => await _context.Funcionarios.ToListAsync();

    public async Task AdicionarAsync(Funcionario funcionario)
    {
        await _context.Funcionarios.AddAsync(funcionario);
        await _context.SaveChangesAsync();
    }

    public async Task AtualizarAsync(Funcionario funcionario)
    {
        _context.Funcionarios.Update(funcionario);
        await _context.SaveChangesAsync();
    }

    public async Task RemoverAsync(int id)
    {
        var funcionario = await ObterPorIdAsync(id);
        if (funcionario != null)
        {
            _context.Funcionarios.Remove(funcionario);
            await _context.SaveChangesAsync();
        }
    }
}
