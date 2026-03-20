namespace ValhallaBebidas.Domain.Entities; 

public class Pedido
{
    public int Id { get; set; }
    public int ClienteId { get; set; }//Chave estrangeira para o cliente que fez o pedido
    public DateTime DataPedido { get; set; } = DateTime.Now;//pega o horário que efetuou, sem precisar fazer no back
    public decimal ValorTotal { get; set; } //Valor total do pedido, calculado a partir da soma dos itens
    public string Status { get; set; } = "Pendente"; // Valores possíveis: Pendente, Finalizado, Cancelado.
    
    
    public Cliente? Cliente { get; set; } //Propriedades de navegação
    public ICollection<ItemPedido> Itens { get; set; } = new List<ItemPedido>(); // Coleção de itens deste pedido. Relacionamento: 1 Pedido possui N ItensPedido.


    /// <summary>
    /// Recalcula o Total com base nos itens atuais.
    /// Útil após adicionar/remover itens.
    /// </summary>
    public void RecalcularTotal()
    {
        ValorTotal = Itens.Sum(item => item.Quantidade * item.Produto.PrecoVenda);                                         
    }
}
