using ValhallaBebidas.Domain.Entities;

namespace ValhallaBebidas.Application.DTOs;

public class PedidoDto
{
    public int Id { get; set; }
    public int ClienteId { get; set; }
    public DateTime DataPedido { get; set; } = DateTime.Now;
    public decimal ValorTotal { get; set; }
    public string Status { get; set; } = "Pendente";
    public List<ItemPedidoDto> Itens { get; set; } = new();
}

public class ItemPedidoDto
{
    public int Id { get; set; } 
    public int ProdutoId { get; set; }
    public int Quantidade { get; set; }
    //Instanciando um produto para acessar o preço de venda, para calcular o subtotal.
    Produto p = new Produto();
    public decimal Subtotal => Quantidade * p.PrecoVenda;
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
    public int ClienteId { get; set; }
    public string Status { get; set; } = "Pendente";
    public List<CriarItemPedidoDto> Itens { get; set; } = new();
}
