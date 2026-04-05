using Microsoft.AspNetCore.Mvc;

namespace ValhallaBebidas.Web.Controllers;

public class HomeController : Controller
{
    public IActionResult Index() => View();
}