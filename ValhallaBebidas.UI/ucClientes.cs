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
    public partial class ucClientes : UserControl
    {
        private readonly ClienteApiService _clienteService = new();
        private List<ClienteDto> _clientes = new();
        public ucClientes()
        {
            InitializeComponent();
            ConfigurarInterface();
            // 
            Load += async (s, e) => await CarregarClientesAsync();
        }

        private void ConfigurarInterface()
        {
            dgvClientes.Columns.Add("colId", "ID");
            dgvClientes.Columns.Add("colNome", "Nome");
            dgvClientes.Columns.Add("colCPF", "CPF");

            dgvClientes.Columns["colId"]!.FillWeight = 30;
            dgvClientes.Columns["colNome"]!.FillWeight = 250;
            dgvClientes.Columns["colCPF"]!.FillWeight = 150;
        }

        private async Task CarregarClientesAsync(string filtro = "")
        {
            if (_clientes.Count == 0 || string.IsNullOrEmpty(filtro))
                _clientes = await _clienteService.GetClientesAsync();

            AtualizarGrid(_clientes, filtro);
        }

        private void AtualizarGrid(List<ClienteDto> lista, string filtro = "")
        {
            dgvClientes.Rows.Clear();

            var exibidos = string.IsNullOrWhiteSpace(filtro)
                ? lista
                : lista.Where(c =>
                    c.Nome.Contains(filtro, StringComparison.OrdinalIgnoreCase) ||
                    c.Documento.Contains(filtro, StringComparison.OrdinalIgnoreCase)).ToList();

            foreach (var c in exibidos)
                dgvClientes.Rows.Add(c.Id, c.Nome, c.Documento);
        }

        private void txtBuscaCliente_TextChanged(object sender, EventArgs e)
        {
            AtualizarGrid(_clientes, txtBuscaCliente.Text);
        }

        private void btnNovoCliente_Click(object sender, EventArgs e)
        {
            (this.FindForm() as frmPrincipal)?.Navegar(new ucNovoCliente());
        }

        private void btnEditarCliente_Click(object sender, EventArgs e)
        {
            if (dgvClientes.CurrentRow == null)
            {
                MessageBox.Show("Selecione um registro para editar.",
                    "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int id = Convert.ToInt32(dgvClientes.CurrentRow.Cells["colId"].Value);
            (this.FindForm() as frmPrincipal)?.Navegar(new ucNovoCliente(id));
        }

        private async void btnExcluirCliente_Click(object sender, EventArgs e)
        {
            if (dgvClientes.CurrentRow == null)
            {
                MessageBox.Show("Selecione um cliente para excluir.",
                    "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int id = Convert.ToInt32(dgvClientes.CurrentRow.Cells["colId"].Value);
            string nome = dgvClientes.CurrentRow.Cells["colNome"].Value?.ToString() ?? "";

            if (MessageBox.Show(
                    $"Excluir \"{nome}\"? Esta ação não pode ser desfeita.",
                    "Confirmar Exclusão", MessageBoxButtons.YesNo, MessageBoxIcon.Question)
                != DialogResult.Yes) return;

            bool ok = await _clienteService.DeleteClienteAsync(id);
            if (ok)
            {
                MessageBox.Show("Cliente excluído com sucesso!", "Sucesso",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                _clientes.Clear();
                await CarregarClientesAsync();
            }
        }
    }
}
