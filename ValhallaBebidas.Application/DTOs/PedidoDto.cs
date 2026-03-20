using ValhallaBebidas.Domain.Enums;

namespace ValhallaBebidas.Application.DTOs;


public class PedidoDto
{
    public int Id { get; set; }
    public int ClienteId { get; set; }
    public string NomeCliente { get; set; } = string.Empty;
    public DateTime DataPedido { get; set; }
    public decimal ValorTotal { get; set; }
    public StatusPedido Status { get; set; }
    public List<ItemPedidoDto> Itens { get; set; } = new();
}

public class ItemPedidoDto
{
    public int Id { get; set; }
    public int ProdutoId { get; set; }
    public string NomeProduto { get; set; } = string.Empty; /* útil para exibir */
    public int Quantidade { get; set; }
    public decimal PrecoUnitario { get; set; } /* backend preenche com PrecoVenda */
    public decimal Subtotal { get; set; } /* backend calcula e envia pronto */
}

public class CriarPedidoDto
{
    public int ClienteId { get; set; }
    public List<CriarItemPedidoDto> Itens { get; set; } = new();
}

public class CriarItemPedidoDto
{
    public int ProdutoId { get; set; }
    public int Quantidade { get; set; }
}

public class AtualizarPedidoDto
{
    /* Id vem pela URL — não precisa no body */
    public StatusPedido Status { get; set; }
    public List<CriarItemPedidoDto> Itens { get; set; } = new();
}
