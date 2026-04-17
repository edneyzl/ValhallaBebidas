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
    public partial class frmInicio : Form
    {
        private readonly FuncionarioApiService _funcionarioService = new();
        private string? _caminhoFotoLocal;
        private readonly int? _idUsu;

        public frmInicio(int? id = null)
        {
            InitializeComponent();
            _idUsu = id;
            this.Load += async (s, e) => await CarregarUsuarioAsync();
        }


        private async Task CarregarUsuarioAsync()
        {



            var funcionario = await _funcionarioService.GetUsuarioByIdAsync(_idUsu.Value);
            if (funcionario != null)
            {
                lblNome.Text = funcionario.Nome;

                if (!string.IsNullOrEmpty(funcionario.FotoPerfil))
                {
                    try
                    {
                        var url = $"{ApiClientService.ApiBaseUrl.TrimEnd('/')}/api/imagens/{funcionario.FotoPerfil}";
                        picFoto.LoadAsync(url);
                    }
                    catch { }
                }
            }
        }



        private void btnIniciar_Click(object sender, EventArgs e)
        {
            var principal = new frmPrincipal();
            principal.Show();
            this.Close();
        }

        private void btnSair_Click(object sender, EventArgs e)
        {
            var login = new FrmLogin();
            login.Show();
            this.Close();
        }


    }
}
