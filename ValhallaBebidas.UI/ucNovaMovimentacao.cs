using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using ValhallaBebidas.Domain.Enums;
using ValhallaBebidas.UI.Services;
using ValhallaBebidas.UI.Services.Models;

namespace ValhallaBebidas.UI
{
    public partial class ucNovaMovimentacao : UserControl
    {
        private readonly MovimentacaoApiService _service = new();
        private readonly ProdutoApiService _produtoService = new();

        private List<ProdutoDto> _produtos = new();

        public ucNovaMovimentacao()
        {
            InitializeComponent();
            Load += async (s, e) => await CarregarDadosAsync();
        }

        //---------------------------------------
        //CARREGAR PRODUTOS NO COMBOBOX
        //---------------------------------------
        private async Task CarregarDadosAsync()
        {
            _produtos = await _produtoService.GetProdutosAsync();

            cmbProduto.Items.Clear();

            foreach (var p in _produtos)
                cmbProduto.Items.Add(p.Nome);

            if (cmbProduto.Items.Count > 0)
                cmbProduto.SelectedIndex = 0;

            // Tipo de movimentação usando ENUM
            cmbTipo.DataSource = Enum.GetValues(typeof(DirecaoMovimentacao));
        }

        private void cmbTipo_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbTipo.SelectedItem == null) return;

            var tipo = (DirecaoMovimentacao)cmbTipo.SelectedItem;

            if (tipo == DirecaoMovimentacao.Entrada)
                txtMotivo.Text = "Reposição de estoque";
            else
                txtMotivo.Text = "Venda / Saída";
        }

        private async void btnSalvarMovimentacao_Click(object sender, EventArgs e)
        {
            //---------------------------------------
            // 1. VALIDAÇÃO
            //---------------------------------------
            if (cmbProduto.SelectedIndex < 0 ||
                cmbTipo.SelectedItem == null ||
                string.IsNullOrWhiteSpace(txtInformeQuantidade.Text) ||
                string.IsNullOrWhiteSpace(txtMotivo.Text))
            {
                MessageBox.Show("Preencha todos os campos obrigatórios.");
                return;
            }

            //---------------------------------------
            // 2. VALIDAR QUANTIDADE
            //---------------------------------------
            if (!int.TryParse(txtInformeQuantidade.Text, out int quantidade) || quantidade <= 0)
            {
                MessageBox.Show("Quantidade inválida.");
                return;
            }

            //---------------------------------------
            // 3. PEGAR PRODUTO SELECIONADO
            //---------------------------------------
            var produtoSelecionado = _produtos[cmbProduto.SelectedIndex];

            //---------------------------------------
            // 4. CRIAR DTO
            //---------------------------------------
            var dto = new CriarMovimentacaoDto
            {
                ProdutoId = produtoSelecionado.Id,
                Quantidade = quantidade,
                Motivo = txtMotivo.Text.Trim(),
                Direcao = (DirecaoMovimentacao)cmbTipo.SelectedItem
            };

            //---------------------------------------
            // 5. CHAMAR API
            //---------------------------------------
            MovimentacaoDto? resultado;

            if (dto.Direcao == DirecaoMovimentacao.Entrada)
                resultado = await _service.RegistrarEntradaAsync(dto);
            else
                resultado = await _service.RegistrarSaidaAsync(dto);

            //---------------------------------------
            // 6. RESULTADO
            //---------------------------------------
            if (resultado != null)
            {
                MessageBox.Show("Movimentação registrada com sucesso!");

                var principal = this.FindForm() as frmPrincipal;
                principal?.Navegar(new ucMovimentacoes());
            }
        }

        private void btnVoltar_Click(object sender, EventArgs e)
        {
            var principal = this.FindForm() as frmPrincipal;
            principal?.Navegar(new ucMovimentacoes());
        }
    }
}

