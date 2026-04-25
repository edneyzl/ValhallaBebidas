using ValhallaBebidas.UI.Services.Models;
using System.Text.Json;
using ValhallaBebidas.UI.DTO;

namespace ValhallaBebidas.UI
{
    public partial class frmCadastroFuncionario : Form
    {
        public frmCadastroFuncionario()
        {
            InitializeComponent();
        }

        private readonly FuncionarioApiService _funcionarioService = new();

        private async void txtCep_Leave(object sender, EventArgs e)
        {
            string cep = txtCep.Text.Replace("-", "").Trim();

            // validação básica
            if (cep.Length != 8)
            {
                MessageBox.Show("CEP inválido!");
                return;
            }

            string url = $"https://viacep.com.br/ws/{cep}/json/";

            using (HttpClient client = new HttpClient())
            {
                try
                {
                    var response = await client.GetStringAsync(url);

                    var dados = JsonSerializer.Deserialize<CepModel>(response);

                    // verifica se veio erro
                    if (response.Contains("\"erro\""))
                    {
                        MessageBox.Show("CEP não encontrado!");
                        return;
                    }

                    txtEndereco.Text = dados.logradouro;
                    txtBairro.Text = dados.bairro;
                    txtCidade.Text = dados.localidade;
                    txtEstado.Text = dados.uf;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Erro: " + ex.Message);
                }
            }
        }

        private void btnFechar_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        

        private void btnLimparCampos_Click(object sender, EventArgs e)
        {
            txtNome.Text = string.Empty;
            txtDataNascimento.Text = string.Empty;
            txtCpf.Text = string.Empty;
            txtEmail.Text = string.Empty;
            txtTelefone.Text = string.Empty;
            txtSenha.Text = string.Empty;
            txtConfirmarSenha.Text = string.Empty;
            txtCep.Text = string.Empty;
            txtEndereco.Text = string.Empty;
            txtNumero.Text = string.Empty;
            txtComplemento.Text = string.Empty;
            txtBairro.Text = string.Empty;
            txtCidade.Text = string.Empty;
            txtEstado.Text = string.Empty;
        
        }

        private async void btnSalvar_Click(object sender, EventArgs e)
        {
            // Validação básica dos campos obrigatórios
            if (string.IsNullOrWhiteSpace(txtNome.Text) ||
                string.IsNullOrWhiteSpace(txtDataNascimento.Text) ||
                string.IsNullOrWhiteSpace(txtCpf.Text) ||
                string.IsNullOrWhiteSpace(txtEmail.Text) ||
                string.IsNullOrWhiteSpace(txtTelefone.Text) ||
                string.IsNullOrWhiteSpace(txtSenha.Text) ||
                string.IsNullOrWhiteSpace(txtConfirmarSenha.Text) ||
                string.IsNullOrWhiteSpace(txtCep.Text) ||
                string.IsNullOrWhiteSpace(txtEndereco.Text) ||
                string.IsNullOrWhiteSpace(txtNumero.Text) ||
                string.IsNullOrWhiteSpace(txtComplemento.Text) ||
                string.IsNullOrWhiteSpace(txtBairro.Text) ||
                string.IsNullOrWhiteSpace(txtCidade.Text) ||
                string.IsNullOrWhiteSpace(txtEstado.Text))
            {
                MessageBox.Show("Preencha todos os campos para continuar.",
                    "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (txtSenha.Text.Length < 4 || txtConfirmarSenha.Text.Length < 4)
            {
                MessageBox.Show("A senha deve ter pelo menos 4 caracteres.",
                    "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }


            if (txtSenha.Text != txtConfirmarSenha.Text)
            {
                MessageBox.Show("As senhas digitadas não conferem.",
                    "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            btnCadastrar.Enabled = false;
            btnCadastrar.Text = "Cadastrando...";
            var dataNascimento = DateTime.Parse(txtDataNascimento.Text);
            try
            {
                var funcionario = await _funcionarioService.CadastrarFuncionarioAsync(
                    nome: txtNome.Text.Trim(),
                    email: txtEmail.Text.Trim(),
                    dataNasc: dataNascimento,
                    cpf: txtCpf.Text.Trim(),
                    telefone: txtTelefone.Text.Trim(),
                    senha: txtSenha.Text);

                if (funcionario != null)
                {
                    MessageBox.Show(
                        $"Funcionário \"{funcionario.NomeCompleto}\" cadastrado com sucesso!\nFaça login para continuar.",
                        "Cadastro Realizado", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    // Fecha o formulário de cadastro e volta para o login
                    this.DialogResult = DialogResult.OK;
                    this.Close();
                }
            }
            finally
            {
                btnCadastrar.Enabled = true;
                btnCadastrar.Text = "Cadastrar";
            }
        }
    }
}
