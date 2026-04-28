using SenacBuy.UI.Services.Models;
using ValhallaBebidas.UI.Services.Models;

namespace ValhallaBebidas.UI
{
    public partial class ucNovoProduto : UserControl
    {
        private readonly ProdutoApiService _produtoService = new();
        private readonly CategoriaApiService _categoriaService = new();
        private List<CategoriaDto> _categorias = new();
        private string? _caminhoFotoLocal;
        private readonly int? _idEdicao;


        public ucNovoProduto(int? id = null)
        {
            InitializeComponent();
            _idEdicao = id;
            Load += async (s, e) => await CarregarCategoriasAsync();
        }


        private async Task CarregarCategoriasAsync()
        {
            if (_categorias.Count == 0)
                _categorias = await _categoriaService.GetCategoriasAsync();
            cmbCategorias.DataSource = _categorias;
            cmbCategorias.DisplayMember = "Nome";
            cmbCategorias.ValueMember = "Id";

            await CarregarEdicaoAsync();
        }

        private async Task CarregarEdicaoAsync()
        {
            if (!_idEdicao.HasValue)
            {
                lblCadastrarNovoProduto.Text = "Cadastro de Novo Produto";
                return;
            }

            lblCadastrarNovoProduto.Text = "Edição de Produto";
            btnSalvar.Text = "Atualizar";

            var produto = await _produtoService.GetProdutoByIdAsync(_idEdicao.Value);
            if (produto != null)
            {
                txtProduto.Text = produto.Nome;
                txtEan.Text = produto.Ean;
                txtQuantidade.Text = produto.QuantidadeMinimo.ToString();
                txtCusto.Text = produto.PrecoCusto.ToString("0.00", System.Globalization.CultureInfo.InvariantCulture);
                txtVenda.Text = produto.PrecoVenda.ToString("0.00", System.Globalization.CultureInfo.InvariantCulture);
                cmbCategorias.SelectedValue = produto.CategoriaId;

                // Pré-seleciona a categoria
                /*if (produto.CategoriaId.HasValue)
                {
                    int idx = _categorias.FindIndex(c => c.Id == produto.CategoriaId.Value);
                    cmbCategorias.SelectedIndex = idx >= 0 ? idx : -1; // +1 por causa do item "(sem categoria)"
                }*/

                if (!string.IsNullOrEmpty(produto.FotoProduto))
                {
                    try
                    {
                        var url = $"{ApiClientService.ApiBaseUrl.TrimEnd('/')}/api/imagens/{produto.FotoProduto}";
                        picFoto.LoadAsync(url);
                    }
                    catch { }
                }
            }
        }

        private void btnSelecionarImagem_Click(object sender, EventArgs e)
        {
            using var ofd = new OpenFileDialog();
            ofd.Filter = "Imagens (*.jpg;*.jpeg;*.png)|*.jpg;*.jpeg;*.png";
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                _caminhoFotoLocal = ofd.FileName;
                picFoto.ImageLocation = _caminhoFotoLocal;
            }
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            (this.FindForm() as frmPrincipal)?.Navegar(new ucProdutos());
        }

        private async void btnSalvar_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtProduto.Text) ||
                string.IsNullOrWhiteSpace(txtEan.Text) ||
                string.IsNullOrWhiteSpace(txtQuantidade.Text) ||
                string.IsNullOrWhiteSpace(txtCusto.Text) ||
                string.IsNullOrWhiteSpace(txtVenda.Text) ||
                string.IsNullOrWhiteSpace(cmbCategorias.Text))
            {
                MessageBox.Show("Preencha todos os campos do produto.",
                    "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Parse dos preços — aceita vírgula ou ponto como separador decimal
            if (!decimal.TryParse(txtCusto.Text.Trim().Replace(',', '.'), System.Globalization.NumberStyles.Any,
                System.Globalization.CultureInfo.InvariantCulture, out decimal precoCusto)
                || !decimal.TryParse(txtVenda.Text.Trim().Replace(',', '.'), System.Globalization.NumberStyles.Any,
                System.Globalization.CultureInfo.InvariantCulture, out decimal precoVenda))
            {
                MessageBox.Show("Preços inválidos. Use ponto ou vírgula como separador decimal. Ex: 9.99 ou 9,99",
                    "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            btnSalvar.Enabled = false;

            try
            {
                string? caminhoFotoApi = null;
                if (!string.IsNullOrEmpty(_caminhoFotoLocal))
                {
                    btnSalvar.Text = "Enviando foto...";
                    caminhoFotoApi = await _produtoService.UploadFotoAsync(_caminhoFotoLocal);
                    if (caminhoFotoApi == null)
                    {
                        return; // falha no upload já exibiu mensagem
                    }
                }

                btnSalvar.Text = "Salvando...";

                // Resolve CategoriaId: índice 0 = "(sem categoria)" = null
                int? categoriaId = cmbCategorias.SelectedValue as int?;

                if (_idEdicao.HasValue)
                {
                    // MODO EDIÇÃO
                    var atualizado = await _produtoService.UpdateProdutoAsync(
                        id: _idEdicao.Value,
                        nome: txtProduto.Text.Trim(),
                        descricao: txtDescricao.Text.Trim(),
                        precoVenda: decimal.Parse(txtVenda.Text.Trim().Replace(',', '.')),
                        precoCusto: decimal.Parse(txtCusto.Text.Trim().Replace(',', '.')),
                        fotoProduto: caminhoFotoApi,
                        categoriaId: categoriaId,
                        qtdMin: int.Parse(txtQuantidade.Text),
                        ean: txtEan.Text.Trim());

                    if (atualizado)
                    {
                        MessageBox.Show("Produto atualizado com sucesso!",
                            "Edição Realizada", MessageBoxButtons.OK, MessageBoxIcon.Information);

                        var principal = this.FindForm() as frmPrincipal;
                        principal?.Navegar(new ucProdutos());
                    }
                }
                else
                {
                    // MODO CRIAÇÃO
                    var criado = await _produtoService.CreateProdutoAsync(
                        nome: txtProduto.Text.Trim(),
                        descricao: txtDescricao.Text.Trim(),
                        precoVenda: decimal.Parse(txtVenda.Text.Trim().Replace(',', '.')),
                        precoCusto: decimal.Parse(txtCusto.Text.Trim().Replace(',', '.')),
                        fotoProduto: caminhoFotoApi,
                        categoriaId: categoriaId,
                        qtdMin: int.Parse(txtQuantidade.Text),
                        ean: txtEan.Text.Trim());

                    if (criado != null)
                    {
                        MessageBox.Show($"Produto \"{criado.Nome}\" cadastrado com sucesso!",
                            "Cadastro Realizado", MessageBoxButtons.OK, MessageBoxIcon.Information);

                        var principal = this.FindForm() as frmPrincipal;
                        principal?.Navegar(new ucProdutos());
                    }
                }
            }
            finally
            {
                btnSalvar.Enabled = true;
                btnSalvar.Text = "Salvar";
            }
        }

    }
}
