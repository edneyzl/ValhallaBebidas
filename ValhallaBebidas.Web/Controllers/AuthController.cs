using Microsoft.AspNetCore.Mvc;
using ValhallaBebidas.Web.Filters;
using ValhallaBebidas.Web.Models;
using System.Text.Json;

namespace ValhallaBebidas.Web.Controllers;

public class AuthController : Controller
{
    private readonly IHttpClientFactory _httpClientFactory;

    public AuthController(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }

    /* ── GET /Auth/Login ── */
    public IActionResult Login()
    {
        /* Se já logado redireciona para home */
        if (!string.IsNullOrEmpty(HttpContext.Session.GetString("UserId")))
            return RedirectToAction("Index", "Home");

        return View();
    }

    /* ── POST /Auth/Login ── */
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Login([FromForm] LoginFormModel model)
    {
        if (!ModelState.IsValid)
            return View(model);

        try
        {
            var client = _httpClientFactory.CreateClient("ValhallaAPI");
            var payload = new { email = model.Email, senha = model.Senha };
            var response = await client.PostAsJsonAsync("api/auth/login-cliente", payload);
            var data = await response.Content.ReadFromJsonAsync<LoginResponseModel>();

            if (!response.IsSuccessStatusCode || data == null || !data.Sucesso)
            {
                ViewBag.Erro = data?.Mensagem ?? "Email ou senha inválidos.";
                return View(model);
            }

            /* Salva na Session — não no localStorage */
            HttpContext.Session.SetString("UserId", data.Id.ToString());
            HttpContext.Session.SetString("UserNome", data.Nome);
            HttpContext.Session.SetString("UserEmail", data.Email);

            return RedirectToAction("Index", "Home");
        }
        catch
        {
            ViewBag.Erro = "Erro de conexão. Tente novamente.";
            return View(model);
        }
    }

    /* ── GET /Auth/Cadastro ── */
    public IActionResult Cadastro()
    {
        if (!string.IsNullOrEmpty(HttpContext.Session.GetString("UserId")))
            return RedirectToAction("Index", "Home");

        return View();
    }

    /* ── POST /Auth/Cadastro ── */
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Cadastro([FromForm] CadastroFormModel model)
    {
        if (!ModelState.IsValid)
            return View(model);

        /* Converte DD/MM/YYYY para DateTime */
        if (!DateTime.TryParseExact(model.DataNascimento, "dd/MM/yyyy",
            System.Globalization.CultureInfo.InvariantCulture,
            System.Globalization.DateTimeStyles.None, out var dataNasc))
        {
            ViewBag.Erro = "Data de nascimento inválida.";
            return View(model);
        }

        /* Monta payload no formato que o CriarClienteDto espera */
        var payload = new
        {
            nome = model.Nome,
            dataNascimento = dataNasc,
            documento = model.Documento,
            telefone = model.Telefone,
            email = model.Email,
            senha = model.Senha,
            endereco = new
            {
                tipoLogradouro = "",
                logradouro = model.Endereco.Logradouro,
                numero = model.Endereco.Numero,
                complemento = model.Endereco.Complemento ?? "",
                cep = model.Endereco.Cep,
                bairro = model.Endereco.Bairro,
                cidade = model.Endereco.Cidade,
                estado = model.Endereco.Estado,
            }
        };

        try
        {
            var client = _httpClientFactory.CreateClient("ValhallaAPI");
            var response = await client.PostAsJsonAsync("api/auth/cadastro", payload);
            var data = await response.Content.ReadFromJsonAsync<LoginResponseModel>();

            if (!response.IsSuccessStatusCode || data == null || !data.Sucesso)
            {
                ViewBag.Erro = data?.Mensagem ?? "Erro ao criar conta.";
                return View(model);
            }

            /* Auto-loga após cadastro */
            HttpContext.Session.SetString("UserId", data.Id.ToString());
            HttpContext.Session.SetString("UserNome", data.Nome);
            HttpContext.Session.SetString("UserEmail", data.Email);

            return RedirectToAction("Index", "Home");
        }
        catch
        {
            ViewBag.Erro = "Erro de conexão. Tente novamente.";
            return View(model);
        }
    }

    /* ── GET /Auth/Logout ── */
    public IActionResult Logout()
    {
        HttpContext.Session.Clear();
        return RedirectToAction("Index", "Home");
    }
}
