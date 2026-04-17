using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace ValhallaBebidas.UI
{
    public partial class frmPrincipal : Form
    {

        private readonly string _nomeUsuario;


        public frmPrincipal(string nomeUsuario = "Usuário", string? caminhoFoto = null)
        {
            InitializeComponent();
            _nomeUsuario = nomeUsuario;
            lblUsuario.Text = $"👤  {nomeUsuario}";

            // Carrega o Dashboard como tela inicial
            LoadUserControl(new ucDashboard());
        }

        private void LoadUserControl(UserControl control)
        {
            panelContainer.Controls.Clear();
            control.Dock = DockStyle.Fill;
            panelContainer.Controls.Add(control);
        }

        private void btnDashboard_Click(object sender, EventArgs e)
        {
            SetActiveButton(btnDashboard);
            LoadUserControl(new ucDashboard());
        }

        private void btnClientes_Click(object sender, EventArgs e)
        {
            SetActiveButton(btnDashboard);
            LoadUserControl(new ucClientes());
        }

        private void btnProdutos_Click(object sender, EventArgs e)
        {
            SetActiveButton(btnDashboard);
            LoadUserControl(new ucProdutos());
        }

        private void btnPedidos_Click(object sender, EventArgs e)
        {
            SetActiveButton(btnDashboard);
            LoadUserControl(new ucPedidos());
        }

        private void btnUsuarios_Click(object sender, EventArgs e)
        {
            SetActiveButton(btnDashboard);
            LoadUserControl(new ucFuncionarios());
        }

        private void btnMovimentacoes_Click(object sender, EventArgs e)
        {
            SetActiveButton(btnDashboard);
            LoadUserControl(new ucMovimentacoes());
        }

        private void btnSair_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Deseja realmente sair do sistema?", "Confirmar Saída",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                Application.Exit();
        }

        private void SetActiveButton(Guna.UI2.WinForms.Guna2Button activeBtn)
        {
            var botoes = new[] { btnDashboard, btnClientes, btnProdutos, btnPedidos, btnUsuarios };
            foreach (var btn in botoes)
            {
                btn.FillColor = Color.FromArgb(22, 22, 22);
                btn.ForeColor = Color.White;
            }
            activeBtn.FillColor = Color.FromArgb(214, 189, 119);
            activeBtn.ForeColor = Color.Black;
        }

        // Permite que UserControls filhos naveguem de volta
        public void Navegar(UserControl control) => LoadUserControl(control);

        
    }
}

