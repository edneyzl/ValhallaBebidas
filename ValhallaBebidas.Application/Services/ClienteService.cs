using ValhallaBebidas.Application.DTOs;
using ValhallaBebidas.Domain.Entities;
using ValhallaBebidas.Domain.Interfaces;

namespace ValhallaBebidas.Application.Services;

public class ClienteService
{

    private readonly IClienteRepository _clienteRepository;

    public ClienteService(IClienteRepository clienteRepository)
    {
        _clienteRepository = clienteRepository;
    }


  public async Task<IEnumerable<ClienteDto>> ListarTodosAsync()
    {
        var clientes = await _clienteRepository.ListarTodosAsync();
        return clientes.Select(c => new ClienteDto
        {
            Id = c.Id,
            Nome = c.NomeCliente,
            Documento = c.Documento//documento pode ser cpf ou cnpj, dependendo do tipo do cliente
        });

    }

    public async Task<ClienteDto?> ObterPorIdAsync(int id)
    {
        var cliente = await _clienteRepository.ObterPorIdAsync(id);
        if (cliente == null) return null;

        return new ClienteDto { Id = cliente.Id, Nome = cliente.NomeCliente, Documento = cliente.Documento };
    }


    public async Task<ClienteDto> CriarAsync(CriarClienteDto dto)
    {
        // Regra de negócio: CPF único
        var existente = await _clienteRepository.ObterPorDocumentoAsync(dto.Documento);
        if (existente != null)
            throw new InvalidOperationException($"Já existe um cliente cadastrado com o Documento informado '{dto.Documento}'.");

        var cliente = new Cliente
        {
            NomeCliente = dto.Nome,
            Documento = dto.Documento
        };

        await _clienteRepository.AdicionarAsync(cliente);

        return new ClienteDto { Id = cliente.Id, Nome = cliente.NomeCliente, Documento = cliente.Documento };
    }

    public async Task AtualizarAsync(int id, AtualizarClienteDto dto)
    {
        var cliente = await _clienteRepository.ObterPorIdAsync(id);
        if (cliente == null)
            throw new KeyNotFoundException($"Cliente com Id {id} não encontrado.");

        // Verifica se outro cliente já usa este CPF
        var comMesmoCPF = await _clienteRepository.ObterPorDocumentoAsync(dto.Documento);
        if (comMesmoCPF != null && comMesmoCPF.Id != id)
            throw new InvalidOperationException($"O CPF '{dto.Documento}' já está em uso por outro cliente.");

        cliente.NomeCliente = dto.Nome;
        cliente.Documento = dto.Documento;

        await _clienteRepository.AtualizarAsync(cliente);
    }

    public async Task RemoverAsync(int id)
    {
        var cliente = await _clienteRepository.ObterPorIdAsync(id);
        if (cliente == null)
            throw new KeyNotFoundException($"Cliente com Id {id} não encontrado.");

        await _clienteRepository.RemoverAsync(id);
    }


}
