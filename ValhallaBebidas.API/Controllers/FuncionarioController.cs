using Microsoft.AspNetCore.Mvc;
using ValhallaBebidas.Application.DTOs;
using ValhallaBebidas.Application.Services;

namespace ValhallaBebidas.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class FuncionarioController : ControllerBase
{
    private readonly FuncionarioService _funcionarioService;

    public FuncionarioController(FuncionarioService funcionarioService)
    {
        _funcionarioService = funcionarioService;
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginFuncionarioDto loginDto)
    {
        try
        {
            var resultado = await _funcionarioService.LoginAsync(loginDto); // ← corrigido
            if (!resultado.Sucesso)
                return Unauthorized(resultado);
            return Ok(resultado);
        }
        catch (Exception ex)
        {
            return BadRequest(new { mensagem = ex.Message });
        }
    }

    [HttpGet]
    public async Task<IActionResult> ListarTodos()
    {
        var funcionarios = await _funcionarioService.ListarTodosAsync();
        return Ok(funcionarios);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> ObterPorId(int id)
    {
        var funcionario = await _funcionarioService.ObterPorIdAsync(id);
        if (funcionario == null)
            return NotFound(new { mensagem = $"Funcionário {id} não encontrado." });
        return Ok(funcionario);
    }

    [HttpPost]
    public async Task<IActionResult> Criar([FromBody] CriarFuncionarioDto dto)
    {
        try
        {
            var funcionario = await _funcionarioService.CriarAsync(dto);
            return CreatedAtAction(nameof(ObterPorId), new { id = funcionario.Id }, funcionario); // ← corrigido
        }
        catch (InvalidOperationException ex)
        {
            return Conflict(new { mensagem = ex.Message });
        }
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Atualizar(int id, [FromBody] AtualizarFuncionarioDto dto)
    {
        try
        {
            await _funcionarioService.AtualizarAsync(id, dto);
            return NoContent();
        }
        catch (KeyNotFoundException ex) { return NotFound(new { mensagem = ex.Message }); }
        catch (InvalidOperationException ex) { return Conflict(new { mensagem = ex.Message }); }
        catch (Exception ex) { return BadRequest(new { mensagem = ex.Message }); }
    }

    [HttpPatch("{id}/status")]
    public async Task<IActionResult> AlterarStatus(int id, [FromBody] AlterarStatusDto dto)
    {
        try
        {
            await _funcionarioService.AlterarStatusAsync(id, dto.Status);
            return NoContent();
        }
        catch (KeyNotFoundException ex) { return NotFound(new { mensagem = ex.Message }); }
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Remover(int id)
    {
        try
        {
            await _funcionarioService.RemoverAsync(id);
            return NoContent();
        }
        catch (KeyNotFoundException ex) { return NotFound(new { mensagem = ex.Message }); }
    }
}