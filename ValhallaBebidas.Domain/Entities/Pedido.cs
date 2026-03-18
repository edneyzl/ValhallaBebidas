namespace ValhallaBebidas.Domain.Entities; 

public class Pedido
{
    public int Id { get; set; }
    public DateTime DataPedido { get; set; } = DateTime.Now;//pega o horário que efetuou, sem precisar fazer no back
    public decimal ValorTotal { get; set; } //Valor total do pedido, calculado a partir da soma dos itens
    public int ClienteId { get; set; }//Chave estrangeira para o cliente que fez o pedido

    // Propriedades de navegação (EF Core)
    /// <summary>Referência ao cliente (objeto completo)</summary>
    public Cliente? Cliente { get; set; }

    /// <summary>
    /// Status atual do pedido.
    /// Valores possíveis: Pendente, Finalizado, Cancelado.
    /// </summary>
    public string Status { get; set; } = "Pendente";


    /// <summary>
    /// Coleção de itens deste pedido.
    /// Relacionamento: 1 Pedido possui N ItensPedido.
    /// Regra: deve conter pelo menos 1 item.
    /// </summary>

    public ICollection<ItemPedido> Itens { get; set; } = new List<ItemPedido>();


    /// <summary>
    /// Recalcula o Total com base nos itens atuais.
    /// Útil após adicionar/remover itens.
    /// </summary>
    public void RecalcularTotal()
    {
        ValorTotal = Itens.Sum(item => item.Quantidade * item.Produto.PrecoVenda);                                         
    }
}
