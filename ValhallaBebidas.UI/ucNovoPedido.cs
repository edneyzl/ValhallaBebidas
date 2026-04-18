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
    public partial class ucNovoPedido : UserControl
    {

        private readonly ClienteApiService _clienteService = new();
        private readonly ProdutoApiService _produtoService = new();
        private readonly PedidoApiService _pedidoService = new();

        // Dados carregados da API
        private List<ClienteDto> _clientes = new();
        private List<ProdutoDto> _produtos = new();

        // Itens que serão enviados para a API ao finalizar
        private readonly List<CriarItemPedidoDto> _itensPedido = new();
        private int _sequencial = 1;

        // Se preenchido → modo edição; null → modo criação
        private PedidoDto? _pedidoExistente;
        private readonly int? _idEdicao;

        // ──────────────────────────────────────────────────────────────────────────────
        // CONSTRUTORES
        // ──────────────────────────────────────────────────────────────────────────────

        public ucNovoPedido(int? id = null)
        {
            InitializeComponent();
            _idEdicao = id;
            ConfigurarInterface();
            Load += async (s, e) => await CarregarDadosDaApiAsync();
        }

        // ──────────────────────────────────────────────────────────────────────────────
        // CONFIGURAÇÃO
        // ──────────────────────────────────────────────────────────────────────────────

        private void ConfigurarInterface()
        {
            dgvItens.Columns.Clear();
            dgvItens.Columns.Add("colSeq", "#");
            dgvItens.Columns.Add("colProduto", "Produto");
            dgvItens.Columns.Add("colQtd", "Qtd");
            dgvItens.Columns.Add("colPreco", "Preço Unit.");

            dgvItens.Columns["colSeq"]!.FillWeight = 20;
            dgvItens.Columns["colQtd"]!.FillWeight = 30;
            dgvItens.Columns["colPreco"]!.FillWeight = 80;
        }

        // ──────────────────────────────────────────────────────────────────────────────
        // CARREGAMENTO INICIAL
        // ──────────────────────────────────────────────────────────────────────────────

        private async Task CarregarDadosDaApiAsync()
        {
            _clientes = await _clienteService.GetClientesAsync();
            _produtos = await _produtoService.GetProdutosAsync();

            cmbCliente.Items.Clear();
            cmbProduto.Items.Clear();

            foreach (var c in _clientes)
                cmbCliente.Items.Add(c.Nome);

            foreach (var p in _produtos)
                cmbProduto.Items.Add($"{p.Nome} — {p.PrecoVenda:C2}");

            if (_idEdicao.HasValue)
            {
                // Modo edição: busca o pedido na API e pré-popula os campos
                _pedidoExistente = await _pedidoService.GetPedidoByIdAsync(_idEdicao.Value);
                if (_pedidoExistente != null)
                {
                    PreencherParaEdicao();
                }
            }
            else
            {
                // Modo criação: seleciona o primeiro cliente por padrão
                if (cmbCliente.Items.Count > 0) cmbCliente.SelectedIndex = 0;
            }
        }
        /// <summary>Pré-popula o formulário com os dados do pedido existente</summary>
        private void PreencherParaEdicao()
        {
            if (_pedidoExistente == null) return;

            // Seleciona o cliente correto no ComboBox
            int indexCliente = _clientes.FindIndex(c => c.Id == _pedidoExistente.ClienteId);
            if (indexCliente >= 0) cmbCliente.SelectedIndex = indexCliente;

            // Pré-seleciona o status do pedido
            int indexStatus = cmbStatus.Items.IndexOf(_pedidoExistente.Status);
            if (indexStatus >= 0) cmbStatus.SelectedIndex = indexStatus;

            // Popula a lista de itens com os dados existentes
            dgvItens.Rows.Clear();
            _itensPedido.Clear();
            _sequencial = 1;

            foreach (var item in _pedidoExistente.Itens)
            {
                _itensPedido.Add(new CriarItemPedidoDto
                {
                    ProdutoId = item.ProdutoId,
                    Quantidade = item.Quantidade
                });
                dgvItens.Rows.Add(_sequencial++, item.NomeProduto, item.Quantidade, item.PrecoUnitario.ToString("C2"));
            }

            // Altera o título do botão para indicar modo edição
            btnFinalizar.Text = "💾 Salvar Pedido";
        }

        private void btnAdicionarItem_Click(object sender, EventArgs e)
        {
            if (cmbProduto.SelectedIndex < 0)
            {
                MessageBox.Show("Selecione um produto.", "Atenção",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var produto = _produtos[cmbProduto.SelectedIndex];
            int quantidade = (int)numQtd.Value;

            _itensPedido.Add(new CriarItemPedidoDto
            {
                ProdutoId = produto.Id,
                Quantidade = quantidade
            });

            dgvItens.Rows.Add(_sequencial++, produto.Nome, quantidade, produto.PrecoVenda.ToString("C2"));
        }

        private async void btnFinalizar_Click(object sender, EventArgs e)
        {
            if (cmbCliente.SelectedIndex < 0)
            {
                MessageBox.Show("Selecione um cliente.", "Atenção",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (_itensPedido.Count == 0)
            {
                MessageBox.Show("Adicione pelo menos um item ao pedido.", "Atenção",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var cliente = _clientes[cmbCliente.SelectedIndex];

            btnFinalizar.Enabled = false;
            btnFinalizar.Text = "Salvando...";

            try
            {
                if (_pedidoExistente == null)
                {
                    // MODO CRIAÇÃO: POST api/pedido
                    var pedido = await _pedidoService.CreatePedidoAsync(
                        clienteId: cliente.Id,
                        itens: _itensPedido,
                        status: cmbStatus.SelectedItem?.ToString() ?? "Pendente");

                    if (pedido != null)
                    {
                        MessageBox.Show(
                            $"Pedido #{pedido.Id} criado com sucesso!\nTotal: {pedido.Total:C2}",
                            "Pedido Criado", MessageBoxButtons.OK, MessageBoxIcon.Information);

                        Navegar(new ucPedidos());
                    }
                }
                else
                {
                    // MODO EDIÇÃO: PUT api/pedido/{id}
                    var pedido = await _pedidoService.UpdatePedidoAsync(
                        id: _pedidoExistente.Id,
                        clienteId: cliente.Id,
                        itens: _itensPedido,
                        status: cmbStatus.SelectedItem?.ToString() ?? "Pendente");

                    if (pedido != null)
                    {
                        MessageBox.Show(
                            $"Pedido #{pedido.Id} atualizado com sucesso!\nNovo Total: {pedido.Total:C2}",
                            "Pedido Atualizado", MessageBoxButtons.OK, MessageBoxIcon.Information);

                        Navegar(new ucPedidos());
                    }
                }
            }
            finally
            {
                btnFinalizar.Enabled = true;
                btnFinalizar.Text = _pedidoExistente == null ? "Finalizar" : "💾 Salvar Pedido";
            }
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            Navegar(new ucPedidos());
        }

        private void Navegar(UserControl uc)
        {
            var principal = this.FindForm() as frmPrincipal;
            principal?.Navegar(uc);
        }
    }
}
