using ValhallaBebidas.Domain.Entities;

namespace ValhallaBebidas.Domain.Interfaces;

public interface IPedidoRepository
{
    Task<Pedido?> ObterPorIdAsync(int id);
    Task<IEnumerable<Pedido>> ListarTodosAsync();
    Task<IEnumerable<Pedido>> ListarPorClienteAsync(int clienteId);
    Task AdicionarAsync(Pedido pedido);
    Task AtualizarAsync(Pedido pedido);
    Task RemoverAsync(int id);
    Task SaveAsync();
}