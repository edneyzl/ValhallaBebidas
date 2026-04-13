using Microsoft.AspNetCore.Mvc;

namespace ValhallaBebidas.Web.Controllers;

[ApiController]
public class ApiProxyController : ControllerBase
{
    private readonly IHttpClientFactory _httpClientFactory;

    public ApiProxyController(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }

    /* =========================
       PRODUTO
    ========================= */

    [HttpGet("api/produto")]
    public async Task<IActionResult> ListarProdutos()
    {
        try
        {
            var client = _httpClientFactory.CreateClient("ValhallaAPI");
            var response = await client.GetAsync("api/produto");
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

    [HttpGet("api/produto/{id}")]
    public async Task<IActionResult> ObterProduto(int id)
    {
        try
        {
            var client = _httpClientFactory.CreateClient("ValhallaAPI");
            var response = await client.GetAsync($"api/produto/{id}");
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

    /* =========================
       CLIENTE
    ========================= */

    [HttpGet("api/cliente/{id}")]
    public async Task<IActionResult> ObterCliente(int id)
    {
        try
        {
            var client = _httpClientFactory.CreateClient("ValhallaAPI");
            var response = await client.GetAsync($"api/cliente/{id}");
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

    [HttpPut("api/cliente/{id}")]
    public async Task<IActionResult> AtualizarCliente(int id, [FromBody] object payload)
    {
        try
        {
            var client = _httpClientFactory.CreateClient("ValhallaAPI");
            var response = await client.PutAsJsonAsync($"api/cliente/{id}", payload);
            var body = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
                return StatusCode((int)response.StatusCode, body);

            return Content(string.IsNullOrWhiteSpace(body) ? "{\"sucesso\":true}" : body, "application/json");
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { mensagem = ex.Message });
        }
    }

    [HttpPut("api/cliente/{id}/endereco")]
    public async Task<IActionResult> AtualizarEndereco(int id, [FromBody] object payload)
    {
        try
        {
            var client = _httpClientFactory.CreateClient("ValhallaAPI");
            var response = await client.PutAsJsonAsync($"api/cliente/{id}/endereco", payload);
            var body = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
                return StatusCode((int)response.StatusCode, body);

            return Content(string.IsNullOrWhiteSpace(body) ? "{\"sucesso\":true}" : body, "application/json");
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { mensagem = ex.Message });
        }
    }

    [HttpPut("api/cliente/{id}/senha")]
    public async Task<IActionResult> AtualizarSenha(int id, [FromBody] object payload)
    {
        try
        {
            var client = _httpClientFactory.CreateClient("ValhallaAPI");
            var response = await client.PutAsJsonAsync($"api/cliente/{id}/senha", payload);
            var body = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
                return StatusCode((int)response.StatusCode, body);

            return Content(string.IsNullOrWhiteSpace(body) ? "{\"sucesso\":true}" : body, "application/json");
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { mensagem = ex.Message });
        }
    }
}