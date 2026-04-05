using Microsoft.AspNetCore.Mvc;
using ValhallaBebidas.Application.DTOs;
using ValhallaBebidas.Application.Services;
using ValhallaBebidas.Domain.Enums;

namespace ValhallaBebidas.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PedidoController : ControllerBase
{
    private readonly PedidoService _pedidoService;

    public PedidoController(PedidoService pedidoService)
    {
        _pedidoService = pedidoService;
    }

    /// <summary>
    /// Lista todos os pedidos.
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> ListarTodos()
    {
        var pedidos = await _pedidoService.ListarTodosAsync();
        return Ok(pedidos);
    }

    /// <summary>
    /// Lista pedidos de um cliente específico.
    /// </summary>
    [HttpGet("cliente/{clienteId}")]
    public async Task<IActionResult> ListarPorCliente(int clienteId)
    {
        try
        {
            var pedidos = await _pedidoService.ListarPorClienteAsync(clienteId);
            return Ok(pedidos);
        }
        catch (Exception ex)
        {
            return BadRequest(new { mensagem = ex.Message });
        }
    }

    /// <summary>
    /// Obtém um pedido pelo ID.
    /// </summary>
    [HttpGet("{id}")]
    public async Task<IActionResult> ObterPorId(int id)
    {
        var pedido = await _pedidoService.ObterPorIdAsync(id);
        if (pedido == null)
            return NotFound(new { mensagem = $"Pedido {id} não encontrado." });
        return Ok(pedido);
    }

    /// <summary>
    /// Cria um novo pedido.
    /// </summary>
    [HttpPost]
    public async Task<IActionResult> Criar([FromBody] CriarPedidoDto dto)
    {
        try
        {
            var pedido = await _pedidoService.CriarAsync(dto);
            return CreatedAtAction(nameof(ObterPorId), new { id = pedido.Id }, pedido);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { mensagem = ex.Message });
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { mensagem = ex.Message });
        }
        catch (Exception ex)
        {
            return BadRequest(new { mensagem = ex.Message });
        }
    }

    /// <summary>
    /// Atualiza o status de um pedido (Confirmado ou Cancelado).
    /// </summary>
    [HttpPut("{id}/status")]
    public async Task<IActionResult> AtualizarStatus(int id, [FromBody] AtualizarStatusDto dto)
    {
        try
        {
            if (!Enum.TryParse<StatusPedido>(dto.NovoStatus, true, out var status))
                return BadRequest(new { mensagem = $"'{dto.NovoStatus}' não é um StatusPedido válido." });

            await _pedidoService.AtualizarStatusAsync(id, status);
            return NoContent();
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { mensagem = ex.Message });
        }
        catch (InvalidOperationException ex)
        {
            return Conflict(new { mensagem = ex.Message });
        }
        catch (Exception ex)
        {
            return BadRequest(new { mensagem = ex.Message });
        }
    }

    /// <summary>
    /// Remove um pedido.
    /// </summary>
    [HttpDelete("{id}")]
    public async Task<IActionResult> Remover(int id)
    {
        try
        {
            await _pedidoService.RemoverAsync(id);
            return NoContent();
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { mensagem = ex.Message });
        }
    }
}
