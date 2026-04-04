using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ValhallaBebidas.Application.DTOs;
using ValhallaBebidas.Application.Services;
namespace ValhallaBebidas.API.Controllers;

[ApiController]
[Route("api/[controller]")]//rota para acessar o controller, api/funcionário
public class FuncionarioController : ControllerBase
{

    private readonly FuncionarioService _funcionarioService;//será injetado no programa.cs


    public FuncionarioController(FuncionarioService usuarioservice)//metodo construtor
    {
        _funcionarioService = usuarioservice;
    }

    /// <summary>
    /// Realiza o login do usuário.
    /// valida email e senha (hash da senha) e retorna os dados do usuário autenticado.
    /// </summary>

    //login do usuário, estou enviando uma requisção um post 
    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginFuncionarioDto loginDto)
    {
        try
        {
            var resultado = await _funcionarioService.AutenticarAsync(loginDto);
            if (!resultado.Sucesso) //se não retornou sucesso 
                return Unauthorized(resultado.Mensagem);
            return Ok(resultado);

        }
        catch (Exception ex)
        {

            return BadRequest(new { mensagem = ex.Message });//parte do controller base 
        }
    }


    //listar 
    [HttpGet]
    public async Task<IActionResult> ListarTodos()
    {
        var usuarios = await _funcionarioService.ListarTodosAsync();
        return Ok(usuarios);
    }

    [HttpGet("{id}")]//rota para listar por id, interpolação de string sem sifrão
    public async Task<IActionResult> ObterPorId(int id)
    {
        var usuario = await _funcionarioService.ObterPorIdAsync(id);
        if (usuario == null)
            return NotFound(new { mensagem = $"Usuário {id}  não foi encontrado" });
        return Ok(usuario);
    }

    [HttpPost]
    public async Task<IActionResult> Criar([FromBody] CriarFuncionarioDto dto)
    {
        try
        {

            var usuario = await _funcionarioService.CriarAsync(dto);
            return CreatedAtAction(nameof(ListarTodos), new { id = usuario.Id }, usuario);
        }
        catch (Exception ex)
        {

            return Conflict(new { mensagem = ex.Message });//caso já tenha usuário 
        }

    }


    [HttpDelete("{id}")]
    public async Task<IActionResult> Remover(int id)
    {
        try
        {
            await _funcionarioService.RemoverAsync(id);
            return NoContent();
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { mensagem = ex.Message });
        }
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] AtualizarFuncionarioDto dto)
    {
        try
        {
            // AtualizarAsync lança KeyNotFoundException se o funcionário não existir
            // e InvalidOperationException em casos de validação (CPF/login duplicados)
            await _funcionarioService.AtualizarAsync(id, dto);
            return NoContent(); // padrão REST para atualização bem-sucedida sem conteúdo
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







}
