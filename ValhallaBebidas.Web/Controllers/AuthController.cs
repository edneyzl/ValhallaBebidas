using Microsoft.AspNetCore.Mvc;

namespace ValhallaBebidas.Web.Controllers;

public class AuthController : Controller
{
    // ── Exibições ──────────────────────────────────────
    public IActionResult Login() => View();
    public IActionResult Cadastro() => View();

    // ── Logout — limpa a sessão e redireciona ──────────
    public IActionResult Logout()
    {
        HttpContext.Session.Clear();
        return RedirectToAction("Index", "Home");
    }
}
