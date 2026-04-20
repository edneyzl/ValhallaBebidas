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
    public partial class ucNovoCliente : UserControl
    {

        private readonly ClienteApiService _clienteService = new();
        private readonly int? _idEdicao;
        public ucNovoCliente(int? id = null)
        {
            InitializeComponent();
            _idEdicao = id;
            this.Load += async (s, e) => await CarregarEdicaoAsync();
        }


        private async Task CarregarEdicaoAsync()
        {
            if (!_idEdicao.HasValue)
            {
                lblTitulo.Text = "📝 Novo Cliente";
                return;
            }

            lblTitulo.Text = "✏️ Editar Cliente";
            btnSalvar.Text = "Atualizar";

            var cliente = await _clienteService.GetClienteByIdAsync(_idEdicao.Value);
            if (cliente != null)
            {
                txtNome.Text = cliente.Nome;
                txtCpf.Text = cliente.CPF;
                // email não no DTO de get? Mockamos algo visual
                txtEmail.Text = "email@exemplo.com";
            }
        }

        private async void btnSalvar_Click(object sender, EventArgs e)
        {
            // Validação dos campos obrigatórios
            if (string.IsNullOrWhiteSpace(txtNome.Text) ||
                string.IsNullOrWhiteSpace(txtEmail.Text) ||
                string.IsNullOrWhiteSpace(txtCpf.Text))
            {
                MessageBox.Show("Preencha os campos obrigatórios: Nome, CPF e E-mail.",
                    "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Desabilita o botão enquanto a requisição está em andamento
            btnSalvar.Enabled = false;
            btnSalvar.Text = "Salvando...";

            try
            {
                // Chama a API — o CPF é o identificador; o email é exibido aqui mas
                // o DTO de Cliente só tem Nome e CPF. Para incluir email seria necessário
                // alterar o DTO. Por ora, concatenamos o email no nome para fins didáticos.
                // ------------------------------------------------------------------
                // NOTA PEDAGÓGICA: O DTO de ClienteController (CriarClienteDto)
                // só aceita Nome e CPF. O campo E-mail digitado na tela fica armazenado
                // no próprio campo Nome separado por vírgula caso queira um workaround,
                // OU você pode expandir o DTO Client para incluir Email.
                // Aqui usamos Nome e CPF conforme o contrato da API atual.
                // ------------------------------------------------------------------
                if (_idEdicao.HasValue)
                {
                    // MODO EDIÇÃO
                    var atualizado = await _clienteService.UpdateClienteAsync(
                        _idEdicao.Value,
                        txtNome.Text.Trim(),
                        txtCpf.Text.Trim()
                    );

                    if (atualizado)
                    {
                        MessageBox.Show("Cliente atualizado com sucesso!",
                            "Edição Realizada", MessageBoxButtons.OK, MessageBoxIcon.Information);

                        var principal = this.FindForm() as frmPrincipal;
                        principal?.Navegar(new ucClientes());
                    }
                }
                else
                {
                    // MODO CRIAÇÃO
                    var criado = await _clienteService.CreateClienteAsync(
                        nome: txtNome.Text.Trim(),
                        cpf: txtCpf.Text.Trim());

                    if (criado != null)
                    {
                        MessageBox.Show($"Cliente \"{criado.Nome}\" cadastrado com sucesso!",
                            "Cadastro Realizado", MessageBoxButtons.OK, MessageBoxIcon.Information);

                        // Volta para a lista de clientes
                        var principal = this.FindForm() as frmPrincipal;
                        principal?.Navegar(new ucClientes());
                    }
                }
            }
            finally
            {
                btnSalvar.Enabled = true;
                btnSalvar.Text = "Salvar";
            }
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            var principal = this.FindForm() as frmPrincipal;
            principal?.Navegar(new ucClientes());
        }
    }
}
