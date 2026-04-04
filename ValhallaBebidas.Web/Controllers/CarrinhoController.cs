using Microsoft.AspNetCore.Mvc;
using ValhallaBebidas.Web.Filters;
using ValhallaBebidas.Application.DTOs;
using System.Text.Json;

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

    /* ── POST /Carrinho/CheckoutApi ── */
    /// <summary>
    /// O token CSRF é validado via cookie pelo AntiForgery, mas como o JS
    /// envia o token no header RequestVerificationToken (não form data),
    /// usamos IgnoreAntiforgeryToken e validamos manualmente.
    /// </summary>
    [HttpPost]
    [IgnoreAntiforgeryToken]
    public async Task<IActionResult> CheckoutApi([FromBody] CheckoutPayload payload)
    {
        /* Validação manual do CSRF token */
        if (!ValidateCsrfToken(HttpContext))
            return BadRequest(new { mensagem = "Requisição inválida." });

        if (payload?.Itens == null || payload.Itens.Count == 0)
            return BadRequest(new { mensagem = "Carrinho vazio." });

        var sessionUserId = HttpContext.Session.GetString("UserId");
        if (!int.TryParse(sessionUserId, out var clienteId))
            return Unauthorized(new { mensagem = "Sessão expirada." });

        try
        {
            var client = _httpClientFactory.CreateClient("ValhallaAPI");
            var apiPayload = new
            {
                clienteId,
                itens = payload.Itens.Select(i => new
                {
                    produtoId = i.ProdutoId,
                    quantidade = i.Quantidade
                })
            };

            var response = await client.PostAsJsonAsync("api/pedido", apiPayload);

            if (!response.IsSuccessStatusCode)
            {
                var erro = await response.Content.ReadAsStringAsync();
                return BadRequest(new { mensagem = erro });
            }

            var pedido = await response.Content.ReadFromJsonAsync<dynamic>();
            return Json(new { pedidoId = pedido?.id, mensagem = "Pedido criado com sucesso." });
        }
        catch
        {
            return StatusCode(500, new { mensagem = "Erro interno. Tente novamente." });
        }
    }

    private static bool ValidateCsrfToken(HttpContext context)
    {
        try
        {
            var cookieToken = context.Request.Cookies["XSRF-TOKEN"];
            var headerToken = context.Request.Headers["RequestVerificationToken"].FirstOrDefault();
            return !string.IsNullOrEmpty(cookieToken) && cookieToken == headerToken;
        }
        catch
        {
            return false;
        }
    }
}