using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using ValhallaBebidas.UI.Services.Models;

namespace ValhallaBebidas.UI
{
    public partial class ucPedidos : UserControl
    {

        private readonly PedidoApiService _pedidoService = new();
        private List<PedidoDto> _pedidos = new();
        public ucPedidos()
        {
            InitializeComponent();
            ConfigurarDataGridView();

            cmbFiltroStatus.SelectedIndexChanged += cmbFiltroStatus_SelectedIndexChanged;
            dgvPedidos.CellFormatting += DgvPedidos_CellFormatting;
            Load += async (s, e) => await CarregarPedidosAsync();
        }

        private void ConfigurarDataGridView()
        {
            dgvPedidos.Columns.Add("colId", "ID");
            dgvPedidos.Columns.Add("colCliente", "Cliente");
            dgvPedidos.Columns.Add("colData", "Data");
            dgvPedidos.Columns.Add("colTotal", "Total (R$)");
            dgvPedidos.Columns.Add("colItens", "Qtd. Itens");
            dgvPedidos.Columns.Add("colStatus", "Status");

            dgvPedidos.Columns["colId"]!.FillWeight = 30;
            dgvPedidos.Columns["colCliente"]!.FillWeight = 200;
            dgvPedidos.Columns["colData"]!.FillWeight = 100;
            dgvPedidos.Columns["colTotal"]!.FillWeight = 100;
            dgvPedidos.Columns["colItens"]!.FillWeight = 60;
            dgvPedidos.Columns["colStatus"]!.FillWeight = 100;
        }

        private async Task CarregarPedidosAsync()
        {
            _pedidos = await _pedidoService.GetPedidosAsync();
            FiltrarGrid();
        }

        private void cmbFiltroStatus_SelectedIndexChanged(object sender, EventArgs e)
        {
            FiltrarGrid();
        }

        private void FiltrarGrid()
        {
            dgvPedidos.Rows.Clear();
            string statusSelecionado = cmbFiltroStatus.SelectedItem?.ToString() ?? "Todos";

            var exibidos = statusSelecionado == "Todos"
    ? _pedidos
    : _pedidos.Where(p => p.Status.ToString().Equals(statusSelecionado, StringComparison.OrdinalIgnoreCase)).ToList();

            foreach (var p in exibidos)
            {
                dgvPedidos.Rows.Add(
                    p.Id,
                    p.NomeCliente,
                    p.DataPedido.ToString("dd/MM/yyyy"),
                    p.Total.ToString("C2"),
                    p.Itens.Count,
                    p.Status.ToString()); // Adicione o .ToString() aqui
            }
        }

        /// <summary>
        /// Aplica cores de fundo nas linhas do DataGrid conforme o Status do pedido.
        /// Pendente   → vermelho claro
        /// Finalizado → verde claro
        /// Cancelado  → cinza claro
        /// </summary>
        private void DgvPedidos_CellFormatting(object? sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.RowIndex < 0 || e.RowIndex >= dgvPedidos.Rows.Count) return;

            var row = dgvPedidos.Rows[e.RowIndex];
            var status = row.Cells["colStatus"]?.Value?.ToString() ?? string.Empty;

            (Color bg, Color fg) = status switch
            {
                "Finalizado" => (Color.FromArgb(200, 230, 201), Color.FromArgb(0, 100, 20)),  // verde claro
                "Pendente" => (Color.FromArgb(255, 243, 205), Color.FromArgb(130, 90, 0)),  // amarelo claro
                "Cancelado" => (Color.FromArgb(255, 205, 210), Color.FromArgb(120, 0, 0)),   // vermelho claro
                _ => (Color.White, Color.Black)
            };

            row.DefaultCellStyle.BackColor = bg;
            row.DefaultCellStyle.ForeColor = fg;
            row.DefaultCellStyle.SelectionBackColor = bg;
            row.DefaultCellStyle.SelectionForeColor = fg;
        }

        private PedidoDto? ObterPedidoSelecionado(string acaoParaMensagem)
        {
            if (dgvPedidos.CurrentRow == null || _pedidos.Count == 0)
            {
                MessageBox.Show($"Selecione um pedido para {acaoParaMensagem}.",
                    "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return null;
            }

            int id = Convert.ToInt32(dgvPedidos.CurrentRow.Cells["colId"].Value);
            return _pedidos.FirstOrDefault(p => p.Id == id);
        }

        private void btnNovoPedido_Click(object sender, EventArgs e)
        {
            (this.FindForm() as frmPrincipal)?.Navegar(new ucNovoPedido());
        }

        private void btnEditarPedido_Click(object sender, EventArgs e)
        {
            var pedido = ObterPedidoSelecionado("editar");
            if (pedido == null) return;

            (this.FindForm() as frmPrincipal)?.Navegar(new ucNovoPedido(pedido.Id));
        }

        private async void btnExcluirPedido_Click(object sender, EventArgs e)
        {
            var pedido = ObterPedidoSelecionado("excluir");
            if (pedido == null) return;

            var confirm = MessageBox.Show(
                $"Deseja excluir o Pedido #{pedido.Id} de {pedido.NomeCliente}?\n" +
                $"Total: {pedido.Total:C2} — {pedido.Itens.Count} item(ns).\n\n" +
                "Essa ação não pode ser desfeita.",
                "Confirmar Exclusão", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (confirm != DialogResult.Yes) return;

            bool ok = await _pedidoService.DeletePedidoAsync(pedido.Id);
            if (ok)
            {
                MessageBox.Show("Pedido excluído com sucesso!", "Sucesso",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                await CarregarPedidosAsync();
            }
        }
    }
}
