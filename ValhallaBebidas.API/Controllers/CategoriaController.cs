using Microsoft.AspNetCore.Mvc;
using ValhallaBebidas.Domain.Interfaces;

namespace ValhallaBebidas.API.Controllers;

/// <summary>
/// Controller para gerenciamento de categorias de produtos.
/// Fornece endpoint de consulta para popular ComboBoxes no cliente.
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class CategoriaController : ControllerBase
{
    private readonly ICategoriaRepository _categoriaRepository;

    public CategoriaController(ICategoriaRepository categoriaRepository)
    {
        _categoriaRepository = categoriaRepository;
    }

    /// <summary>
    /// Retorna todas as categorias cadastradas.
    /// Usado pela UI para preencher o ComboBox de categorias no cadastro de produto.
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> GetCategorias()
    {
        var categorias = await _categoriaRepository.ListarTodosAsync();
        return Ok(categorias.Select(c => new { c.Id, c.Nome }).OrderBy(c => c.Nome));
    }
}
