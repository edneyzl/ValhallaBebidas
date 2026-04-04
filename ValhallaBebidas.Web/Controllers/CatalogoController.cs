using Microsoft.AspNetCore.Mvc;

namespace ValhallaBebidas.Web.Controllers;

public class CatalogoController : Controller
{
    // ── /Catalogo ──────────────────────────────────────
    public IActionResult Index() => View();

    // ── /Catalogo/Produto?id=1 ─────────────────────────
    public IActionResult Produto([FromQuery] string? id)
    {
        if (string.IsNullOrEmpty(id))
            return RedirectToAction("Index");

        ViewData["ProdutoId"] = id;
        return View();
    }
}
