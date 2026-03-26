using ValhallaBebidas.Application.DTOs;
using ValhallaBebidas.Domain.Entities;
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

    public async Task<IEnumerable<PedidoDto>> ListarTodosAsync()
    {
        var pedidos = await _pedidoRepository.ListarTodosAsync();
        return pedidos.Select(MapearParaDto);
    }

    public async Task<PedidoDto?> ObterPorIdAsync(int id)
    {
        var pedido = await _pedidoRepository.ObterPorIdAsync(id);
        return pedido == null ? null : MapearParaDto(pedido);
    }

    public async Task<IEnumerable<PedidoDto>> ListarPorClienteAsync(int clienteId)
    {
        var pedidos = await _pedidoRepository.ListarPorClienteAsync(clienteId);
        return pedidos.Select(MapearParaDto);
    }

    public async Task<PedidoDto> CriarAsync(CriarPedidoDto dto)
    {
        // Regra de negócio: deve ter pelo menos 1 item
        if (dto.Itens == null || dto.Itens.Count == 0)
            throw new InvalidOperationException("Um pedido deve conter pelo menos 1 item.");

        // Verifica se o cliente existe
        var cliente = await _clienteRepository.ObterPorIdAsync(dto.ClienteId);
        if (cliente == null)
            throw new KeyNotFoundException($"Cliente com Id {dto.ClienteId} não encontrado.");

        // Monta os itens verificando se os produtos existem
        var itens = new List<ItemPedido>();
        foreach (var itemDto in dto.Itens)
        {
            var produto = await _produtoRepository.ObterPorIdAsync(itemDto.ProdutoId);
            if (produto == null)
                throw new KeyNotFoundException($"Produto com Id {itemDto.ProdutoId} não encontrado.");

            itens.Add(new ItemPedido
            {
                ProdutoId = produto.Id,
                Quantidade = itemDto.Quantidade,
                PrecoVenda = produto.PrecoVenda // Captura o preço atual do produto
            });
        }

        // Cria o pedido e calcula o total automaticamente
        var pedido = new Pedido
        {
            ClienteId = dto.ClienteId,
            DataPedido = DateTime.Now,
            Status = "Pendente",
            Itens = itens
        };

        // Regra: Total calculado com base nos itens
        pedido.RecalcularTotal();

        await _pedidoRepository.AdicionarAsync(pedido);

        // Recarrega com as informações completas
        var pedidoCriado = await _pedidoRepository.ObterPorIdAsync(pedido.Id);
        return MapearParaDto(pedidoCriado!);
    }

    public async Task RemoverAsync(int id)
    {
        var pedido = await _pedidoRepository.ObterPorIdAsync(id);
        if (pedido == null)
            throw new KeyNotFoundException($"Pedido com Id {id} não encontrado.");

        await _pedidoRepository.RemoverAsync(id);
    }

    /// <summary>
    /// Atualiza um pedido existente: troca o cliente e reconstrói todos os itens.
    /// O total é recalculado automaticamente com os preços atuais dos produtos.
    /// </summary>
    public async Task<PedidoDto> AtualizarAsync(int id, AtualizarPedidoDto dto)
    {
        if (dto.Itens == null || dto.Itens.Count == 0)
            throw new InvalidOperationException("Um pedido deve conter pelo menos 1 item.");

        var pedido = await _pedidoRepository.ObterPorIdAsync(id);
        if (pedido == null)
            throw new KeyNotFoundException($"Pedido com Id {id} não encontrado.");

        var cliente = await _clienteRepository.ObterPorIdAsync(dto.ClienteId);
        if (cliente == null)
            throw new KeyNotFoundException($"Cliente com Id {dto.ClienteId} não encontrado.");

        // Reporta o cliente se necessário
        pedido.ClienteId = dto.ClienteId;
        pedido.Status = dto.Status ?? pedido.Status;

        // Remove os itens antigos e substitui pelos novos
        pedido.Itens.Clear();

        foreach (var itemDto in dto.Itens)
        {
            var produto = await _produtoRepository.ObterPorIdAsync(itemDto.ProdutoId);
            if (produto == null)
                throw new KeyNotFoundException($"Produto com Id {itemDto.ProdutoId} não encontrado.");

            pedido.Itens.Add(new ItemPedido
            {
                ProdutoId = produto.Id,
                Quantidade = itemDto.Quantidade,
                PrecoUnitario = produto.Preco
            });
        }

        pedido.RecalcularTotal();
        await _pedidoRepository.AtualizarAsync(pedido);

        // Recarrega com dados completos (Cliente e Itens com Produto)
        var atualizado = await _pedidoRepository.ObterPorIdAsync(pedido.Id);
        return MapearParaDto(atualizado!);
    }

    /// <summary>Método auxiliar para converter Pedido (entidade) em PedidoDto</summary>
    private static PedidoDto MapearParaDto(Pedido pedido)
    {
        return new PedidoDto
        {
            Id = pedido.Id,
            ClienteId = pedido.ClienteId,
            NomeCliente = pedido.Cliente?.Nome ?? string.Empty,
            DataPedido = pedido.DataPedido,
            Total = pedido.Total,
            Status = pedido.Status,
            Itens = pedido.Itens.Select(i => new ItemPedidoDto
            {
                Id = i.Id,
                ProdutoId = i.ProdutoId,
                NomeProduto = i.Produto?.Nome ?? string.Empty,
                Quantidade = i.Quantidade,
                PrecoUnitario = i.PrecoUnitario
            }).ToList()
        };
    }

}
