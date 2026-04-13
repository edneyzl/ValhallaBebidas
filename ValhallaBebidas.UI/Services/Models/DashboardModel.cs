namespace ValhallaBebidas.UI.Services.Models
{
    /// <summary>
    /// Espelho local do retorno de GET /api/dashboard.
    /// </summary>
    public class DashboardDto
    {
        public decimal TotalVendas        { get; set; }
        public int     TotalClientes      { get; set; }
        public int     TotalProdutos      { get; set; }
        public int     TotalPedidos       { get; set; }
        public int     PedidosPendentes   { get; set; }
        public int     PedidosConcluidos  { get; set; }
        public int     PedidosCancelados  { get; set; }
        public List<PedidoMesDto> PedidosPorMes { get; set; } = new();
    }

    public class PedidoMesDto
    {
        public int Ano   { get; set; }
        public int Mes   { get; set; }
        public int Total { get; set; }
    }
}
