using ValhallaBebidas.Application.DTOs;
using ValhallaBebidas.Domain.Entities;
using ValhallaBebidas.Domain.Interfaces;

namespace ValhallaBebidas.Application.Services;

public class ClienteService
{
    private readonly IClienteRepository _clienteRepository;
    private readonly IEnderecoRepository _enderecoRepository;

    public ClienteService(
        IClienteRepository clienteRepository,
        IEnderecoRepository enderecoRepository)
    {
        _clienteRepository = clienteRepository;
        _enderecoRepository = enderecoRepository;
    }

    public async Task<IEnumerable<ClienteDto>> ListarTodosAsync()
    {
        var clientes = await _clienteRepository.ListarTodosAsync();
        return clientes.Select(MapearParaDto);
    }

    public async Task<ClienteDto?> ObterPorIdAsync(int id)
    {
        var cliente = await _clienteRepository.ObterPorIdAsync(id);
        return cliente == null ? null : MapearParaDto(cliente);
    }

    public async Task<ClienteDto?> ObterPorEmailAsync(string email)
    {
        var cliente = await _clienteRepository.ObterPorEmailAsync(email);
        return cliente == null ? null : MapearParaDto(cliente);
    }

    public async Task<ClienteDto> CriarAsync(CriarClienteDto dto)
    {
        var existenteDoc = await _clienteRepository.ObterPorDocumentoAsync(dto.Documento);
        if (existenteDoc != null)
            throw new InvalidOperationException($"Já existe um cliente com o documento '{dto.Documento}'.");

        var existenteEmail = await _clienteRepository.ObterPorEmailAsync(dto.Email);
        if (existenteEmail != null)
            throw new InvalidOperationException($"Já existe um cliente com o email '{dto.Email}'.");

        var endereco = new Endereco
        {
            Logradouro = dto.Endereco.Logradouro,
            Numero = dto.Endereco.Numero,
            Complemento = dto.Endereco.Complemento ?? "",
            Cep = dto.Endereco.Cep,
            Bairro = dto.Endereco.Bairro,
            Cidade = dto.Endereco.Cidade,
            Estado = dto.Endereco.Estado,
        };

        await _enderecoRepository.AdicionarAsync(endereco);
        await _enderecoRepository.SaveAsync();

        var cliente = new Cliente
        {
            NomeCliente = dto.Nome,
            DataNascimento = dto.DataNascimento,
            Documento = dto.Documento,
            Telefone = dto.Telefone,
            Email = dto.Email,
            SenhaHash = BCrypt.Net.BCrypt.HashPassword(dto.Senha),
            Status = true,
            Endereco = endereco,
        };

        await _clienteRepository.AdicionarAsync(cliente);
        await _clienteRepository.SaveAsync();

        var salvo = await _clienteRepository.ObterPorIdAsync(cliente.Id);
        return MapearParaDto(salvo!);
    }

    public async Task AtualizarAsync(int id, AtualizarClienteDto dto)
    {
        var cliente = await _clienteRepository.ObterPorIdAsync(id);
        if (cliente == null)
            throw new KeyNotFoundException($"Cliente com Id {id} não encontrado.");

        var comMesmoDoc = await _clienteRepository.ObterPorDocumentoAsync(dto.Documento);
        if (comMesmoDoc != null && comMesmoDoc.Id != id)
            throw new InvalidOperationException($"O documento '{dto.Documento}' já está em uso por outro cliente.");

        cliente.NomeCliente = dto.Nome;
        cliente.Documento = dto.Documento;
        cliente.Telefone = dto.Telefone;
        cliente.DataNascimento = dto.DataNascimento;

        await _clienteRepository.AtualizarAsync(cliente);
        await _clienteRepository.SaveAsync();
    }

    public async Task AlterarStatusAsync(int id, bool status)
    {
        var cliente = await _clienteRepository.ObterPorIdAsync(id);
        if (cliente == null)
            throw new KeyNotFoundException($"Cliente com Id {id} não encontrado.");

        cliente.Status = status;
        await _clienteRepository.AtualizarAsync(cliente);
        await _clienteRepository.SaveAsync();
    }

