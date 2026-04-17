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
    private readonly IMovimentacaoRepository _movimentacaoRepository;

    public PedidoService(
        IPedidoRepository pedidoRepository,
        IClienteRepository clienteRepository,
        IProdutoRepository produtoRepository,
        IMovimentacaoRepository movimentacaoRepository)
    {
        _pedidoRepository = pedidoRepository;
        _clienteRepository = clienteRepository;
        _produtoRepository = produtoRepository;
        _movimentacaoRepository = movimentacaoRepository;
    }

    public async Task<IEnumerable<PedidoDto>> ListarTodosAsync()
    {
        var pedidos = await _pedidoRepository.ListarTodosAsync();
        return pedidos.Select(MapearParaDto);
    }

    public async Task<IEnumerable<PedidoDto>> ListarPorClienteAsync(int clienteId)
    {
        var pedidos = await _pedidoRepository.ListarPorClienteAsync(clienteId);
        return pedidos.Select(MapearParaDto);
    }

    public async Task<PedidoDto?> ObterPorIdAsync(int id)
    {
        var pedido = await _pedidoRepository.ObterPorIdAsync(id);
        return pedido == null ? null : MapearParaDto(pedido);
    }

    public async Task<PedidoDto> CriarAsync(CriarPedidoDto dto)
    {
        if (dto.Itens == null || dto.Itens.Count == 0)
            throw new InvalidOperationException("Um pedido deve conter pelo menos 1 item.");

        var cliente = await _clienteRepository.ObterPorIdAsync(dto.ClienteId);
        if (cliente == null)
            throw new KeyNotFoundException($"Cliente com Id {dto.ClienteId} não encontrado.");

        var produtoIds = dto.Itens.Select(i => i.ProdutoId).Distinct();
        var produtos = (await _produtoRepository.ObterPorIdsAsync(produtoIds))
            .ToDictionary(p => p.Id);

        var itens = new List<ItemPedido>();

        foreach (var itemDto in dto.Itens)
        {
            if (!produtos.TryGetValue(itemDto.ProdutoId, out var produto))
                throw new KeyNotFoundException($"Produto com Id {itemDto.ProdutoId} não encontrado.");

            if (produto.QuantidadeEstoque < itemDto.Quantidade)
                throw new InvalidOperationException(
                    $"Estoque insuficiente para '{produto.Nome}'. Disponível: {produto.QuantidadeEstoque}.");

            itens.Add(new ItemPedido
            {
                ProdutoId = produto.Id,
                Quantidade = itemDto.Quantidade,
                PrecoUnitario = produto.PrecoVenda
            });
        }

        var pedido = new Pedido
        {
            ClienteId = dto.ClienteId,
            DataPedido = DateTime.UtcNow,
            Status = StatusPedido.Pendente,
            Itens = itens,
            EnderecoEntregaLogradouro = dto.Entrega?.Logradouro,
            EnderecoEntregaNumero = dto.Entrega?.Numero,
            EnderecoEntregaComplemento = dto.Entrega?.Complemento,
            EnderecoEntregaBairro = dto.Entrega?.Bairro,
            EnderecoEntregaCidade = dto.Entrega?.Cidade,
            EnderecoEntregaEstado = dto.Entrega?.Estado,
            EnderecoEntregaCep = dto.Entrega?.Cep,
        };

        pedido.RecalcularTotal();

        await _pedidoRepository.AdicionarAsync(pedido);

        foreach (var item in pedido.Itens)
        {
            var produto = produtos[item.ProdutoId];

            produto.QuantidadeEstoque -= item.Quantidade;
            await _produtoRepository.AtualizarAsync(produto);

            await _movimentacaoRepository.AdicionarAsync(new Movimentacao
            {
                ProdutoId = produto.Id,
                Quantidade = item.Quantidade,
                Direcao = DirecaoMovimentacao.Saida,
                Motivo = $"Reserva de estoque — pedido pendente #{pedido.Id}",
                Data = DateTime.UtcNow
            });
        }

        await _pedidoRepository.AtualizarAsync(pedido);
        await _pedidoRepository.SaveAsync();

        var salvo = await _pedidoRepository.ObterPorIdAsync(pedido.Id);
        return MapearParaDto(salvo!);
    }

    public async Task AtualizarStatusAsync(int id, StatusPedido novoStatus)
    {
        var pedido = await _pedidoRepository.ObterPorIdAsync(id);
        if (pedido == null)
            throw new KeyNotFoundException($"Pedido com Id {id} não encontrado.");

        if (pedido.Status == StatusPedido.Cancelado)
            throw new InvalidOperationException("Um pedido cancelado não pode ser alterado.");

        pedido.Status = novoStatus;

        await _pedidoRepository.AtualizarAsync(pedido);
        await _pedidoRepository.SaveAsync();
    }

    public async Task CancelarAsync(int id, int clienteId)
    {
        var pedido = await _pedidoRepository.ObterPorIdAsync(id);
        if (pedido == null)
            throw new KeyNotFoundException($"Pedido com Id {id} não encontrado.");

        if (pedido.ClienteId != clienteId)
            throw new InvalidOperationException("Você não tem permissão para cancelar este pedido.");

        if (pedido.Status == StatusPedido.Cancelado)
            throw new InvalidOperationException("Pedido já está cancelado.");

        var produtoIds = pedido.Itens.Select(i => i.ProdutoId).Distinct();
        var produtos = (await _produtoRepository.ObterPorIdsAsync(produtoIds))
            .ToDictionary(p => p.Id);

        foreach (var item in pedido.Itens)
        {
            var produto = produtos[item.ProdutoId];

            produto.QuantidadeEstoque += item.Quantidade;
            await _produtoRepository.AtualizarAsync(produto);

            await _movimentacaoRepository.AdicionarAsync(new Movimentacao
            {
                ProdutoId = produto.Id,
                Quantidade = item.Quantidade,
                Direcao = DirecaoMovimentacao.Entrada,
                Motivo = $"Estorno — cancelamento pedido #{id}",
                Data = DateTime.UtcNow
            });
        }

        pedido.Status = StatusPedido.Cancelado;
        await _pedidoRepository.AtualizarAsync(pedido);
        await _pedidoRepository.SaveAsync();
    }

    public async Task RemoverAsync(int id)
    {
        var pedido = await _pedidoRepository.ObterPorIdAsync(id);
        if (pedido == null)
            throw new KeyNotFoundException($"Pedido com Id {id} não encontrado.");

        await _pedidoRepository.RemoverAsync(id);
        await _pedidoRepository.SaveAsync();
    }

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
            PrecoUnitario = i.PrecoUnitario,
            Subtotal = i.Subtotal,
        }).ToList(),
        EnderecoEntrega = pedido.EnderecoEntregaLogradouro == null ? null : new EnderecoEntregaDto
        {
            Logradouro = pedido.EnderecoEntregaLogradouro,
            Numero = pedido.EnderecoEntregaNumero ?? string.Empty,
            Complemento = pedido.EnderecoEntregaComplemento ?? string.Empty,
            Bairro = pedido.EnderecoEntregaBairro ?? string.Empty,
            Cidade = pedido.EnderecoEntregaCidade ?? string.Empty,
            Estado = pedido.EnderecoEntregaEstado ?? string.Empty,
            Cep = pedido.EnderecoEntregaCep ?? string.Empty,
        },
    };
}