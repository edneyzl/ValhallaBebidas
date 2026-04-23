using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using ValhallaBebidas.UI.Services;
using ValhallaBebidas.UI.Services.Models;

namespace ValhallaBebidas.UI
{
    public partial class ucMovimentacoes : UserControl
    {
        private readonly MovimentacaoApiService _service = new();
        private List<MovimentacaoDto> _movimentacoes = new();

        public ucMovimentacoes()
        {
            InitializeComponent();
            ConfigurarInterface();
            Load += async (s, e) => await CarregarMovimentacoesAsync();
        }


        //configuração do grid
        private void ConfigurarInterface()
        {
            dgvMovimentacoes.RowTemplate.Height = 35;

            dgvMovimentacoes.Columns.Add("colId", "ID");
            dgvMovimentacoes.Columns.Add("colProduto", "Produto");
            dgvMovimentacoes.Columns.Add("colQuantidade", "Qtd");
            dgvMovimentacoes.Columns.Add("colDirecao", "Tipo");
            dgvMovimentacoes.Columns.Add("colMotivo", "Motivo");
            dgvMovimentacoes.Columns.Add("colData", "Data");
            dgvMovimentacoes.Columns.Add("colImpacto", "Impacto");

            dgvMovimentacoes.Columns["colId"]!.FillWeight = 30;
            dgvMovimentacoes.Columns["colProduto"]!.FillWeight = 200;
        }

        //carregar dados 
        private async Task CarregarMovimentacoesAsync(string filtro = "")
        {
            if (_movimentacoes.Count == 0 || string.IsNullOrEmpty(filtro))
                _movimentacoes = await _service.GetMovimentacoesAsync();

            AtualizarGrid(_movimentacoes, filtro);
        }

        //atualizar grid
        private void AtualizarGrid(List<MovimentacaoDto> lista, string filtro = "")
        {
            dgvMovimentacoes.Rows.Clear();

            var exibidos = string.IsNullOrWhiteSpace(filtro)
                ? lista
                : lista.Where(m =>
                    m.NomeProduto.Contains(filtro, StringComparison.OrdinalIgnoreCase) ||
                    m.Motivo.Contains(filtro, StringComparison.OrdinalIgnoreCase)
                ).ToList();

            foreach (var m in exibidos)
            {
                int rowIndex = dgvMovimentacoes.Rows.Add(
                    m.Id,
                    m.NomeProduto,
                    m.Quantidade,
                    m.Direcao.ToString(),
                    m.Motivo,
                    m.Data.ToLocalTime().ToString("dd/MM/yyyy HH:mm"),
                    m.Impacto
                );

                // 🎨 cor dinâmica (entrada verde, saída vermelha)
                if (m.Direcao.ToString() == "Entrada")
                    dgvMovimentacoes.Rows[rowIndex].Cells["colImpacto"].Style.ForeColor = Color.Green;
                else
                    dgvMovimentacoes.Rows[rowIndex].Cells["colImpacto"].Style.ForeColor = Color.Red;
            }
        }

        private void txtBuscaMovi_TextChanged(object sender, EventArgs e)
        {
            AtualizarGrid(_movimentacoes, txtBuscaMovi.Text);
        }
    }
}