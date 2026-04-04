using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace ValhallaBebidas.Web.Filters;

/// <summary>
/// Filter que bloqueia acesso a Controllers/Actions protegidos
/// caso o usuário não esteja autenticado na Session.
/// Uso: [TypeFilter(typeof(AuthFilter))] no controller ou action.
/// </summary>
public class AuthFilter : IActionFilter
{
    public void OnActionExecuting(ActionExecutingContext context)
    {
        var session = context.HttpContext.Session;
        var userId = session.GetString("UserId");

        if (string.IsNullOrEmpty(userId))
        {
            /* AJAX: retorna 401 (JS redireciona) */
            if (context.HttpContext.Request.Headers["X-Requested-With"] == "XMLHttpRequest"
                || context.HttpContext.Request.ContentType?.Contains("application/json") == true)
            {
                context.Result = new StatusCodeResult(401);
            }
            else
            {
                /* Navegação normal: redireciona para login */
                context.Result = new RedirectToActionResult("Login", "Auth", null);
            }
        }
    }

    public void OnActionExecuted(ActionExecutedContext context) { }
}
