using Microsoft.AspNetCore.Mvc;

namespace ValhallaBebidas.Web.Controllers;

public class CatalogoController : Controller
{
    private readonly IHttpClientFactory _httpClientFactory;

    public CatalogoController(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }

    public IActionResult Index() => View();

    public IActionResult Produto([FromQuery] string? id)
    {
        if (string.IsNullOrWhiteSpace(id))
            return RedirectToAction(nameof(Index));

        ViewData["ProdutoId"] = id;
        return View();
    }

    [HttpGet]
    public async Task<IActionResult> ListarProdutos([FromQuery] int? categoriaId)
    {
        var client = _httpClientFactory.CreateClient("ValhallaAPI");

        var url = categoriaId.HasValue
            ? $"api/Produto/categoria/{categoriaId.Value}"
            : "api/Produto";

        var response = await client.GetAsync(url);

        if (!response.IsSuccessStatusCode)
        {
            return StatusCode((int)response.StatusCode, new
            {
                mensagem = "Erro ao buscar produtos na API."
            });
        }

        var json = await response.Content.ReadAsStringAsync();
        return Content(json, "application/json");
    }

    [HttpGet]
    public async Task<IActionResult> ObterProduto([FromQuery] int id)
    {
        var client = _httpClientFactory.CreateClient("ValhallaAPI");

        var response = await client.GetAsync($"api/Produto/{id}");

        if (!response.IsSuccessStatusCode)
        {
            return StatusCode((int)response.StatusCode, new
            {
                mensagem = $"Produto {id} não encontrado."
            });
        }

        var json = await response.Content.ReadAsStringAsync();
        return Content(json, "application/json");
    }
}