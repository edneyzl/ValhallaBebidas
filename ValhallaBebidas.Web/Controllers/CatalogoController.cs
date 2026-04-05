using Microsoft.AspNetCore.Mvc;

namespace ValhallaBebidas.Web.Controllers;

public class CatalogoController : Controller
{
    /* ── /Catalogo — público (JS carrega produtos via /api/produto) ── */
    public IActionResult Index() => View();

    /* ── /Catalogo/Produto?id=1 — público (JS carrega via /api/produto/{id}) ── */
    public IActionResult Produto([FromQuery] string? id)
    {
        if (string.IsNullOrEmpty(id))
            return RedirectToAction("Index");

        ViewData["ProdutoId"] = id;
        return View();
    }
}