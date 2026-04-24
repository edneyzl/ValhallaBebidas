using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using SenacBuy.UI.Services.Models;
using ValhallaBebidas.UI.Services.Models;

namespace ValhallaBebidas.UI
{
    public partial class ucNovoFuncionario : UserControl
    {
        private readonly FuncionarioApiService _usuarioService = new();
        private string? _caminhoFotoLocal;
        private readonly int? _idEdicao;

        public ucNovoFuncionario(int? id = null)
        {
            InitializeComponent();
            _idEdicao = id;
            this.Load += async (s, e) => await CarregarEdicaoAsync();
        }

        private async Task CarregarEdicaoAsync()
        {
            if (!_idEdicao.HasValue)
            {
                lblTitulo.Text = "👥  Novo ";
                return;
            }

            lblTitulo.Text = "👥  Editar";
            btnCadastrar.Text = "Atualizar Usuário";
            txtSenha.PlaceholderText = "Deixe em branco para não alterar"; // Senha geralmente não volta

            var usuario = await _usuarioService.GetUsuarioByIdAsync(_idEdicao.Value);
            if (usuario != null)
            {
                txtNome.Text = usuario.NomeCompleto;
                txtEmail.Text = usuario.Email;
            }
        }

        private async void btnCadastrar_Click(object sender, EventArgs e)
        {
            try
            {
                btnCadastrar.Enabled = false;

                string nome = txtNome.Text.Trim();
                string email = txtEmail.Text.Trim();
                string senha = txtSenha.Text;
                DateTime dataNasc = DateTime.Parse(txtDataNascimento.Text);
                string cpf = txtCpf.Text.Trim();
                string telefone = txtTelefone.Text.Trim();

                // Se não é edição, senha é obrigatória
                if (!_idEdicao.HasValue && string.IsNullOrEmpty(senha))
                {
                    MessageBox.Show("Preencha todos os campos obrigatórios (*).",
                        "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (!_idEdicao.HasValue && senha.Length < 6)
                {
                    MessageBox.Show("A senha deve ter pelo menos 6 caracteres.",
                        "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                string? caminhoFotoApi = null;
                if (!string.IsNullOrEmpty(_caminhoFotoLocal))
                {
                    btnCadastrar.Text = "Enviando foto...";
                    caminhoFotoApi = await _usuarioService.UploadFotoAsync(_caminhoFotoLocal);
                    if (caminhoFotoApi == null)
                    {
                        return; // falha no upload já exibiu mensagem
                    }
                }

                if (_idEdicao.HasValue)
                {
                    btnCadastrar.Text = "Atualizando Usuário...";
                    var dto = new AtualizarFuncionarioDto
                    {
                        Id = _idEdicao.Value,
                        NomeCompleto = nome,
                        Email = email,
                        DataNascimento = dataNasc,
                        Cpf = cpf,
                        Telefone = telefone,
                    };

                    var ok = await _usuarioService.AtualizarUsuarioAsync(dto);
                    if (ok)
                    {
                        MessageBox.Show("Usuário atualizado com sucesso!", "Sucesso",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
                        VoltarParaLista();
                    }
                }
                else
                {
                    btnCadastrar.Text = "Salvando Usuário...";

                    // Substitua a chamada incorreta de "_usuarioService.CadastrarFunciorioAsync" por "_usuarioService.CadastrarFuncionarioAsync"
                    // e ajuste os parâmetros conforme a assinatura correta do método.

                    var novoDto = await _usuarioService.CadastrarFuncionarioAsync(
                        nome,
                        dataNasc, // dataNasc (preencha conforme necessário)
                        "", // cpf (preencha conforme necessário)
                        "", // telefone (preencha conforme necessário)
                        email,
                        senha,
                        caminhoFotoApi
                    );
                    if (novoDto != null)
                    {
                        MessageBox.Show("Usuário cadastrado com sucesso!", "Sucesso",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
                        VoltarParaLista();
                    }
                }
            }
            finally
            {
                btnCadastrar.Enabled = true;
                btnCadastrar.Text = "Salvar Funcionário";
            }
        }

        private void VoltarParaLista()
        {
            var principal = this.FindForm() as frmPrincipal;
            principal?.Navegar(new ucFuncionarios());
        }

        private void txtTelefone_TextChanged(object sender, EventArgs e)
        {

        }
    }
}

