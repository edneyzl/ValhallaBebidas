using ValhallaBebidas.Domain.Entities;

namespace ValhallaBebidas.Domain.Interfaces;

public interface IMovimentacaoRepository
{
    Task<IEnumerable<Movimentacao>> ListarTodosAsync();
    Task<IEnumerable<Movimentacao>> ListarPorProdutoAsync(int produtoId);
    Task<Movimentacao?> ObterPorIdAsync(int id);
    Task AdicionarAsync(Movimentacao movimentacao);
    Task RemoverAsync(int id);
    Task SaveAsync();
}