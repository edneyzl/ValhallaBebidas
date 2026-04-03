using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ValhallaBebidas.Infrastructure.Data;

namespace ValhallaBebidas.API.Controllers;


// <summary>
/// Controller para gerenciamento de categorias de produtos.
/// Fornece endpoint de consulta para popular ComboBoxes no cliente.
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class CategoriaController : ControllerBase
{
    private readonly ValhallaBebidasDbContext _db;

    public CategoriaController(ValhallaBebidasDbContext db)
    {
        _db = db;
    }

    /// <summary>
    /// Retorna todas as categorias cadastradas.
    /// Usado pela UI para preencher o ComboBox de categorias no cadastro de produto.
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> GetCategorias()
    {
        var categorias = await _db.Categorias
            .OrderBy(c => c.Nome)
            .Select(c => new { c.Id, c.Nome })
            .ToListAsync();

        return Ok(categorias);
    }

}
