using ValhallaBebidas.Domain.Enums;

namespace ValhallaBebidas.Domain.Entities; 

public class Pedido
{
    public int Id { get; set; }
    public int ClienteId { get; set; } /* chave estrangeira para o cliente */
    public DateTime DataPedido { get; set; } = DateTime.UtcNow;
    public decimal ValorTotal { get; set; }
    public StatusPedido Status { get; set; } = StatusPedido.Pendente; /* enum em vez de string */

    /// <summary>
    /// Endereço de entrega do pedido (flattened fields — value object).
    /// </summary>
    public string? EnderecoEntregaLogradouro { get; set; }
    public string? EnderecoEntregaNumero { get; set; }
    public string? EnderecoEntregaComplemento { get; set; }
    public string? EnderecoEntregaBairro { get; set; }
    public string? EnderecoEntregaCidade { get; set; }
    public string? EnderecoEntregaEstado { get; set; }
    public string? EnderecoEntregaCep { get; set; }

    public Cliente? Cliente { get; set; }
    public ICollection<ItemPedido> Itens { get; set; } = new List<ItemPedido>();

    /// <summary>
    /// Recalcula o ValorTotal com base nos itens atuais.
    /// Útil após adicionar ou remover itens do pedido.
    /// </summary>
    public void RecalcularTotal()
    {
        ValorTotal = Itens.Sum(item => item.Quantidade * item.PrecoUnitario);
    }
}
