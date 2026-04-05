using Microsoft.AspNetCore.Mvc;
using ValhallaBebidas.Application.DTOs;
using ValhallaBebidas.Application.Services;

namespace ValhallaBebidas.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class MovimentacaoController : ControllerBase
{
    private readonly MovimentacaoService _movimentacaoService;

    public MovimentacaoController(MovimentacaoService movimentacaoService)
    {
        _movimentacaoService = movimentacaoService;
    }

    // ════════════════════════════════════════
    // GET /api/movimentacao
    // ════════════════════════════════════════
    [HttpGet]
    public async Task<IActionResult> ListarTodos()
        => Ok(await _movimentacaoService.ListarTodosAsync());

    // ════════════════════════════════════════
    // GET /api/movimentacao/produto/{produtoId}
    // ════════════════════════════════════════
    [HttpGet("produto/{produtoId}")]
    public async Task<IActionResult> ListarPorProduto(int produtoId)
        => Ok(await _movimentacaoService.ListarPorProdutoAsync(produtoId));

    // ════════════════════════════════════════
    // GET /api/movimentacao/{id}
    // ════════════════════════════════════════
    [HttpGet("{id}")]
    public async Task<IActionResult> Obter(int id)
    {
        var dto = await _movimentacaoService.ObterPorIdAsync(id);
        return dto == null ? NotFound() : Ok(dto);
    }

    // ════════════════════════════════════════
    // POST /api/movimentacao/entrada
    // ════════════════════════════════════════
    [HttpPost("entrada")]
    public async Task<IActionResult> RegistrarEntrada([FromBody] CriarMovimentacaoDto dto)
    {
        try
        {
            var resultado = await _movimentacaoService.RegistrarEntradaAsync(dto);
            return Ok(resultado);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { mensagem = ex.Message });
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { mensagem = ex.Message });
        }
    }

    // ════════════════════════════════════════
    // POST /api/movimentacao/saida
    // ════════════════════════════════════════
    [HttpPost("saida")]
    public async Task<IActionResult> RegistrarSaida([FromBody] CriarMovimentacaoDto dto)
    {
        try
        {
            var resultado = await _movimentacaoService.RegistrarSaidaAsync(dto);
            return Ok(resultado);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { mensagem = ex.Message });
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { mensagem = ex.Message });
        }
    }

    // ════════════════════════════════════════
    // POST /api/movimentacao/{id}/estornar
    // ════════════════════════════════════════
    [HttpPost("{id}/estornar")]
    public async Task<IActionResult> Estornar(int id)
    {
        try
        {
            await _movimentacaoService.EstornarAsync(id);
            return Ok(new { mensagem = "Movimentacao estornada com sucesso." });
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { mensagem = ex.Message });
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { mensagem = ex.Message });
        }
    }

    // ════════════════════════════════════════
    // DELETE /api/movimentacao/{id}
    // ════════════════════════════
    [HttpDelete("{id}")]
    public async Task<IActionResult> Remover(int id)
    {
        await _movimentacaoService.RemoverAsync(id);
        return NoContent();
    }
}
