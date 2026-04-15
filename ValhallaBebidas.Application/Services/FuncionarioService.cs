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

    public async Task<IEnumerable<FuncionarioDto>> ListarTodosAsync()
    {
        var funcionarios = await _funcionarioRepository.ListarTodosAsync();
        return funcionarios.Select(MapearParaDto);
    }

    public async Task<FuncionarioDto?> ObterPorIdAsync(int id)
    {
        var funcionario = await _funcionarioRepository.ObterPorIdAsync(id);
        return funcionario == null ? null : MapearParaDto(funcionario);
    }

    public async Task<FuncionarioDto> CriarAsync(CriarFuncionarioDto dto)
    {
        var existenteCpf = await _funcionarioRepository.ObterPorCpfAsync(dto.Cpf);
        if (existenteCpf != null)
            throw new InvalidOperationException($"Já existe um funcionário com o CPF '{dto.Cpf}'.");

        var existenteEmail = await _funcionarioRepository.ObterPorEmailAsync(dto.Email);
        if (existenteEmail != null)
            throw new InvalidOperationException($"Já existe um funcionário com o email '{dto.Email}'.");

        var endereco = new Endereco
        {
            Logradouro = dto.Endereco.Logradouro,
            Numero = dto.Endereco.Numero,
            Complemento = dto.Endereco.Complemento,
            Cep = dto.Endereco.Cep,
            Bairro = dto.Endereco.Bairro,
            Cidade = dto.Endereco.Cidade,
            Estado = dto.Endereco.Estado,
        };

        await _enderecoRepository.AdicionarAsync(endereco);
        await _enderecoRepository.SaveAsync();

        var funcionario = new Funcionario
        {
            NomeCompleto = dto.NomeCompleto,
            DataNascimento = dto.DataNascimento,
            Cpf = dto.Cpf,
            Telefone = dto.Telefone,
            Email = dto.Email,
            SenhaHash = BCrypt.Net.BCrypt.HashPassword(dto.Senha),
            Status = true,
            Endereco = endereco,
        };

        await _funcionarioRepository.AdicionarAsync(funcionario);
        await _funcionarioRepository.SaveAsync();

        var salvo = await _funcionarioRepository.ObterPorIdAsync(funcionario.Id);
        return MapearParaDto(salvo!);
    }

    public async Task AtualizarAsync(int id, AtualizarFuncionarioDto dto)
    {
        var funcionario = await _funcionarioRepository.ObterPorIdAsync(id);
        if (funcionario == null)
            throw new KeyNotFoundException($"Funcionário com Id {id} não encontrado.");

        var comMesmoCpf = await _funcionarioRepository.ObterPorCpfAsync(dto.Cpf);
        if (comMesmoCpf != null && comMesmoCpf.Id != id)
            throw new InvalidOperationException($"O CPF '{dto.Cpf}' já está em uso por outro funcionário.");

        var comMesmoEmail = await _funcionarioRepository.ObterPorEmailAsync(dto.Email);
        if (comMesmoEmail != null && comMesmoEmail.Id != id)
            throw new InvalidOperationException($"O e-mail '{dto.Email}' já está em uso por outro funcionário.");

        funcionario.NomeCompleto = dto.NomeCompleto;
        funcionario.DataNascimento = dto.DataNascimento;
        funcionario.Cpf = dto.Cpf;
        funcionario.Telefone = dto.Telefone;
        funcionario.Email = dto.Email;
        funcionario.Status = dto.Status;

        if (!string.IsNullOrWhiteSpace(dto.Senha))
            funcionario.SenhaHash = BCrypt.Net.BCrypt.HashPassword(dto.Senha);

        await _funcionarioRepository.AtualizarAsync(funcionario);
        await _funcionarioRepository.SaveAsync();
    }

    public async Task AlterarStatusAsync(int id, bool status)
    {
        var funcionario = await _funcionarioRepository.ObterPorIdAsync(id);
        if (funcionario == null)
            throw new KeyNotFoundException($"Funcionário com Id {id} não encontrado.");

        funcionario.Status = status;

        await _funcionarioRepository.AtualizarAsync(funcionario);
        await _funcionarioRepository.SaveAsync();
    }

    public async Task RemoverAsync(int id)
    {
        var funcionario = await _funcionarioRepository.ObterPorIdAsync(id);
        if (funcionario == null)
            throw new KeyNotFoundException($"Funcionário com Id {id} não encontrado.");

        await _funcionarioRepository.RemoverAsync(id);
        await _funcionarioRepository.SaveAsync();
    }

    public async Task<LoginFuncionarioResponseDto> LoginAsync(LoginFuncionarioDto dto)
    {
        var funcionario = await _funcionarioRepository.ObterPorEmailAsync(dto.Email);

        if (funcionario == null || !BCrypt.Net.BCrypt.Verify(dto.Senha, funcionario.SenhaHash))
            return new LoginFuncionarioResponseDto
            {
                Sucesso = false,
                Mensagem = "Email ou senha inválidos."
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
            Email = funcionario.Email,
            Sucesso = true,
            Mensagem = "Login realizado com sucesso."
        };
    }

    private static FuncionarioDto MapearParaDto(Funcionario f) => new()
    {
        Id = f.Id,
        NomeCompleto = f.NomeCompleto,
        DataNascimento = f.DataNascimento,
        Cpf = f.Cpf,
        Telefone = f.Telefone,
        Email = f.Email,
        Status = f.Status,
    };
}