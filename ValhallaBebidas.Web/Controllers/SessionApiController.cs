using Microsoft.AspNetCore.Mvc;

namespace ValhallaBebidas.Web.Controllers;

[Route("api/session")]
[ApiController]
public class SessionApiController : ControllerBase
{
    /* JS chama GET /api/session para saber se está logado */
    [HttpGet]
    public IActionResult GetSession()
    {
        var userId = HttpContext.Session.GetString("UserId");
        if (string.IsNullOrEmpty(userId))
            return Ok(new { isLogado = false, nome = (string?)null });

        return Ok(new
        {
            isLogado = true,
            userId = int.Parse(userId),
            nome = HttpContext.Session.GetString("UserNome"),
            email = HttpContext.Session.GetString("UserEmail"),
        });
    }
}

