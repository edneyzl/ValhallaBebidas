using Microsoft.AspNetCore.Mvc;
using ValhallaBebidas.Web.Filters;

namespace ValhallaBebidas.Web.Controllers;

[TypeFilter(typeof(AuthFilter))]
public class ContaController : Controller
{
    public IActionResult Perfil()
    {
        /* Passa dados da sessão para a View */
        ViewBag.UserNome = HttpContext.Session.GetString("UserNome");
        ViewBag.UserEmail = HttpContext.Session.GetString("UserEmail");
        ViewBag.UserId = HttpContext.Session.GetString("UserId");
        return View();
    }
}
