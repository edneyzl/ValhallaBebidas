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

    [HttpGet]
    public async Task<IActionResult> ListarTodos()
    {
        var pedidos = await _pedidoService.ListarTodosAsync();
        return Ok(pedidos);
    }

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

    [HttpGet("{id}")]
    public async Task<IActionResult> ObterPorId(int id)
    {
        var pedido = await _pedidoService.ObterPorIdAsync(id);
        if (pedido == null)
            return NotFound(new { mensagem = $"Pedido {id} não encontrado." });

        return Ok(pedido);
    }

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

    [HttpPut("{id}/status")]
    public async Task<IActionResult> AtualizarStatus(int id, [FromBody] AtualizarStatusDto dto)
    {
        try
        {
            await _pedidoService.AtualizarStatusAsync(id, dto.NovoStatus);
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

    [HttpPost("{id}/cancelar")]
    public async Task<IActionResult> Cancelar(int id, [FromQuery] int clienteId)
    {
        if (clienteId <= 0)
            return Unauthorized(new { mensagem = "Cliente não autenticado." });

        try
        {
            await _pedidoService.CancelarAsync(id, clienteId);
            return Ok(new { mensagem = "Pedido cancelado com sucesso." });
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