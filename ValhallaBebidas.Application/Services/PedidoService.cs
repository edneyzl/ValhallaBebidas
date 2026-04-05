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
    private readonly IUnitOfWork _unitOfWork;

    public PedidoService(
        IPedidoRepository pedidoRepository,
        IClienteRepository clienteRepository,
        IProdutoRepository produtoRepository,
        IMovimentacaoRepository movimentacaoRepository,
        IUnitOfWork unitOfWork)
    {
        _pedidoRepository = pedidoRepository;
        _clienteRepository = clienteRepository;
        _produtoRepository = produtoRepository;
        _movimentacaoRepository = movimentacaoRepository;
        _unitOfWork = unitOfWork;
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

        /* Batch load — 1 query para todos os produtos (elimina N+1) */
        var produtoIds = dto.Itens.Select(i => i.ProdutoId).Distinct();
        var produtos = (await _produtoRepository.ObterPorIdsAsync(produtoIds)).ToDictionary(p => p.Id);

        /* Monta os itens e registra baixa de estoque */
        var itens = new List<ItemPedido>();
        foreach (var itemDto in dto.Itens)
        {
            if (!produtos.TryGetValue(itemDto.ProdutoId, out var produto))
                throw new KeyNotFoundException($"Produto com Id {itemDto.ProdutoId} não encontrado.");

            /* Estoque suficiente — validação no mesmo snapshot */
            if (produto.QuantidadeEstoque < itemDto.Quantidade)
                throw new InvalidOperationException(
                    $"Estoque insuficiente para '{produto.Nome}'. Disponível: {produto.QuantidadeEstoque}.");

            itens.Add(new ItemPedido
            {
                ProdutoId = produto.Id,
                Quantidade = itemDto.Quantidade,
                PrecoUnitario = produto.PrecoVenda, /* captura o preço atual */
            });

            /* Decrementa estoque e registra movimentação no mesmo SaveChanges */
            produto.QuantidadeEstoque -= itemDto.Quantidade;

            await _movimentacaoRepository.AdicionarAsync(new Movimentacao
            {
                ProdutoId = produto.Id,
                Quantidade = itemDto.Quantidade,
                Direcao = DirecaoMovimentacao.Saida,
                Motivo = $"Pedido pendente",
                Data = DateTime.UtcNow,
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
        await _unitOfWork.SaveChangesAsync();

        /* Retorna do change tracker — sem nova query */
        var salvo = await _pedidoRepository.ObterPorIdAsync(pedido.Id);
        return MapearParaDto(salvo!);
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
        await _unitOfWork.SaveChangesAsync();
    }

    // ════════════════════════════════════════
    // CANCELAR — reverte estoque e registra estorno
    // O pedido deve ter itens carregados pelo repo
    // ════════════════════════════════════════
    public async Task CancelarAsync(int id, int clienteId)
    {
        var pedido = await _pedidoRepository.ObterPorIdAsync(id);

        if (pedido == null)
        throw new KeyNotFoundException($"Pedido com Id {id} não encontrado.");

        var produtoIds = pedido.Itens.Select(i => i.ProdutoId).Distinct();
        var produtos = (await _produtoRepository.ObterPorIdsAsync(produtoIds)).ToDictionary(p => p.Id);

        foreach (var item in pedido.Itens)
        {
            if (produtos.TryGetValue(item.ProdutoId, out var produto))
            {
                produto.QuantidadeEstoque += item.Quantidade;

                await _movimentacaoRepository.AdicionarAsync(new Movimentacao
                {
                    // ... sua lógica de movimentação ...
                });
            }
        }

        pedido.Status = StatusPedido.Cancelado;
        await _unitOfWork.SaveChangesAsync();
    }

    // ════════════════════════════════════════
    // REMOVER
    // ════════════════════════════════════════
    public async Task RemoverAsync(int id)
    {
        var pedido = await _pedidoRepository.ObterPorIdAsync(id);
        if (pedido == null)
            throw new KeyNotFoundException($"Pedido com Id {id} não encontrado.");

        await _pedidoRepository.RemoverAsync(id);
        await _unitOfWork.SaveChangesAsync();
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