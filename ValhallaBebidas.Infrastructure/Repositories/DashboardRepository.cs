using Microsoft.EntityFrameworkCore;
using ValhallaBebidas.Domain.Entities;
using ValhallaBebidas.Domain.Enums;
using ValhallaBebidas.Domain.Interfaces;
using ValhallaBebidas.Infrastructure.Data;

namespace ValhallaBebidas.Infrastructure.Repositories;

public class DashboardRepository : IDashboardRepository
{
    private readonly ValhallaBebidasDbContext _db;

    public DashboardRepository(ValhallaBebidasDbContext db)
    {
        _db = db;
    }

    public async Task<decimal> TotalVendasConfirmadasAsync()
        => await _db.Pedidos
            .Where(p => p.Status == StatusPedido.Confirmado)
            .SumAsync(p => (decimal?)p.ValorTotal) ?? 0m;

    public async Task<int> TotalClientesAsync()
        => await _db.Clientes.CountAsync();

    public async Task<int> TotalProdutosAsync()
        => await _db.Produtos.CountAsync();

    public async Task<int> TotalPedidosAsync()
        => await _db.Pedidos.CountAsync();

    public async Task<int> PedidosPorStatusAsync(StatusPedido status)
        => await _db.Pedidos.CountAsync(p => p.Status == status);

    public async Task<IEnumerable<PedidosMes>> PedidosUltimosMesesAsync(int meses)
    {
        var inicio = DateTime.UtcNow.AddMonths(-meses + 1).Date;
        return await _db.Pedidos
            .Where(p => p.DataPedido >= inicio)
            .GroupBy(p => new { p.DataPedido.Year, p.DataPedido.Month })
            .Select(g => new PedidosMes
            {
                Ano = g.Key.Year,
                Mes = g.Key.Month,
                Total = g.Count()
            })
            .OrderBy(g => g.Ano).ThenBy(g => g.Mes)
            .ToListAsync();
    }
}
