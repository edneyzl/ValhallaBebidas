using Microsoft.AspNetCore.Mvc;

namespace ValhallaBebidas.Web.Controllers;

public class ContaController : Controller
{
    // ── /Conta/Perfil ──────────────────────────────────
    public IActionResult Perfil() => View();
}