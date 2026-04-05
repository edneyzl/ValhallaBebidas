using Microsoft.AspNetCore.Mvc;
using ValhallaBebidas.Web.Filters;

namespace ValhallaBebidas.Web.Controllers;

[TypeFilter(typeof(AuthFilter))]
public class PedidosController : Controller
{
    private readonly IHttpClientFactory _httpClientFactory;

    public PedidosController(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }

    public IActionResult Index() => View();

    /* ── GET /Pedidos/MeusPedidosApi — chamado pelo JS ── */
    [HttpGet]
    public async Task<IActionResult> MeusPedidosApi()
    {
        var clienteId = HttpContext.Session.GetString("UserId");
        if (string.IsNullOrEmpty(clienteId))
            return Unauthorized();

        try
        {
            var client = _httpClientFactory.CreateClient("ValhallaAPI");
            var response = await client.GetAsync($"api/pedido/cliente/{clienteId}");
            var data = await response.Content.ReadAsStringAsync();
            return Content(data, "application/json");
        }
        catch
        {
            return StatusCode(500);
        }
    }

    /* ── POST /Pedidos/Cancelar/{id} — protegido por AuthFilter ── */
    [HttpPost("Cancelar/{id}")]
    public async Task<IActionResult> CancelarPedido(int id)
    {
        var clienteId = HttpContext.Session.GetString("UserId");
        if (string.IsNullOrEmpty(clienteId))
            return Unauthorized();

        try
        {
            var client = _httpClientFactory.CreateClient("ValhallaAPI");
            var response = await client.PostAsync($"api/pedido/{id}/cancelar?clienteId={clienteId}", null);

            if (!response.IsSuccessStatusCode)
            {
                var data = await response.Content.ReadAsStringAsync();
                return BadRequest(new { mensagem = data });
            }

            return Ok(new { mensagem = "Pedido cancelado com sucesso." });
        }
        catch
        {
            return StatusCode(500, new { mensagem = "Erro ao cancelar pedido." });
        }
    }
}