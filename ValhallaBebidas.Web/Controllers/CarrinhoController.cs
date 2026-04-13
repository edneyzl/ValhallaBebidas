using Microsoft.AspNetCore.Mvc;
using ValhallaBebidas.Web.Filters;
using ValhallaBebidas.Application.DTOs;

namespace ValhallaBebidas.Web.Controllers;

[TypeFilter(typeof(AuthFilter))]
public class CarrinhoController : Controller
{
    private readonly IHttpClientFactory _httpClientFactory;

    public CarrinhoController(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }

    public IActionResult Index() => View();
    public IActionResult Checkout() => View();
    public IActionResult Confirmacao() => View();

    [HttpGet]
    public async Task<IActionResult> ObterPedidoApi(int id)
    {
        var clienteId = HttpContext.Session.GetString("UserId");
        if (string.IsNullOrEmpty(clienteId))
            return Unauthorized();

        try
        {
            var client = _httpClientFactory.CreateClient("ValhallaAPI");
            var response = await client.GetAsync($"api/pedido/{id}");
            var body = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
                return StatusCode((int)response.StatusCode, body);

            return Content(body, "application/json");
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { mensagem = ex.Message });
        }
    }

    [HttpPost]
    [IgnoreAntiforgeryToken]
    public async Task<IActionResult> CheckoutApi([FromBody] CriarPedidoDto payload)
    {
        if (payload?.Itens == null || payload.Itens.Count == 0)
            return BadRequest(new { mensagem = "Carrinho vazio." });

        var sessionUserId = HttpContext.Session.GetString("UserId");
        if (!int.TryParse(sessionUserId, out var clienteId))
            return Unauthorized(new { mensagem = "Sessão expirada." });

        try
        {
            payload.ClienteId = clienteId;

            var client = _httpClientFactory.CreateClient("ValhallaAPI");
            var response = await client.PostAsJsonAsync("api/pedido", payload);

            var respostaTexto = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
            {
                return StatusCode((int)response.StatusCode, new
                {
                    mensagem = string.IsNullOrWhiteSpace(respostaTexto)
                        ? "Erro retornado pela API de pedidos."
                        : respostaTexto
                });
            }

            var pedido = await response.Content.ReadFromJsonAsync<PedidoCriadoResponseDto>();

            return Json(new
            {
                pedidoId = pedido?.Id,
                mensagem = "Pedido criado com sucesso."
            });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { mensagem = ex.Message });
        }
    }
}

public class PedidoCriadoResponseDto
{
    public int Id { get; set; }
}