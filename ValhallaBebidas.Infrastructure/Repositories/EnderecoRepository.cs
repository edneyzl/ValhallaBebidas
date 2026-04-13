using Microsoft.EntityFrameworkCore;
using ValhallaBebidas.Domain.Entities;
using ValhallaBebidas.Domain.Interfaces;
using ValhallaBebidas.Infrastructure.Data;

namespace ValhallaBebidas.Infrastructure.Repositories;

public class EnderecoRepository : IEnderecoRepository
{

    private readonly ValhallaBebidasDbContext _context;

    public EnderecoRepository(ValhallaBebidasDbContext context)
    {
        _context = context;
    }


    public async Task<Endereco?> ObterPorIdAsync(int id)
        => await _context.Enderecos.FindAsync(id);

    public async Task<Endereco?> ObterPorCepAsync(string cep)
    => await _context.Enderecos.FirstOrDefaultAsync(e => e.Cep == cep);

    public async Task AdicionarAsync(Endereco endereco)
    {
        await _context.Enderecos.AddAsync(endereco);
        await _context.SaveChangesAsync();
    }

    public async Task AtualizarAsync(Endereco endereco)
    {
        _context.Enderecos.Update(endereco);
        await _context.SaveChangesAsync();
    }

    public async Task RemoverAsync(int id)
    {
        var endereco = await ObterPorIdAsync(id);
        if (endereco != null)
        {
            _context.Enderecos.Remove(endereco);
            await _context.SaveChangesAsync();
        }
    }
}
