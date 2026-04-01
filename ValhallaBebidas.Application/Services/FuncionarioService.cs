using ValhallaBebidas.Application.DTOs;
using ValhallaBebidas.Domain.Entities;
using ValhallaBebidas.Domain.Interfaces;

namespace ValhallaBebidas.Application.Services;

public class FuncionarioService
{
    private readonly IFuncionarioRepository _funcionarioRepository;
    private readonly IEnderecoRepository _enderecoRepository;

    public FuncionarioService(
        IFuncionarioRepository funcionarioRepository,
        IEnderecoRepository enderecoRepository)
    {
        _funcionarioRepository = funcionarioRepository;
        _enderecoRepository = enderecoRepository;
    }

    // ════════════════════════════════════════
    // LISTAR TODOS
    // ════════════════════════════════════════
    public async Task<IEnumerable<FuncionarioDto>> ListarTodosAsync()
    {
        var funcionarios = await _funcionarioRepository.ListarTodosAsync();
        return funcionarios.Select(MapearParaDto);
    }

    // ════════════════════════════════════════
    // OBTER POR ID
    // ════════════════════════════════════════
    public async Task<FuncionarioDto?> ObterPorIdAsync(int id)
    {
        var funcionario = await _funcionarioRepository.ObterPorIdAsync(id);
        return funcionario == null ? null : MapearParaDto(funcionario);
    }

    // ════════════════════════════════════════
    // CRIAR
    // ════════════════════════════════════════
    public async Task<FuncionarioDto> CriarAsync(CriarFuncionarioDto dto)
    {
        /* CPF único */
        var existenteCpf = await _funcionarioRepository.ObterPorCpfAsync(dto.Cpf);
        if (existenteCpf != null)
            throw new InvalidOperationException($"Já existe um funcionário com o CPF '{dto.Cpf}'.");

        /* Email único */
        var existenteEmail = await _funcionarioRepository.ObterPorEmailAsync(dto.Email);
        if (existenteEmail != null)
            throw new InvalidOperationException($"Já existe um funcionário com o email '{dto.Email}'.");

        /* Login único */
        var existenteLogin = await _funcionarioRepository.ObterPorLoginAsync(dto.Login);
        if (existenteLogin != null)
            throw new InvalidOperationException($"O login '{dto.Login}' já está em uso.");

        /* Cria o endereço */
        var endereco = new Endereco
        {
            TipoLogradouro = dto.Endereco.TipoLogradouro,
            Logradouro = dto.Endereco.Logradouro,
            Numero = dto.Endereco.Numero,
            Complemento = dto.Endereco.Complemento,
            Cep = dto.Endereco.Cep,
            Bairro = dto.Endereco.Bairro,
            Cidade = dto.Endereco.Cidade,
            Estado = dto.Endereco.Estado,
        };

        await _enderecoRepository.AdicionarAsync(endereco);

        /* Cria o funcionário */
        var funcionario = new Funcionario
        {
            NomeCompleto = dto.NomeCompleto,
            DataNascimento = dto.DataNascimento,
            Cpf = dto.Cpf,
            Telefone = dto.Telefone,
            Email = dto.Email,
            Login = dto.Login,
            SenhaHash = BCrypt.Net.BCrypt.HashPassword(dto.Senha),
            Status = true,
            EnderecoId = endereco.Id,
        };

        await _funcionarioRepository.AdicionarAsync(funcionario);

        return MapearParaDto(funcionario);
    }

    // ════════════════════════════════════════
    // ATUALIZAR
    // ════════════════════════════════════════
    public async Task AtualizarAsync(int id, AtualizarFuncionarioDto dto)
    {
        var funcionario = await _funcionarioRepository.ObterPorIdAsync(id);
        if (funcionario == null)
            throw new KeyNotFoundException($"Funcionário com Id {id} não encontrado.");

        /* Verifica duplicatas em outros registros */
        var comMesmoCpf = await _funcionarioRepository.ObterPorCpfAsync(dto.Cpf);
        if (comMesmoCpf != null && comMesmoCpf.Id != id)
            throw new InvalidOperationException($"O CPF '{dto.Cpf}' já está em uso por outro funcionário.");

        var comMesmoLogin = await _funcionarioRepository.ObterPorLoginAsync(dto.Login);
        if (comMesmoLogin != null && comMesmoLogin.Id != id)
            throw new InvalidOperationException($"O login '{dto.Login}' já está em uso por outro funcionário.");

        funcionario.NomeCompleto = dto.NomeCompleto;
        funcionario.DataNascimento = dto.DataNascimento;
        funcionario.Cpf = dto.Cpf;
        funcionario.Telefone = dto.Telefone;
        funcionario.Email = dto.Email;
        funcionario.Login = dto.Login;
        funcionario.Status = dto.Status;

        /* Atualiza senha apenas se informada */
        if (!string.IsNullOrWhiteSpace(dto.Senha))
            funcionario.SenhaHash = BCrypt.Net.BCrypt.HashPassword(dto.Senha);

        await _funcionarioRepository.AtualizarAsync(funcionario);
    }

    // ════════════════════════════════════════
    // ATIVAR / INATIVAR — soft delete
    // ════════════════════════════════════════
    public async Task AlterarStatusAsync(int id, bool status)
    {
        var funcionario = await _funcionarioRepository.ObterPorIdAsync(id);
        if (funcionario == null)
            throw new KeyNotFoundException($"Funcionário com Id {id} não encontrado.");

        funcionario.Status = status;
        await _funcionarioRepository.AtualizarAsync(funcionario);
    }

    // ════════════════════════════════════════
    // REMOVER
    // ════════════════════════════════════════
    public async Task RemoverAsync(int id)
    {
        var funcionario = await _funcionarioRepository.ObterPorIdAsync(id);
        if (funcionario == null)
            throw new KeyNotFoundException($"Funcionário com Id {id} não encontrado.");

        await _funcionarioRepository.RemoverAsync(id);
    }

    // ════════════════════════════════════════
    // LOGIN — valida credenciais (Windows Forms)
    // ════════════════════════════════════════
    public async Task<LoginFuncionarioResponseDto> LoginAsync(LoginFuncionarioDto dto)
    {
        var funcionario = await _funcionarioRepository.ObterPorLoginAsync(dto.Login);

        if (funcionario == null || !BCrypt.Net.BCrypt.Verify(dto.Senha, funcionario.SenhaHash))
            return new LoginFuncionarioResponseDto
            {
                Sucesso = false,
                Mensagem = "Login ou senha inválidos."
            };

        if (!funcionario.Status)
            return new LoginFuncionarioResponseDto
            {
                Sucesso = false,
                Mensagem = "Conta inativa. Entre em contato com o administrador."
            };

        return new LoginFuncionarioResponseDto
        {
            Id = funcionario.Id,
            Nome = funcionario.NomeCompleto,
            Login = funcionario.Login,
            Email = funcionario.Email,
            Sucesso = true,
            Mensagem = "Login realizado com sucesso."
        };
    }

    // ════════════════════════════════════════
    // MAPPER — Entidade → DTO
    // ════════════════════════════════════════
    private static FuncionarioDto MapearParaDto(Funcionario f) => new()
    {
        Id = f.Id,
        NomeCompleto = f.NomeCompleto,
        DataNascimento = f.DataNascimento,
        Cpf = f.Cpf,
        Telefone = f.Telefone,
        Email = f.Email,
        Login = f.Login,
        Status = f.Status,
    };
}