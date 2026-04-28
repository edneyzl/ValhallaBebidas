using ValhallaBebidas.Domain.Enums;

namespace ValhallaBebidas.UI.Services.Models


{
    /// <summary>
    /// Espelhos locais dos DTOs de Pedido da ValhallaBebidas.API.
    /// </summary>

    // Resposta de GET /api/pedido
    public class PedidoDto
    {
        public int               Id          { get; set; }
        public int               ClienteId   { get; set; }
        public string            NomeCliente { get; set; } = string.Empty;
        public DateTime          DataPedido  { get; set; }
        public decimal           Total       { get; set; }
        public StatusPedido       Status      { get; set; } 
        public List<ItemPedidoDto> Itens     { get; set; } = new();
    }

    public class ItemPedidoDto
    {
        public int     Id            { get; set; }
        public int     ProdutoId     { get; set; }
        public string  NomeProduto   { get; set; } = string.Empty;
        public int     Quantidade    { get; set; }
        public decimal PrecoUnitario { get; set; }
        public decimal Subtotal      => Quantidade * PrecoUnitario;
    }

    // Corpo de POST /api/pedido
    public class CriarPedidoDto
    {
        public int                    ClienteId { get; set; }
        public StatusPedido             Status    { get; set; } 
        public List<CriarItemPedidoDto> Itens   { get; set; } = new();
    }

    public class CriarItemPedidoDto
    {
        public int ProdutoId  { get; set; }
        public int Quantidade { get; set; }
    }

    // Corpo de PUT /api/pedido/{id}
    public class AtualizarPedidoDto
    {
        public int                      ClienteId { get; set; }
        public StatusPedido             Status    { get; set; } 
        public List<CriarItemPedidoDto> Itens     { get; set; } = new();
    }
}
