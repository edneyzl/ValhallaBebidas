using ValhallaBebidas.Application.DTOs;
using ValhallaBebidas.Domain.Entities;
using ValhallaBebidas.Domain.Interfaces;

namespace ValhallaBebidas.Application.Services;

public class ClienteService
{
    private readonly IClienteRepository _clienteRepository;
    private readonly IEnderecoRepository _enderecoRepository;
    private readonly IUnitOfWork _unitOfWork;

    public ClienteService(
        IClienteRepository clienteRepository,
        IEnderecoRepository enderecoRepository,
        IUnitOfWork unitOfWork)
    {
        _clienteRepository = clienteRepository;
        _enderecoRepository = enderecoRepository;
        _unitOfWork = unitOfWork;
    }

    // ════════════════════════════════════════
    // LISTAR TODOS
    // ════════════════════════════════════════
    public async Task<IEnumerable<ClienteDto>> ListarTodosAsync()
    {
        var clientes = await _clienteRepository.ListarTodosAsync();
        return clientes.Select(MapearParaDto);
    }

    // ════════════════════════════════════════
    // OBTER POR ID
    // ════════════════════════════════════════
    public async Task<ClienteDto?> ObterPorIdAsync(int id)
    {
        var cliente = await _clienteRepository.ObterPorIdAsync(id);
        return cliente == null ? null : MapearParaDto(cliente);
    }

    // ════════════════════════════════════════
    // OBTER POR EMAIL — usado no login
    // ════════════════════════════════════════
    public async Task<ClienteDto?> ObterPorEmailAsync(string email)
    {
        var cliente = await _clienteRepository.ObterPorEmailAsync(email);
        return cliente == null ? null : MapearParaDto(cliente);
    }

    // ════════════════════════════════════════
    // CRIAR
    // ════════════════════════════════════════
    public async Task<ClienteDto> CriarAsync(CriarClienteDto dto)
    {
        /* Documento único */
        var existenteDoc = await _clienteRepository.ObterPorDocumentoAsync(dto.Documento);
        if (existenteDoc != null)
            throw new InvalidOperationException($"Já existe um cliente com o documento '{dto.Documento}'.");

        /* Email único */
        var existenteEmail = await _clienteRepository.ObterPorEmailAsync(dto.Email);
        if (existenteEmail != null)
            throw new InvalidOperationException($"Já existe um cliente com o email '{dto.Email}'.");

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

        /* Cria o cliente */
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
        await _unitOfWork.SaveChangesAsync();

        return MapearParaDto(cliente);
    }

    // ════════════════════════════════════════
    // ATUALIZAR
    // ════════════════════════════════════════
    public async Task AtualizarAsync(int id, AtualizarClienteDto dto)
    {
        var cliente = await _clienteRepository.ObterPorIdAsync(id);
        if (cliente == null)
            throw new KeyNotFoundException($"Cliente com Id {id} não encontrado.");

        /* Verifica documento duplicado em outro cliente */
        var comMesmoDoc = await _clienteRepository.ObterPorDocumentoAsync(dto.Documento);
        if (comMesmoDoc != null && comMesmoDoc.Id != id)
            throw new InvalidOperationException($"O documento '{dto.Documento}' já está em uso por outro cliente.");

        cliente.NomeCliente = dto.Nome;
        cliente.Documento = dto.Documento;
        cliente.Telefone = dto.Telefone;
        cliente.DataNascimento = dto.DataNascimento;

        await _clienteRepository.AtualizarAsync(cliente);
        await _unitOfWork.SaveChangesAsync();
    }

    // ════════════════════════════════════════
    // ATIVAR / INATIVAR — soft delete
    // ════════════════════════════════════════
    public async Task AlterarStatusAsync(int id, bool status)
    {
        var cliente = await _clienteRepository.ObterPorIdAsync(id);
        if (cliente == null)
            throw new KeyNotFoundException($"Cliente com Id {id} não encontrado.");

        cliente.Status = status;
        await _clienteRepository.AtualizarAsync(cliente);
        await _unitOfWork.SaveChangesAsync();
    }

    // ════════════════════════════════════════
    // REMOVER
    // ════════════════════════════════════════
    public async Task RemoverAsync(int id)
    {
        var cliente = await _clienteRepository.ObterPorIdAsync(id);
        if (cliente == null)
            throw new KeyNotFoundException($"Cliente com Id {id} não encontrado.");

        await _clienteRepository.RemoverAsync(id);
        await _unitOfWork.SaveChangesAsync();
    }

    // ════════════════════════════════════════
    // LOGIN — valida credenciais
    // ════════════════════════════════════════
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

    // ════════════════════════════════════════
    // MAPPER — Entidade → DTO
    // ════════════════════════════════════════
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
            TipoLogradouro = c.Endereco.TipoLogradouro,
            Logradouro = c.Endereco.Logradouro,
            Numero = c.Endereco.Numero,
            Complemento = c.Endereco.Complemento,
            Cep = c.Endereco.Cep,
            Bairro = c.Endereco.Bairro,
            Cidade = c.Endereco.Cidade,
            Estado = c.Endereco.Estado,
        },
    };
}