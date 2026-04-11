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
    }
}
