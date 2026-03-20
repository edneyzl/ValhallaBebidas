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
    //calculo dentro de uma classe - não é boa e nem má pratica - á tipico
    public decimal Subtotal => Quantidade * (Produto?.PrecoVenda ?? 0); // Subtotal é o valor total do item, ou seja, a quantidade vezes o preço de venda do produto. O operador => é usado para criar uma propriedade de expressão, que calcula o valor do subtotal sempre que for acessada, sem a necessidade de armazenar esse valor em um campo separado.

    //propriedades de navegação, referência entre entidades, possui o tipo da classe 
    public Pedido? Pedido { get; set; }
    public Produto? Produto { get; set; }   
}
