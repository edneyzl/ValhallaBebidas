using Microsoft.AspNetCore.Mvc;
using ValhallaBebidas.Web.Filters;
using System.Text.Json;

namespace ValhallaBebidas.Web.Controllers;

[TypeFilter(typeof(AuthFilter))]
[Route("[controller]")]
public class PedidosController : Controller
{
    private readonly IHttpClientFactory _httpClientFactory;

    public PedidosController(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }

    [HttpGet("")]
    public IActionResult Index() => View();

    [HttpGet("MeusPedidosApi")]
    public async Task<IActionResult> MeusPedidosApi()
    {
        var clienteId = HttpContext.Session.GetString("UserId");
        if (!int.TryParse(clienteId, out var id))
            return Unauthorized();

        try
        {
            var client = _httpClientFactory.CreateClient("ValhallaAPI");
            var response = await client.GetAsync($"api/pedido/cliente/{id}");

            var body = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
                return StatusCode((int)response.StatusCode, body);

            if (string.IsNullOrWhiteSpace(body))
                return Json(new object[0]);

            var pedidos = JsonSerializer.Deserialize<object>(body, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            return Json(pedidos);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { mensagem = ex.Message });
        }
    }

    [HttpPost("Cancelar/{id:int}")]
    public async Task<IActionResult> CancelarPedido(int id)
    {
        var clienteId = HttpContext.Session.GetString("UserId");
        if (string.IsNullOrEmpty(clienteId))
            return Unauthorized();

        try
        {
            var client = _httpClientFactory.CreateClient("ValhallaAPI");
            var response = await client.PostAsync($"api/pedido/{id}/cancelar?clienteId={clienteId}", null);

            var body = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
            {
                return StatusCode((int)response.StatusCode, new
                {
                    mensagem = string.IsNullOrWhiteSpace(body)
                        ? "Erro ao cancelar pedido."
                        : body
                });
            }

            return Ok(new { mensagem = "Pedido cancelado com sucesso." });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { mensagem = ex.Message });
        }
    }
}