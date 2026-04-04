using System.Collections.Concurrent;
using Microsoft.AspNetCore.Mvc;

namespace ValhallaBebidas.Web.Controllers;

public class CarrinhoController : Controller
{
    // In-memory store — substitui pela API quando pronto
    private static readonly ConcurrentDictionary<string, PedidoWeb> _pedidos = new();

    // ── Exibições ──────────────────────────────────────
    public IActionResult Index() => View();
    public IActionResult Checkout() => View();
    public IActionResult Confirmacao() => View();

    // ── POST /Carrinho/CheckoutApi ─────────────────────
    [HttpPost]
    public IActionResult CheckoutApi([FromBody] CheckoutPayload payload)
    {
        if (payload?.Itens == null || payload.Itens.Count == 0)
            return BadRequest(new { mensagem = "Carrinho vazio." });

        var id = Guid.NewGuid().ToString("N")[..8].ToUpper();

        _pedidos[id] = new PedidoWeb
        {
            Id = id,
            Itens = payload.Itens,
            Entrega = payload.Entrega,
            CriadoEm = DateTime.UtcNow
        };

        return Json(new { pedidoId = id, mensagem = "Pedido criado com sucesso." });
    }

    // ── GET /Carrinho/GetPedido?pedidoId=XXX ──────────
    [HttpGet]
    public IActionResult GetPedido([FromQuery] string? pedidoId)
    {
        if (string.IsNullOrEmpty(pedidoId))
            return BadRequest(new { mensagem = "Id do pedido não informado." });

        if (_pedidos.TryGetValue(pedidoId, out var pedido))
            return Json(pedido);

        return NotFound(new { mensagem = $"Pedido '{pedidoId}' não encontrado." });
    }
}

// ── DTOs ──────────────────────────────────────────────
public record CheckoutPayload(
    List<ItemPedidoWeb> Itens,
    EnderecoEntrega Entrega
);

public record ItemPedidoWeb(
    string ProdutoId,
    int Quantidade,
    string Nome,
    decimal Preco,
    string? Imagem
);

public record EnderecoEntrega(
    string Logradouro,
    string Numero,
    string? Complemento,
    string Bairro,
    string Cidade,
    string Estado,
    string Cep
);

public class PedidoWeb
{
    public string Id { get; set; } = string.Empty;
    public List<ItemPedidoWeb> Itens { get; set; } = [];
    public EnderecoEntrega? Entrega { get; set; }
    public DateTime CriadoEm { get; set; }
}