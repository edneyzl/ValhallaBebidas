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
        FuncionarioDto func;
        public frmInicio(FuncionarioDto funcionario)
        {
            InitializeComponent();
            lblNome.Text = $"Olá, {funcionario.NomeCompleto}";
            func = funcionario;
            CentralizarLabel();
        }

        private void CentralizarLabel()
        {
            lblNome.Left = (panelBack2.Width - lblNome.Width) / 2;
        }


        private void btnIniciar_Click(object sender, EventArgs e)
        {
            var principal = new frmPrincipal(func);
            principal.Show();
            this.Close();
        }

        private void btnSair_Click(object sender, EventArgs e)
        {
            var login = new FrmLogin();
            login.Show();
            this.Close();
        }

        private void frmInicio_Load(object sender, EventArgs e)
        {
            CentralizarLabel();
        }
    }
}
