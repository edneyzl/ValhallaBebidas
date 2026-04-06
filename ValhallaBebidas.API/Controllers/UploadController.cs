using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
namespace ValhallaBebidas.API.Controllers;


[ApiController]
[Route("api/[controller]")]
public class UploadController : ControllerBase
{
    private readonly IWebHostEnvironment _environment;

    // Injeta o IWebHostEnvironment para obter o caminho físico da aplicação (wwwroot)
    public UploadController(IWebHostEnvironment environment)
    {
        _environment = environment;
    }

    [HttpPost("usuario")]
    public async Task<IActionResult> UploadFotoUsuario(IFormFile file)
    {
        return await ProcessarUpload(file, "usuarios");
    }

    [HttpPost("produto")]
    public async Task<IActionResult> UploadFotoProduto(IFormFile file)
    {
        return await ProcessarUpload(file, "produtos");
    }

    private async Task<IActionResult> ProcessarUpload(IFormFile file, string subpasta)
    {
        if (file == null || file.Length == 0)
            return BadRequest(new { Mensagem = "Nenhum arquivo enviado." });

        // Validação de formato (apenas JPG/PNG)
        var extensao = Path.GetExtension(file.FileName).ToLowerInvariant();
        if (extensao != ".jpg" && extensao != ".jpeg" && extensao != ".png")
            return BadRequest(new { Mensagem = "Formato de arquivo não suportado. Apenas JPG e PNG são permitidos." });

        // Validação de tamanho (ex: max 5MB)
        if (file.Length > 5 * 1024 * 1024)
            return BadRequest(new { Mensagem = "O tamanho máximo permitido é 5MB." });

        // Gera nome de arquivo único para evitar sobreposição
        var nomeUnico = $"{Guid.NewGuid()}{extensao}";

        // Caminho relativo a ser salvo no banco (ex: "usuarios/f82a...jpg")
        var caminhoRelativo = Path.Combine(subpasta, nomeUnico).Replace("\\", "/");

        // Define a pasta raiz de uploads (Cria se não existir)
        var pathPasta = Path.Combine(_environment.WebRootPath ?? _environment.ContentRootPath, "uploads", subpasta);
        if (!Directory.Exists(pathPasta))
            Directory.CreateDirectory(pathPasta);

        // Caminho físico final do arquivo
        var caminhoFisico = Path.Combine(pathPasta, nomeUnico);

        using (var stream = new FileStream(caminhoFisico, FileMode.Create))
        {
            await file.CopyToAsync(stream);
        }

        // Retorna o caminho que será utilizado depois pelo front-end para salvar na Entidade
        return Ok(new { Caminho = caminhoRelativo });
    }

}
