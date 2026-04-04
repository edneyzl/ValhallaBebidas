using ValhallaBebidas.Domain.Enums;

namespace ValhallaBebidas.Domain.Interfaces;

public interface IDashboardRepository
{
    Task<decimal> TotalVendasConfirmadasAsync();
    Task<int> TotalClientesAsync();
    Task<int> TotalProdutosAsync();
    Task<int> TotalPedidosAsync();
    Task<int> PedidosPorStatusAsync(StatusPedido status);
    Task<IEnumerable<PedidosMes>> PedidosUltimosMesesAsync(int meses);
}

public class PedidosMes
{
    public int Ano { get; set; }
    public int Mes { get; set; }
    public int Total { get; set; }
}
