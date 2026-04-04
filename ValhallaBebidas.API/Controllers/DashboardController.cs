using Microsoft.AspNetCore.Mvc;
using ValhallaBebidas.Domain.Enums;
using ValhallaBebidas.Domain.Interfaces;

namespace ValhallaBebidas.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class DashboardController : ControllerBase
{
    private readonly IDashboardRepository _repository;

    public DashboardController(IDashboardRepository repository)
    {
        _repository = repository;
    }

    [HttpGet]
    public async Task<IActionResult> GetDashboard()
    {
        var totalVendas = await _repository.TotalVendasConfirmadasAsync();
        var totalClientes = await _repository.TotalClientesAsync();
        var totalProdutos = await _repository.TotalProdutosAsync();
        var totalPedidos = await _repository.TotalPedidosAsync();

        var pedidosPend = await _repository.PedidosPorStatusAsync(StatusPedido.Pendente);
        var pedidosConcl = await _repository.PedidosPorStatusAsync(StatusPedido.Confirmado);
        var pedidosCanc = await _repository.PedidosPorStatusAsync(StatusPedido.Cancelado);

        var pedidosPorMes = await _repository.PedidosUltimosMesesAsync(6);

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
