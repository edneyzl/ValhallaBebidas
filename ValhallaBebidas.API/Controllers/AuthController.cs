using Microsoft.AspNetCore.Mvc;
using ValhallaBebidas.Application.DTOs;
using ValhallaBebidas.Application.Services;

namespace ValhallaBebidas.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly ClienteService _clienteService;

    public AuthController(ClienteService clienteService)
    {
        _clienteService = clienteService;
    }

    /// <summary>
    /// Login do cliente — valida email e senha.
    /// </summary>
    [HttpPost("login-cliente")]
    public async Task<IActionResult> LoginCliente([FromBody] LoginClienteDto dto)
    {
        if (string.IsNullOrWhiteSpace(dto.Email) || string.IsNullOrWhiteSpace(dto.Senha))
            return BadRequest(new { mensagem = "Email e senha são obrigatórios." });

        try
        {
            var resultado = await _clienteService.LoginAsync(dto);
            if (!resultado.Sucesso)
                return Unauthorized(resultado);
            return Ok(resultado);
        }
        catch (Exception ex)
        {
            return BadRequest(new { mensagem = ex.Message });
        }
    }

    /// <summary>
    /// Cadastro de novo cliente.
    /// </summary>
    [HttpPost("cadastro")]
    public async Task<IActionResult> CadastroCliente([FromBody] CriarClienteDto dto)
    {
        if (string.IsNullOrWhiteSpace(dto.Email) || string.IsNullOrWhiteSpace(dto.Senha))
            return BadRequest(new { mensagem = "Email e senha são obrigatórios." });

        try
        {
            var cliente = await _clienteService.CriarAsync(dto);

            /* Retorna no formato LoginClienteResponseDto para compatibilidade com o Web */
            var response = new LoginClienteResponseDto
            {
                Id = cliente.Id,
                Nome = cliente.Nome,
                Email = dto.Email,
                Sucesso = true,
                Mensagem = "Conta criada com sucesso."
            };

            return Ok(response);
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
}
