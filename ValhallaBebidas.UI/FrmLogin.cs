using ValhallaBebidas.UI.Services.Models;

namespace ValhallaBebidas.UI
{
    public partial class FrmLogin : Form
    {

        private readonly FuncionarioApiService _funcionarioApiService = new();

        public FrmLogin()
        {
            InitializeComponent();
        }


        // ──────────────────────────────────────────────────────────────────────────────
        // CADASTRAR-SE
        // ──────────────────────────────────────────────────────────────────────────────

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            frmCadastroFuncionario form = new();//chama a tela de cadastro
            form.ShowDialog();//impede que clicar em outra tela enquanto a de cadastro estiver aberta
        }

        // ──────────────────────────────────────────────────────────────────────────────
        // FECHAR APLICAÇÃO
        // ──────────────────────────────────────────────────────────────────────────────

        private void guna2CircleButton1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        // ──────────────────────────────────────────────────────────────────────────────
        // ENTRAR
        // ──────────────────────────────────────────────────────────────────────────────

        private async void btnLogar_Click(object sender, EventArgs e)
        {
            // Validação: campos obrigatórios
            if (string.IsNullOrWhiteSpace(txtEmail.Text) ||
                string.IsNullOrWhiteSpace(txtSenha.Text))
            {
                MessageBox.Show("Preencha email e senha.", "Atenção",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            btnLogar.Enabled = false;
            btnLogar.Text = "Logando...";

            try
            {
                // Chama a API para autenticar — POST api/usuario/login
                var resultado = await _funcionarioApiService.LoginAsync(
                    email: txtEmail.Text.Trim(),
                    senha: txtSenha.Text);

                if (resultado == null)
                {
                    // Mensagem de erro já exibida pelo UsuarioApiService
                    return;
                }

                if (resultado.Sucesso)
                {
                    // Login bem-sucedido — abre o formulário principal
                    var inicial = new frmInicio(resultado.NomeCompleto, resultado.Id);
                    inicial.Show();
                    this.Hide();
                }
                else
                {
                    MessageBox.Show(
                        $"Acesso negado.\n{resultado.Mensagem}",
                        "Autenticação Falhou",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
                }
            }
            finally
            {
                btnLogar.Enabled = true;
                btnLogar.Text = "Logar";
            }
        }
    }
}
