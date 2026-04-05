using Microsoft.AspNetCore.Mvc;
using ValhallaBebidas.Application.DTOs;
using ValhallaBebidas.Application.Services;

namespace ValhallaBebidas.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProdutoController : ControllerBase
{
    private readonly ProdutoService _produtoService;

    public ProdutoController(ProdutoService produtoService)
    {
        _produtoService = produtoService;
    }

    /* ── Leitura — público (frontend) ── */

    [HttpGet]
    public async Task<IActionResult> ListarTodos()
    {
        var produtos = await _produtoService.ListarAtivosAsync();
        return Ok(produtos);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> ObterPorId(int id)
    {
        var produto = await _produtoService.ObterPorIdAsync(id);
        if (produto == null)
            return NotFound(new { mensagem = $"Produto {id} não encontrado." });
        return Ok(produto);
    }

    [HttpGet("categoria/{categoriaId}")]
    public async Task<IActionResult> ListarPorCategoria(int categoriaId)
    {
        var produtos = await _produtoService.ListarPorCategoriaAsync(categoriaId);
        return Ok(produtos);
    }

    [HttpGet("estoque-baixo")]
    public async Task<IActionResult> ListarEstoqueBaixo()
    {
        var produtos = await _produtoService.ListarEstoqueBaixoAsync();
        return Ok(produtos);
    }

    /* ── Escrita — Windows Forms ── */

    [HttpPost]
    public async Task<IActionResult> Criar([FromBody] CriarProdutoDto dto)
    {
        try
        {
            var produto = await _produtoService.CriarAsync(dto);
            return CreatedAtAction(nameof(ObterPorId), new { id = produto.Id }, produto);
        }
        catch (InvalidOperationException ex) { return Conflict(new { mensagem = ex.Message }); }
        catch (KeyNotFoundException ex) { return NotFound(new { mensagem = ex.Message }); }
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Atualizar(int id, [FromBody] AtualizarProdutoDto dto)
    {
        try
        {
            await _produtoService.AtualizarAsync(id, dto);
            return NoContent();
        }
        catch (KeyNotFoundException ex) { return NotFound(new { mensagem = ex.Message }); }
        catch (InvalidOperationException ex) { return Conflict(new { mensagem = ex.Message }); }
    }

    [HttpPatch("{id}/status")]
    public async Task<IActionResult> AlterarStatus(int id, [FromBody] AlterarStatusDto dto)
    {
        try
        {
            await _produtoService.AlterarStatusAsync(id, dto.Status);
            return NoContent();
        }
        catch (KeyNotFoundException ex) { return NotFound(new { mensagem = ex.Message }); }
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Remover(int id)
    {
        try
        {
            await _produtoService.RemoverAsync(id);
            return NoContent();
        }
        catch (KeyNotFoundException ex) { return NotFound(new { mensagem = ex.Message }); }
    }
}