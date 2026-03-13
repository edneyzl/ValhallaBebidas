namespace ValhallaBebidas.Domain.Entities; 

public class Pedido
{
    public int Id { get; set; }
    public DateTime DataPedido { get; set; } = DateTime.Now;//pega o horário que efetuou, sem precisar fazer no back
    public decimal ValorTotal { get; set; }
    public int ClienteId { get; set; }

    //propriedades de navegação, referência entre entidades, possui o tipo da classe 
    public Cliente? Cliente { get; set; }

    public ICollection<ItemPedido> Itens { get; set; } = new List<ItemPedido>();

    public void RecalcularTotal()
    {
        ValorTotal = Itens.Sum(item => item.Quantidade * item.Produto.PrecoVenda);                                         
    }
}
