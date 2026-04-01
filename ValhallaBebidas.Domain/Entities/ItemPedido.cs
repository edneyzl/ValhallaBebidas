namespace ValhallaBebidas.Domain.Entities;

public class ItemPedido 
{
    public int Id { get; set; }
    /// <summary>Chave estrangeira para o pedido</summary>
    public int PedidoId { get; set; }
    /// <summary>Chave estrangeira para o produto</summary>
    public int ProdutoId { get; set; }
    /// <summary>Quantidade deste produto no pedido</summary>
    public int Quantidade { get; set; }

    /// <summary>
    /// Preço por unidade adotado no momento da criação do item no pedido.
    /// Mantém o histórico mesmo que o preço do produto mude posteriormente.
    /// </summary>
    public decimal PrecoUnitario { get; set; }

    // Calcula o subtotal usando o PrecoUnitario quando disponível; caso contrário
    // faz fallback para o preço do produto (compatibilidade).
    public decimal Subtotal =>
         Quantidade * (PrecoUnitario != 0 ? PrecoUnitario : (Produto?.PrecoVenda ?? 0));

    //propriedades de navegação, referência entre entidades, possui o tipo da classe 
    public Pedido? Pedido { get; set; }
    public Produto? Produto { get; set; }
}
