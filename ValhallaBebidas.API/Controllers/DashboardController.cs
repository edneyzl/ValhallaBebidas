using Microsoft.AspNetCore.Mvc;
using ValhallaBebidas.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using ValhallaBebidas.Infrastructure.Data;
namespace ValhallaBebidas.API.Controllers;


/// <summary>
/// Fornece dados agregados para o Dashboard da aplicação.
/// Endpoint: GET /api/dashboard
///
/// Retorna:
///   - TotalVendas:     soma dos totais de pedidos Finalizados
///   - TotalClientes:   número de clientes cadastrados
///   - TotalProdutos:   número de produtos cadastrados
///   - TotalPedidos:    número total de pedidos
///   - PedidosPorMes:   quantidade de pedidos por mês (últimos 6 meses)
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class DashboardController : ControllerBase
{

    private readonly ValhallaBebidasDbContext _db;

    public DashboardController(ValhallaBebidasDbContext db)
    {
        _db = db;
    }

    [HttpGet]
    public async Task<IActionResult> GetDashboard()
    {
        // Total de vendas = soma dos pedidos com status Finalizado
        var totalVendas = await _db.Pedidos
            .Where(p => p.Status == StatusPedido.Confirmado)
            .SumAsync(p => (decimal?)p.Total) ?? 0m;

        var totalClientes = await _db.Clientes.CountAsync();
        var totalProdutos = await _db.Produtos.CountAsync();
        var totalPedidos = await _db.Pedidos.CountAsync();
        var pedidosPend = await _db.Pedidos.CountAsync(p => p.Status == StatusPedido.Pendente);
        var pedidosConcl = await _db.Pedidos.CountAsync(p => p.Status == StatusPedido.Confirmado);
        var pedidosCanc = await _db.Pedidos.CountAsync(p => p.Status == StatusPedido.Confirmado);

        // Pedidos por mês (últimos 6 meses)
        var inicio = DateTime.Today.AddMonths(-5).Date;
        var pedidosPorMes = await _db.Pedidos
            .Where(p => p.DataPedido >= inicio)
            .GroupBy(p => new { p.DataPedido.Year, p.DataPedido.Month })
            .Select(g => new
            {
                Ano = g.Key.Year,
                Mes = g.Key.Month,
                Total = g.Count()
            })
            .OrderBy(g => g.Ano).ThenBy(g => g.Mes)
            .ToListAsync();

        return Ok(new
        {
            totalVendas,
            totalClientes,
            totalProdutos,
            totalPedidos,
            pedidosPendentes = pedidosPend,
            pedidosConcluidos = pedidosConcl,
            pedidosCancelados = pedidosCanc,
            pedidosPorMes
        });
    }
}
