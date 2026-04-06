using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
namespace ValhallaBebidas.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ImagemController : ControllerBase
{
    private readonly IWebHostEnvironment _environment;

    public ImagemController(IWebHostEnvironment environment)
    {
        _environment = environment;
    }

    [HttpGet("{folder}/{file}")]
    public IActionResult GetImagem(string folder, string file)
    {
        // Garante que só possa acessar pastas permitidas por segurança
        if (folder != "usuarios" && folder != "produtos")
            return BadRequest("Pasta inválida.");

        var pathRoot = _environment.WebRootPath ?? _environment.ContentRootPath;
        var caminhoFisico = Path.Combine(pathRoot, "uploads", folder, file);

        if (!System.IO.File.Exists(caminhoFisico))
            return NotFound();

        // Determina o Content-Type baseado na extensão
        var extensao = Path.GetExtension(file).ToLowerInvariant();
        string contentType = extensao switch
        {
            ".jpg" or ".jpeg" => "image/jpeg",
            ".png" => "image/png",
            _ => "application/octet-stream"
        };

        return PhysicalFile(caminhoFisico, contentType);
    }
}
