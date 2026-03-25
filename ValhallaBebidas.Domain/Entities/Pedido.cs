using ValhallaBebidas.Domain.Enums;

namespace ValhallaBebidas.Domain.Entities; 

public class Pedido
{
    public int Id { get; set; }
    public int ClienteId { get; set; } /* chave estrangeira para o cliente */
    public DateTime DataPedido { get; set; } = DateTime.Now;
    public decimal ValorTotal { get; set; }
    public StatusPedido Status { get; set; } = StatusPedido.Pendente; /* enum em vez de string */

    public Cliente? Cliente { get; set; }
    public ICollection<ItemPedido> Itens { get; set; } = new List<ItemPedido>();

    /// <summary>
    /// Recalcula o ValorTotal com base nos itens atuais.
    /// Útil após adicionar ou remover itens do pedido.
    /// </summary>
    public void RecalcularTotal()
    {
        ValorTotal = Itens.Sum(item => item.Quantidade * (item.Produto?.PrecoVenda ?? 0)); // Se Produto for null, considera preço como 0 para evitar erros.
    }
}
