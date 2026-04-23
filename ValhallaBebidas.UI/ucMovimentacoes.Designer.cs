namespace ValhallaBebidas.UI
{
    partial class ucMovimentacoes
    {
        /// <summary> 
        /// Variável de designer necessária.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Limpar os recursos que estão sendo usados.
        /// </summary>
        /// <param name="disposing">true se for necessário descartar os recursos gerenciados; caso contrário, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Código gerado pelo Designer de Componentes

        /// <summary> 
        /// Método necessário para suporte ao Designer - não modifique 
        /// o conteúdo deste método com o editor de código.
        /// </summary>
        private void InitializeComponent()
        {
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges7 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges8 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges1 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges2 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges3 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges4 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges5 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges6 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges9 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges10 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            DataGridViewCellStyle dataGridViewCellStyle1 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle2 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle3 = new DataGridViewCellStyle();
            panelHeader = new Guna.UI2.WinForms.Guna2Panel();
            btnNovaMovi = new Guna.UI2.WinForms.Guna2Button();
            btnEditarMovi = new Guna.UI2.WinForms.Guna2Button();
            txtBuscaMovi = new Guna.UI2.WinForms.Guna2TextBox();
            lblSubtitulo = new Label();
            lblTitulo = new Label();
            panelConteudo = new Guna.UI2.WinForms.Guna2Panel();
            dgvMovimentacoes = new Guna.UI2.WinForms.Guna2DataGridView();
            panelHeader.SuspendLayout();
            panelConteudo.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvMovimentacoes).BeginInit();
            SuspendLayout();
            // 
            // panelHeader
            // 
            panelHeader.BackColor = Color.FromArgb(33, 33, 33);
            panelHeader.Controls.Add(btnNovaMovi);
            panelHeader.Controls.Add(btnEditarMovi);
            panelHeader.Controls.Add(txtBuscaMovi);
            panelHeader.Controls.Add(lblSubtitulo);
            panelHeader.Controls.Add(lblTitulo);
            panelHeader.CustomizableEdges = customizableEdges7;
            panelHeader.ForeColor = SystemColors.ControlText;
            panelHeader.Location = new Point(0, 0);
            panelHeader.Name = "panelHeader";
            panelHeader.ShadowDecoration.CustomizableEdges = customizableEdges8;
            panelHeader.Size = new Size(969, 80);
            panelHeader.TabIndex = 1;
            // 
            // btnNovaMovi
            // 
            btnNovaMovi.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnNovaMovi.BorderRadius = 10;
            btnNovaMovi.Cursor = Cursors.Hand;
            btnNovaMovi.CustomizableEdges = customizableEdges1;
            btnNovaMovi.FillColor = Color.FromArgb(214, 189, 119);
            btnNovaMovi.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            btnNovaMovi.ForeColor = Color.Black;
            btnNovaMovi.Location = new Point(596, 19);
            btnNovaMovi.Name = "btnNovaMovi";
            btnNovaMovi.ShadowDecoration.CustomizableEdges = customizableEdges2;
            btnNovaMovi.Size = new Size(198, 40);
            btnNovaMovi.TabIndex = 9;
            btnNovaMovi.Text = "+ Nova Movimentação";
            // 
            // btnEditarMovi
            // 
            btnEditarMovi.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnEditarMovi.BorderRadius = 10;
            btnEditarMovi.Cursor = Cursors.Hand;
            btnEditarMovi.CustomizableEdges = customizableEdges3;
            btnEditarMovi.FillColor = Color.FromArgb(64, 64, 64);
            btnEditarMovi.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold);
            btnEditarMovi.ForeColor = Color.White;
            btnEditarMovi.Location = new Point(814, 19);
            btnEditarMovi.Name = "btnEditarMovi";
            btnEditarMovi.ShadowDecoration.CustomizableEdges = customizableEdges4;
            btnEditarMovi.Size = new Size(110, 40);
            btnEditarMovi.TabIndex = 10;
            btnEditarMovi.Text = "✏️ Editar";
            // 
            // txtBuscaMovi
            // 
            txtBuscaMovi.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            txtBuscaMovi.BorderColor = Color.FromArgb(214, 189, 119);
            txtBuscaMovi.BorderRadius = 8;
            txtBuscaMovi.CustomizableEdges = customizableEdges5;
            txtBuscaMovi.DefaultText = "";
            txtBuscaMovi.FillColor = Color.FromArgb(33, 33, 33);
            txtBuscaMovi.FocusedState.BorderColor = Color.FromArgb(0, 123, 204);
            txtBuscaMovi.Font = new Font("Segoe UI", 9.5F);
            txtBuscaMovi.ForeColor = Color.FromArgb(214, 189, 119);
            txtBuscaMovi.Location = new Point(304, 19);
            txtBuscaMovi.Name = "txtBuscaMovi";
            txtBuscaMovi.PlaceholderForeColor = Color.FromArgb(214, 189, 119);
            txtBuscaMovi.PlaceholderText = "🔍  Pesquisar";
            txtBuscaMovi.SelectedText = "";
            txtBuscaMovi.ShadowDecoration.CustomizableEdges = customizableEdges6;
            txtBuscaMovi.Size = new Size(217, 40);
            txtBuscaMovi.TabIndex = 10;
            txtBuscaMovi.TextChanged += txtBuscaMovi_TextChanged;
            // 
            // lblSubtitulo
            // 
            lblSubtitulo.Font = new Font("Segoe UI", 9F);
            lblSubtitulo.ForeColor = Color.FromArgb(214, 189, 119);
            lblSubtitulo.Location = new Point(8, 43);
            lblSubtitulo.Name = "lblSubtitulo";
            lblSubtitulo.Size = new Size(238, 20);
            lblSubtitulo.TabIndex = 3;
            lblSubtitulo.Text = "Gerencie as movimentações do sistema";
            // 
            // lblTitulo
            // 
            lblTitulo.Font = new Font("Century Gothic", 14F, FontStyle.Bold);
            lblTitulo.ForeColor = Color.FromArgb(214, 189, 119);
            lblTitulo.Location = new Point(8, 19);
            lblTitulo.Name = "lblTitulo";
            lblTitulo.Size = new Size(225, 34);
            lblTitulo.TabIndex = 2;
            lblTitulo.Text = "📦  Movimentações";
            // 
            // panelConteudo
            // 
            panelConteudo.BackColor = Color.FromArgb(64, 64, 64);
            panelConteudo.Controls.Add(dgvMovimentacoes);
            panelConteudo.CustomizableEdges = customizableEdges9;
            panelConteudo.Location = new Point(0, 80);
            panelConteudo.Name = "panelConteudo";
            panelConteudo.ShadowDecoration.CustomizableEdges = customizableEdges10;
            panelConteudo.Size = new Size(969, 469);
            panelConteudo.TabIndex = 2;
            // 
            // dgvMovimentacoes
            // 
            dataGridViewCellStyle1.BackColor = Color.White;
            dgvMovimentacoes.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            dgvMovimentacoes.BackgroundColor = Color.Black;
            dataGridViewCellStyle2.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = Color.FromArgb(100, 88, 255);
            dataGridViewCellStyle2.Font = new Font("Segoe UI", 9F);
            dataGridViewCellStyle2.ForeColor = Color.White;
            dataGridViewCellStyle2.SelectionBackColor = SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = DataGridViewTriState.True;
            dgvMovimentacoes.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
            dgvMovimentacoes.ColumnHeadersHeight = 4;
            dgvMovimentacoes.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.EnableResizing;
            dataGridViewCellStyle3.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = Color.White;
            dataGridViewCellStyle3.Font = new Font("Segoe UI", 9F);
            dataGridViewCellStyle3.ForeColor = Color.FromArgb(71, 69, 94);
            dataGridViewCellStyle3.SelectionBackColor = Color.FromArgb(231, 229, 255);
            dataGridViewCellStyle3.SelectionForeColor = Color.FromArgb(71, 69, 94);
            dataGridViewCellStyle3.WrapMode = DataGridViewTriState.False;
            dgvMovimentacoes.DefaultCellStyle = dataGridViewCellStyle3;
            dgvMovimentacoes.GridColor = Color.FromArgb(231, 229, 255);
            dgvMovimentacoes.Location = new Point(20, 6);
            dgvMovimentacoes.Name = "dgvMovimentacoes";
            dgvMovimentacoes.RowHeadersVisible = false;
            dgvMovimentacoes.Size = new Size(930, 450);
            dgvMovimentacoes.TabIndex = 0;
            dgvMovimentacoes.ThemeStyle.AlternatingRowsStyle.BackColor = Color.White;
            dgvMovimentacoes.ThemeStyle.AlternatingRowsStyle.Font = null;
            dgvMovimentacoes.ThemeStyle.AlternatingRowsStyle.ForeColor = Color.Empty;
            dgvMovimentacoes.ThemeStyle.AlternatingRowsStyle.SelectionBackColor = Color.Empty;
            dgvMovimentacoes.ThemeStyle.AlternatingRowsStyle.SelectionForeColor = Color.Empty;
            dgvMovimentacoes.ThemeStyle.BackColor = Color.Black;
            dgvMovimentacoes.ThemeStyle.GridColor = Color.FromArgb(231, 229, 255);
            dgvMovimentacoes.ThemeStyle.HeaderStyle.BackColor = Color.FromArgb(100, 88, 255);
            dgvMovimentacoes.ThemeStyle.HeaderStyle.BorderStyle = DataGridViewHeaderBorderStyle.None;
            dgvMovimentacoes.ThemeStyle.HeaderStyle.Font = new Font("Segoe UI", 9F);
            dgvMovimentacoes.ThemeStyle.HeaderStyle.ForeColor = Color.White;
            dgvMovimentacoes.ThemeStyle.HeaderStyle.HeaightSizeMode = DataGridViewColumnHeadersHeightSizeMode.EnableResizing;
            dgvMovimentacoes.ThemeStyle.HeaderStyle.Height = 4;
            dgvMovimentacoes.ThemeStyle.ReadOnly = false;
            dgvMovimentacoes.ThemeStyle.RowsStyle.BackColor = Color.White;
            dgvMovimentacoes.ThemeStyle.RowsStyle.BorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            dgvMovimentacoes.ThemeStyle.RowsStyle.Font = new Font("Segoe UI", 9F);
            dgvMovimentacoes.ThemeStyle.RowsStyle.ForeColor = Color.FromArgb(71, 69, 94);
            dgvMovimentacoes.ThemeStyle.RowsStyle.Height = 25;
            dgvMovimentacoes.ThemeStyle.RowsStyle.SelectionBackColor = Color.FromArgb(231, 229, 255);
            dgvMovimentacoes.ThemeStyle.RowsStyle.SelectionForeColor = Color.FromArgb(71, 69, 94);
            // 
            // ucMovimentacoes
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            Controls.Add(panelConteudo);
            Controls.Add(panelHeader);
            Name = "ucMovimentacoes";
            Size = new Size(969, 549);
            panelHeader.ResumeLayout(false);
            panelConteudo.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)dgvMovimentacoes).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private Guna.UI2.WinForms.Guna2Panel panelHeader;
        private Label lblSubtitulo;
        private Label lblTitulo;
        private Guna.UI2.WinForms.Guna2TextBox txtBuscaMovi;
        private Guna.UI2.WinForms.Guna2Button btnNovaMovi;
        private Guna.UI2.WinForms.Guna2Button btnEditarMovi;
        private Guna.UI2.WinForms.Guna2Panel panelConteudo;
        private Guna.UI2.WinForms.Guna2DataGridView dgvMovimentacoes;
    }
}