    public async Task RemoverAsync(int id)
    {
        var cliente = await _clienteRepository.ObterPorIdAsync(id);
        if (cliente == null)
            throw new KeyNotFoundException($"Cliente com Id {id} não encontrado.");

        await _clienteRepository.RemoverAsync(id);
        await _clienteRepository.SaveAsync();
    }

    public async Task<LoginClienteResponseDto> LoginAsync(LoginClienteDto dto)
    {
        var cliente = await _clienteRepository.ObterPorEmailAsync(dto.Email);

        if (cliente == null || !BCrypt.Net.BCrypt.Verify(dto.Senha, cliente.SenhaHash))
            return new LoginClienteResponseDto
            {
                Sucesso = false,
                Mensagem = "Email ou senha inválidos."
            };

        if (!cliente.Status)
            return new LoginClienteResponseDto
            {
                Sucesso = false,
                Mensagem = "Conta inativa. Entre em contato com o suporte."
            };

        return new LoginClienteResponseDto
        {
            Id = cliente.Id,
            Nome = cliente.NomeCliente,
            Email = cliente.Email,
            Sucesso = true,
            Mensagem = "Login realizado com sucesso."
        };
    }

    public async Task AtualizarEnderecoAsync(int clienteId, AtualizarEnderecoDto dto)
    {
        var cliente = await _clienteRepository.ObterPorIdAsync(clienteId);
        if (cliente == null)
            throw new KeyNotFoundException($"Cliente com Id {clienteId} não encontrado.");

        if (cliente.EnderecoId == null)
            throw new InvalidOperationException("Cliente não possui endereço cadastrado.");

        var enderecoExistente = await _enderecoRepository.ObterPorIdAsync(cliente.EnderecoId.Value);
        if (enderecoExistente == null)
            throw new KeyNotFoundException("Endereço não encontrado.");

        enderecoExistente.Logradouro = dto.Logradouro;
        enderecoExistente.Numero = dto.Numero;
        enderecoExistente.Complemento = dto.Complemento;
        enderecoExistente.Cep = dto.Cep;
        enderecoExistente.Bairro = dto.Bairro;
        enderecoExistente.Cidade = dto.Cidade;
        enderecoExistente.Estado = dto.Estado;

        await _enderecoRepository.AtualizarAsync(enderecoExistente);
        await _enderecoRepository.SaveAsync();
    }

    public async Task AtualizarSenhaAsync(int clienteId, AtualizarSenhaDto dto)
    {
        var cliente = await _clienteRepository.ObterPorIdAsync(clienteId);
        if (cliente == null)
            throw new KeyNotFoundException($"Cliente com Id {clienteId} não encontrado.");

        if (!BCrypt.Net.BCrypt.Verify(dto.SenhaAtual, cliente.SenhaHash))
            throw new InvalidOperationException("Senha atual inválida.");

        cliente.SenhaHash = BCrypt.Net.BCrypt.HashPassword(dto.NovaSenha);

        await _clienteRepository.AtualizarAsync(cliente);
        await _clienteRepository.SaveAsync();
    }

    private static ClienteDto MapearParaDto(Cliente c) => new()
    {
        Id = c.Id,
        Nome = c.NomeCliente,
        Documento = c.Documento,
        Email = c.Email,
        Telefone = c.Telefone,
        DataNascimento = c.DataNascimento,
        Status = c.Status,
        Endereco = c.Endereco == null ? null : new EnderecoDto
        {
            Id = c.Endereco.Id,
            Logradouro = c.Endereco.Logradouro,
            Numero = c.Endereco.Numero,
            Complemento = c.Endereco.Complemento ?? "",
            Cep = c.Endereco.Cep,
            Bairro = c.Endereco.Bairro,
            Cidade = c.Endereco.Cidade,
            Estado = c.Endereco.Estado,
        },
    };
}