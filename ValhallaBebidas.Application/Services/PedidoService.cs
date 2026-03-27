using ValhallaBebidas.Application.DTOs;
using ValhallaBebidas.Domain.Entities;
using ValhallaBebidas.Domain.Enums;
using ValhallaBebidas.Domain.Interfaces;

namespace ValhallaBebidas.Application.Services;

public class PedidoService
{
    private readonly IPedidoRepository _pedidoRepository;
    private readonly IClienteRepository _clienteRepository;
    private readonly IProdutoRepository _produtoRepository;

    public PedidoService(
        IPedidoRepository pedidoRepository,
        IClienteRepository clienteRepository,
        IProdutoRepository produtoRepository)
    {
        _pedidoRepository = pedidoRepository;
        _clienteRepository = clienteRepository;
        _produtoRepository = produtoRepository;
    }

    // ════════════════════════════════════════
    // LISTAR TODOS
    // ════════════════════════════════════════
    public async Task<IEnumerable<PedidoDto>> ListarTodosAsync()
    {
        var pedidos = await _pedidoRepository.ListarTodosAsync();
        return pedidos.Select(MapearParaDto);
    }

    // ════════════════════════════════════════
    // LISTAR POR CLIENTE
    // ════════════════════════════════════════
    public async Task<IEnumerable<PedidoDto>> ListarPorClienteAsync(int clienteId)
    {
        var pedidos = await _pedidoRepository.ListarPorClienteAsync(clienteId);
        return pedidos.Select(MapearParaDto);
    }

    // ════════════════════════════════════════
    // OBTER POR ID
    // ════════════════════════════════════════
    public async Task<PedidoDto?> ObterPorIdAsync(int id)
    {
        var pedido = await _pedidoRepository.ObterPorIdAsync(id);
        return pedido == null ? null : MapearParaDto(pedido);
    }

    // ════════════════════════════════════════
    // CRIAR
    // ════════════════════════════════════════
    public async Task<PedidoDto> CriarAsync(CriarPedidoDto dto)
    {
        /* Pelo menos 1 item */
        if (dto.Itens == null || dto.Itens.Count == 0)
            throw new InvalidOperationException("Um pedido deve conter pelo menos 1 item.");

        /* Cliente existe */
        var cliente = await _clienteRepository.ObterPorIdAsync(dto.ClienteId);
        if (cliente == null)
            throw new KeyNotFoundException($"Cliente com Id {dto.ClienteId} não encontrado.");

        /* Monta os itens */
        var itens = new List<ItemPedido>();
        foreach (var itemDto in dto.Itens)
        {
            var produto = await _produtoRepository.ObterPorIdAsync(itemDto.ProdutoId);
            if (produto == null)
                throw new KeyNotFoundException($"Produto com Id {itemDto.ProdutoId} não encontrado.");

            /* Estoque suficiente */
            if (produto.QuantidadeEstoque < itemDto.Quantidade)
                throw new InvalidOperationException(
                    $"Estoque insuficiente para '{produto.Nome}'. Disponível: {produto.QuantidadeEstoque}.");

            itens.Add(new ItemPedido
            {
                ProdutoId = produto.Id,
                Quantidade = itemDto.Quantidade,
                PrecoVenda = produto.PrecoVenda, /* captura o preço atual */
            });
        }

        var pedido = new Pedido
        {
            ClienteId = dto.ClienteId,
            DataPedido = DateTime.Now,
            Status = StatusPedido.Pendente,
            Itens = itens,
        };

        pedido.RecalcularTotal();

        await _pedidoRepository.AdicionarAsync(pedido);

        var pedidoCriado = await _pedidoRepository.ObterPorIdAsync(pedido.Id);
        return MapearParaDto(pedidoCriado!);
    }

    // ════════════════════════════════════════
    // ATUALIZAR STATUS — confirmar ou cancelar
    // ════════════════════════════════════════
    public async Task AtualizarStatusAsync(int id, StatusPedido novoStatus)
    {
        var pedido = await _pedidoRepository.ObterPorIdAsync(id);
        if (pedido == null)
            throw new KeyNotFoundException($"Pedido com Id {id} não encontrado.");

        /* Pedido cancelado não pode ser reaberto */
        if (pedido.Status == StatusPedido.Cancelado)
            throw new InvalidOperationException("Um pedido cancelado não pode ser alterado.");

        pedido.Status = novoStatus;
        await _pedidoRepository.AtualizarAsync(pedido);
    }

    // ════════════════════════════════════════
    // CANCELAR — atalho semântico
    // ════════════════════════════════════════
    public async Task CancelarAsync(int id)
        => await AtualizarStatusAsync(id, StatusPedido.Cancelado);

    // ════════════════════════════════════════
    // REMOVER
    // ════════════════════════════════════════
    public async Task RemoverAsync(int id)
    {
        var pedido = await _pedidoRepository.ObterPorIdAsync(id);
        if (pedido == null)
            throw new KeyNotFoundException($"Pedido com Id {id} não encontrado.");

        await _pedidoRepository.RemoverAsync(id);
    }

    // ════════════════════════════════════════
    // MAPPER — Entidade → DTO
    // ════════════════════════════════════════
    private static PedidoDto MapearParaDto(Pedido pedido) => new()
    {
        Id = pedido.Id,
        ClienteId = pedido.ClienteId,
        NomeCliente = pedido.Cliente?.NomeCliente ?? string.Empty,
        DataPedido = pedido.DataPedido,
        ValorTotal = pedido.ValorTotal,
        Status = pedido.Status,
        Itens = pedido.Itens.Select(i => new ItemPedidoDto
        {
            Id = i.Id,
            ProdutoId = i.ProdutoId,
            NomeProduto = i.Produto?.Nome ?? string.Empty,
            Quantidade = i.Quantidade,
            PrecoUnitario = i.PrecoVenda,
            Subtotal = i.Subtotal,
        }).ToList(),
    };
}