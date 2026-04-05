using Microsoft.AspNetCore.Mvc;
using ValhallaBebidas.Application.DTOs;

namespace ValhallaBebidas.Web.Controllers;

/// <summary>
/// Proxy que redireciona chamadas JS /api/* para a API real.
/// As rotas sao mapeadas exatamente como o JS espera.
/// </summary>
[ApiController]
public class ApiProxyController : ControllerBase
{
    private readonly IHttpClientFactory _httpClientFactory;

    public ApiProxyController(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }

    /* ── GET /api/produto ── */
    [HttpGet("api/produto")]
    public async Task<IActionResult> ListarProdutos()
    {
        var client = _httpClientFactory.CreateClient("ValhallaAPI");
        var response = await client.GetAsync("api/produto");
        if (!response.IsSuccessStatusCode)
            return StatusCode(502, new { mensagem = "API indisponível." });
        var data = await response.Content.ReadAsStringAsync();
        return Content(data, "application/json");
    }

    /* ── GET /api/produto/{id} ── */
    [HttpGet("api/produto/{id}")]
    public async Task<IActionResult> ObterProduto(int id)
    {
        var client = _httpClientFactory.CreateClient("ValhallaAPI");
        var response = await client.GetAsync($"api/produto/{id}");
        if (!response.IsSuccessStatusCode)
            return StatusCode(502, new { mensagem = "API indisponível." });
        var data = await response.Content.ReadAsStringAsync();
        return Content(data, "application/json");
    }

    /* ── GET /api/pedido ── */
    [HttpGet("api/pedido")]
    public async Task<IActionResult> ListarPedidos()
    {
        var client = _httpClientFactory.CreateClient("ValhallaAPI");
        var response = await client.GetAsync("api/pedido");
        if (!response.IsSuccessStatusCode)
            return StatusCode(502, new { mensagem = "API indisponível." });
        var data = await response.Content.ReadAsStringAsync();
        return Content(data, "application/json");
    }

    /* ── GET /api/pedido/cliente/{clienteId} ── */
    [HttpGet("api/pedido/cliente/{clienteId}")]
    public async Task<IActionResult> PedidosPorCliente(int clienteId)
    {
        var client = _httpClientFactory.CreateClient("ValhallaAPI");
        var response = await client.GetAsync($"api/pedido/cliente/{clienteId}");
        if (!response.IsSuccessStatusCode)
            return StatusCode(502, new { mensagem = "API indisponível." });
        var data = await response.Content.ReadAsStringAsync();
        return Content(data, "application/json");
    }

    /* ── GET /api/pedido/{id} ── */
    [HttpGet("api/pedido/{id}")]
    public async Task<IActionResult> ObterPedido(int id)
    {
        var client = _httpClientFactory.CreateClient("ValhallaAPI");
        var response = await client.GetAsync($"api/pedido/{id}");
        if (!response.IsSuccessStatusCode)
            return StatusCode(502, new { mensagem = "API indisponível." });
        var data = await response.Content.ReadAsStringAsync();
        return Content(data, "application/json");
    }

    /* ── POST /api/pedido (criar) ── */
    [HttpPost("api/pedido")]
    public async Task<IActionResult> CriarPedido([FromBody] CriarPedidoDto payload)
    {
        var client = _httpClientFactory.CreateClient("ValhallaAPI");
        var response = await client.PostAsJsonAsync("api/pedido", payload);
        var data = await response.Content.ReadAsStringAsync();

        if (!response.IsSuccessStatusCode)
            return StatusCode((int)response.StatusCode, data);

        return Content(data, "application/json");
    }

    /* ── PUT /api/pedido/{id}/status ── */
    [HttpPut("api/pedido/{id}/status")]
    public async Task<IActionResult> AtualizarStatus(int id, [FromBody] AtualizarStatusDto payload)
    {
        if (string.IsNullOrEmpty(payload?.NovoStatus))
            return BadRequest(new { mensagem = "campo 'novoStatus' é obrigatório." });

        var client = _httpClientFactory.CreateClient("ValhallaAPI");
        var response = await client.PutAsJsonAsync($"api/pedido/{id}/status", payload);
        var data = await response.Content.ReadAsStringAsync();

        if (!response.IsSuccessStatusCode)
            return StatusCode((int)response.StatusCode, data);

        return NoContent();
    }
}
