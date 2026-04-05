using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ValhallaBebidas.Application.DTOs;
using ValhallaBebidas.Application.Services;
namespace ValhallaBebidas.API.Controllers;


/// <summary>
/// Controller CRUD completo para Clientes.
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class ClienteController : ControllerBase
{
    private readonly ClienteService _clienteService;

    public ClienteController(ClienteService clienteService)
    {
        _clienteService = clienteService;
    }

    [HttpGet]
    public async Task<IActionResult> ListarTodos()
    {
        var clientes = await _clienteService.ListarTodosAsync();
        return Ok(clientes);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> ObterPorId(int id)
    {
        var cliente = await _clienteService.ObterPorIdAsync(id);
        if (cliente == null) return NotFound(new { mensagem = $"Cliente {id} não encontrado." });
        return Ok(cliente);
    }

    [HttpPost]
    public async Task<IActionResult> Criar([FromBody] CriarClienteDto dto)
    {
        try
        {
            var cliente = await _clienteService.CriarAsync(dto);
            return CreatedAtAction(nameof(ObterPorId), new { id = cliente.Id }, cliente);
        }
        catch (InvalidOperationException ex)
        {
            return Conflict(new { mensagem = ex.Message });
        }
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Atualizar(int id, [FromBody] AtualizarClienteDto dto)
    {
        try
        {
            await _clienteService.AtualizarAsync(id, dto);
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
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Remover(int id)
    {
        try
        {
            await _clienteService.RemoverAsync(id);
            return NoContent();
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { mensagem = ex.Message });
        }
    }

    /// <summary>
    /// Atualiza o endereço do cliente.
    /// </summary>
    [HttpPut("{id}/endereco")]
    public async Task<IActionResult> AtualizarEndereco(int id, [FromBody] AtualizarEnderecoDto dto)
    {
        try
        {
            await _clienteService.AtualizarEnderecoAsync(id, dto);
            return NoContent();
        }
        catch (KeyNotFoundException ex) { return NotFound(new { mensagem = ex.Message }); }
        catch (InvalidOperationException ex) { return Conflict(new { mensagem = ex.Message }); }
    }

    /// <summary>
    /// Altera a senha do cliente.
    /// </summary>
    [HttpPut("{id}/senha")]
    public async Task<IActionResult> AtualizarSenha(int id, [FromBody] AtualizarSenhaDto dto)
    {
        try
        {
            await _clienteService.AtualizarSenhaAsync(id, dto);
            return NoContent();
        }
        catch (KeyNotFoundException ex) { return NotFound(new { mensagem = ex.Message }); }
        catch (InvalidOperationException ex) { return BadRequest(new { mensagem = ex.Message }); }
    }
}
