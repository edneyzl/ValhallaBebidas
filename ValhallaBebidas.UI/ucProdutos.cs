using SenacBuy.UI.Services.Models;
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
    public partial class ucProdutos : UserControl
    {

        private readonly ProdutoApiService _produtoService = new();
        private List<ProdutoDto> _produtos = new();

        public ucProdutos()
        {
            InitializeComponent();
            ConfigurarInterface();
            Load += async (s, e) => await CarregarProdutosAsync();
        }

        private void ConfigurarInterface()
        {
            dgvProdutos.RowTemplate.Height = 50;

            dgvProdutos.Columns.Add(new DataGridViewImageColumn
            {
                Name = "colFoto",
                HeaderText = "Foto",
                ImageLayout = DataGridViewImageCellLayout.Zoom,
                FillWeight = 40
            });
            dgvProdutos.Columns.Add("colId", "ID");
            dgvProdutos.Columns.Add("colNome", "Produto");
            dgvProdutos.Columns.Add("colPreco", "Preço");

            dgvProdutos.Columns["colId"]!.FillWeight = 30;
            dgvProdutos.Columns["colNome"]!.FillWeight = 250;
            dgvProdutos.Columns["colPreco"]!.FillWeight = 100;
        }

        // ──────────────────────────────────────────────────────────────────────────────
        // CARREGAMENTO E PESQUISA
        // ──────────────────────────────────────────────────────────────────────────────

        private async Task CarregarProdutosAsync(string filtro = "")
        {
            if (_produtos.Count == 0 || string.IsNullOrEmpty(filtro))
                _produtos = await _produtoService.GetProdutosAsync();

            AtualizarGrid(_produtos, filtro);
        }

        private void AtualizarGrid(List<ProdutoDto> lista, string filtro = "")
        {
            dgvProdutos.Rows.Clear();

            var exibidos = string.IsNullOrWhiteSpace(filtro)
                ? lista
                : lista.Where(p =>
                    p.Nome.Contains(filtro, StringComparison.OrdinalIgnoreCase)).ToList();

            foreach (var p in exibidos)
            {
                int rowIndex = dgvProdutos.Rows.Add(null, p.Id, p.Nome, p.PrecoVenda.ToString("C2"));
                _ = CarregarImagemAsync(rowIndex, p.FotoProduto);
            }
        }

        private async Task CarregarImagemAsync(int rowIndex, string? caminhoRelativo)
        {
            if (string.IsNullOrEmpty(caminhoRelativo)) return;
            try
            {
                var url = $"{ApiClientService.ApiBaseUrl.TrimEnd('/')}/api/imagens/{caminhoRelativo}";
                using var stream = await ApiClientService.Cliente.GetStreamAsync(url);
                var img = System.Drawing.Image.FromStream(stream);

                if (dgvProdutos.Rows.Count > rowIndex)
                    dgvProdutos.Rows[rowIndex].Cells["colFoto"].Value = img;
            }
            catch { /* Ignora erro de carregamento */ }
        }

        // ──────────────────────────────────────────────────────────────────────────────
        // BARRA DE PESQUISA
        // ──────────────────────────────────────────────────────────────────────────────

        private void txtBuscaProduto_TextChanged_1(object sender, EventArgs e)
        {
            AtualizarGrid(_produtos, txtBuscaProduto.Text);
        }

        // ──────────────────────────────────────────────────────────────────────────────
        // BOTÕES
        // ──────────────────────────────────────────────────────────────────────────────

        private void btnNovoProduto_Click(object sender, EventArgs e)
        {
            (this.FindForm() as frmPrincipal)?.Navegar(new ucNovoProduto());
        }

        private void btnEditarProduto_Click_1(object sender, EventArgs e)
        {
            if (dgvProdutos.CurrentRow == null)
            {
                MessageBox.Show("Selecione um registro para editar.",
                    "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int id = Convert.ToInt32(dgvProdutos.CurrentRow.Cells["colId"].Value);
            (this.FindForm() as frmPrincipal)?.Navegar(new ucNovoProduto(id));
        }

        private async void btnExcluirProduto_Click_1(object sender, EventArgs e)
        {
            if (dgvProdutos.CurrentRow == null)
            {
                MessageBox.Show("Selecione um produto para excluir.",
                    "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int id = Convert.ToInt32(dgvProdutos.CurrentRow.Cells["colId"].Value);
            string nome = dgvProdutos.CurrentRow.Cells["colNome"].Value?.ToString() ?? "";

            if (MessageBox.Show(
                    $"Excluir \"{nome}\"? Esta ação não pode ser desfeita.",
                    "Confirmar Exclusão", MessageBoxButtons.YesNo, MessageBoxIcon.Question)
                != DialogResult.Yes) return;

            bool ok = await _produtoService.DeleteProdutoAsync(id);
            if (ok)
            {
                MessageBox.Show("Produto excluído com sucesso!", "Sucesso",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                _produtos.Clear();
                await CarregarProdutosAsync();
            }
        }


    }
}
