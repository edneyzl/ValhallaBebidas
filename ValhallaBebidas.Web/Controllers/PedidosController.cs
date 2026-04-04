using Microsoft.AspNetCore.Mvc;

namespace ValhallaBebidas.Web.Controllers;

public class PedidosController : Controller
{
    // ── /Pedidos ───────────────────────────────────────
    public IActionResult Index() => View();
}
