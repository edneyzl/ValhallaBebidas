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

        private readonly string _nomeUsuario;

        public frmInicio(string nomeUsuario = "Usuário", int? id = null)
        {
            InitializeComponent();
            _nomeUsuario = nomeUsuario;
            lblNome.Text = $"Olá, {nomeUsuario}";
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
